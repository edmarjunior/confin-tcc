using ConFin.Common.Domain;
using ConFin.Common.Domain.Dto;
using ConFin.Domain.ContaFinanceira;
using ConFin.Domain.LancamentoCategoria;
using System;
using System.Net;
using System.Net.Mail;

namespace ConFin.Domain.Login
{
    public class LoginService : ILoginService
    {
        public readonly ILoginRepository LoginRepository;
        public readonly IContaFinanceiraRepository ContaFinanceiraRepository;
        public readonly ILancamentoCategoriaRepository LancamentoCategoriaRepository;
        public readonly Notification Notification;
        private static Parameters _parameters;

        public LoginService(Notification notification, ILoginRepository loginRepository, Parameters parameters, IContaFinanceiraRepository contaFinanceiraRepository, ILancamentoCategoriaRepository lancamentoCategoriaRepository)
        {
            Notification = notification;
            LoginRepository = loginRepository;
            _parameters = parameters;
            ContaFinanceiraRepository = contaFinanceiraRepository;
            LancamentoCategoriaRepository = lancamentoCategoriaRepository;
        }

        public UsuarioDto Get(string email, string senha = null)
        {
            var usuario = LoginRepository.Get(email, senha);

            if (usuario == null)
            {
                Notification.Add("Usuário não encontrado :(");
                return null;
            }

            if (usuario.DataConfirmCadastro.HasValue)
                return usuario;

            Notification.Add("Aguardando usuário confirmar cadastro");
            return null;
        }

        public void Post(UsuarioDto usuario)
        {
            if (usuario == null)
            {
                Notification.Add("Usuário não enviado para cadastro");
                return;
            }

            if (!usuario.IsValid(Notification))
                return;

            if (LoginRepository.Get(usuario.Email) != null)
            {
                Notification.Add("E-mail de usuário já cadastrado");
                return;
            }

            LoginRepository.OpenTransaction();
            LoginRepository.Post(usuario);
            EnviaEmailConfirmacaoCadastro(usuario);
            LoginRepository.CommitTransaction();
        }

        private static void EnviaEmailConfirmacaoCadastro(UsuarioDto usuario)
        {

            var body = $"<p>Prezado(a) {usuario.Nome.ToUpper()} </p>" +
                          "<p>Parabéns por tomar a decisão de ter um maior Controle Financeiro Pessoal ao utilizar nossos serviços</p>" +
                          "<p>Para confirmar seu acesso, favor clicar no link abaixo</p>" +
                          $"<p><a href=\"{_parameters.UriWeb}Login/GetConfirmacao?idUsuario={usuario.Id}\" target=\"_blank\">LINK PARA CONFIRMAR CADASTRO (CLIQUE AQUI)</a></p>" +
                          "<p>Este é um e-mail automático. Não é necessário respondê-lo</p>" +
                          "<p>Atenciosamente,</br>ConFin - Controle Financeiro Pessoal </p>";

            EnviaEmail(usuario, body, "Confirmação de Cadastro");
        }

        public void PostReenviarSenha(string email)
        {

            var usuario = Get(email);
            if (Notification.Any)
                return;

            var token = Guid.NewGuid().ToString();

            LoginRepository.OpenTransaction();
            LoginRepository.PostSolicitacaoTrocaSenhaLogin(usuario.Id, token);

            if (Notification.Any)
            {
                LoginRepository.RollbackTransaction();
                return;
            }

            var body = $"<p>Prezado(a) {usuario.Nome.ToUpper()} </p>" +
                        "<p>É normal as vezes esquecermos nossas credenciais de acesso" +
                        "<p>Para inserir uma nova senha de acesso, favor clicar no link abaixo</p>" +
                       $"<p><a href=\"{_parameters.UriWeb}Login/RedefinirSenha?idUsuario={usuario.Id}&token={token}\"" +
                            " target=\"_blank\">LINK PARA REDEFINIR SENHA (CLIQUE AQUI)</a></p>" +
                        "<p>Este é um e-mail automático. Não é necessário respondê-lo</p>" +
                        "<p>Atenciosamente,</br>ConFin - Controle Financeiro Pessoal </p>";

            EnviaEmail(usuario, body, "Alteração de Senha");
            LoginRepository.CommitTransaction();

        }

        public void GetVerificaTokenValidoRedefinirSenha(int idUsuario, string token)
        {
            var dadosSolicitacao = LoginRepository.GetSolicitacaoTrocaSenhaLogin(idUsuario, token);
            if (dadosSolicitacao == null)
            {
                Notification.Add("Solicitação de redefinição de senha não encontrada, favor realizar nova solicitação.");
                return;
            }

            var tempoExpiracao = DateTime.Now - dadosSolicitacao.DataCadastro;

            if (tempoExpiracao.Minutes > 60)
                Notification.Add("A Solicitação de refinição de senha está expirada, favor realizar nova solicitação.");

        }

        public void PutConfirmacaoCadastro(int idUsuario)
        {
            LoginRepository.OpenTransaction();

            LoginRepository.PutConfirmacaoCadastro(idUsuario);

            ContaFinanceiraRepository.Post(new ContaFinanceiraDto
            {
                Nome = "Padrão",
                IdTipo = 3, // Carteira
                ValorSaldoInicial = 0,
                IdUsuarioCadastro = idUsuario
            });

            LancamentoCategoriaRepository.PostCategoriasIniciaisUsuario(idUsuario);

            if(!Notification.Any)
                LoginRepository.CommitTransaction();
            else
                LoginRepository.RollbackTransaction();
        }

        private static void EnviaEmail(UsuarioDto usuario, string body, string subject)
        {
            var client = new SmtpClient()
            {
                Host = "smtp-mail.outlook.com",
                EnableSsl = true,
                Credentials = new NetworkCredential("confinpessoal@outlook.com", "teste321"),
                Port = 587
            };

            var mail = new MailMessage
            {
                Sender = new MailAddress(usuario.Email, usuario.Nome),
                From = new MailAddress("confinpessoal@outlook.com", "ConFin automático"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mail.To.Add(new MailAddress(usuario.Email, usuario.Nome));
            client.Send(mail);
        }
    }
}
