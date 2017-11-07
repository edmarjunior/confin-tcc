using ConFin.Common.Domain;
using ConFin.Common.Domain.Auxiliar;
using ConFin.Common.Domain.Dto;
using ConFin.Domain.ContaFinanceira;
using ConFin.Domain.LancamentoCategoria;
using System;

namespace ConFin.Domain.Login
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IContaFinanceiraRepository _contaFinanceiraRepository;
        private readonly ILancamentoCategoriaRepository _lancamentoCategoriaRepository;
        private readonly Notification _notification;
        private static Parameters _parameters;

        public LoginService(Notification notification, ILoginRepository loginRepository, Parameters parameters, IContaFinanceiraRepository contaFinanceiraRepository, ILancamentoCategoriaRepository lancamentoCategoriaRepository)
        {
            _notification = notification;
            _loginRepository = loginRepository;
            _parameters = parameters;
            _contaFinanceiraRepository = contaFinanceiraRepository;
            _lancamentoCategoriaRepository = lancamentoCategoriaRepository;
        }

        public UsuarioDto Get(string email, string senha = null)
        {
            var usuario = _loginRepository.Get(email, senha);

            if (usuario == null)
            {
                _notification.Add("Usuário não encontrado :(");
                return null;
            }

            if (usuario.DataConfirmCadastro.HasValue)
                return usuario;

            _notification.Add("Aguardando usuário confirmar cadastro");
            return null;
        }

        public void Post(UsuarioDto usuario)
        {
            if (usuario == null)
            {
                _notification.Add("Usuário não enviado para cadastro");
                return;
            }

            if (!usuario.IsValid(_notification))
                return;

            if (_loginRepository.Get(usuario.Email) != null)
            {
                _notification.Add("E-mail de usuário já cadastrado");
                return;
            }

            _loginRepository.OpenTransaction();
            _loginRepository.Post(usuario);
            EnviaEmailConfirmacaoCadastro(usuario);
            _loginRepository.CommitTransaction();
        }

        private static void EnviaEmailConfirmacaoCadastro(UsuarioDto usuario)
        {

            var body = $"<p>Prezado(a) {usuario.Nome.ToUpper()} </p>" +
                          "<p>Parabéns por tomar a decisão de ter um maior Controle Financeiro Pessoal ao utilizar nossos serviços</p>" +
                          "<p>Para confirmar seu acesso, favor clicar no link abaixo</p>" +
                          $"<p><a href=\"{_parameters.UriWeb}Login/GetConfirmacao?idUsuario={usuario.Id}\" target=\"_blank\">LINK PARA CONFIRMAR CADASTRO (CLIQUE AQUI)</a></p>" +
                          "<p>Este é um e-mail automático. Não é necessário respondê-lo</p>" +
                          "<p>Atenciosamente,</br>ConFin - Controle Financeiro Pessoal </p>";

            Email.Enviar("Confirmação de Cadastro", usuario.Email, body, usuario.Nome);
        }

        public void PostReenviarSenha(string email)
        {
            var usuario = Get(email);
            if (_notification.Any)
                return;

            var token = Guid.NewGuid().ToString();

            _loginRepository.OpenTransaction();
            _loginRepository.PostSolicitacaoTrocaSenhaLogin(usuario.Id, token);

            if (_notification.Any)
            {
                _loginRepository.RollbackTransaction();
                return;
            }

            var body = $"<p>Prezado(a) {usuario.Nome.ToUpper()} </p>" +
                        "<p>É normal as vezes esquecermos nossas credenciais de acesso" +
                        "<p>Para inserir uma nova senha de acesso, favor clicar no link abaixo</p>" +
                       $"<p><a href=\"{_parameters.UriWeb}Login/RedefinirSenha?idUsuario={usuario.Id}&token={token}\"" +
                            " target=\"_blank\">LINK PARA REDEFINIR SENHA (CLIQUE AQUI)</a></p>" +
                        "<p>Este é um e-mail automático. Não é necessário respondê-lo</p>" +
                        "<p>Atenciosamente,</br>ConFin - Controle Financeiro Pessoal </p>";

            Email.Enviar("Alteração de Senha", usuario.Email, body, usuario.Nome);
            _loginRepository.CommitTransaction();
        }

        public void GetVerificaTokenValidoRedefinirSenha(int idUsuario, string token)
        {
            var dadosSolicitacao = _loginRepository.GetSolicitacaoTrocaSenhaLogin(idUsuario, token);
            if (dadosSolicitacao == null)
            {
                _notification.Add("Solicitação de redefinição de senha não encontrada, favor realizar nova solicitação.");
                return;
            }

            var tempoExpiracao = DateTime.Now - dadosSolicitacao.DataCadastro;

            if (tempoExpiracao.Minutes > 60)
                _notification.Add("A Solicitação de refinição de senha está expirada, favor realizar nova solicitação.");

        }

        public void PutConfirmacaoCadastro(int idUsuario)
        {
            _loginRepository.OpenTransaction();

            _loginRepository.PutConfirmacaoCadastro(idUsuario);

            _contaFinanceiraRepository.Post(new ContaFinanceiraDto
            {
                Nome = "Padrão",
                IdTipo = 3, // Carteira
                ValorSaldoInicial = 0,
                IdUsuarioCadastro = idUsuario
            });

            _lancamentoCategoriaRepository.PostCategoriasIniciaisUsuario(idUsuario);

            if(!_notification.Any)
                _loginRepository.CommitTransaction();
            else
                _loginRepository.RollbackTransaction();
        }

    }
}
