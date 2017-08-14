using ConFin.Common.Domain;
using ConFin.Common.Domain.Dto;
using System;
using System.Net;
using System.Net.Mail;

namespace ConFin.Domain.Login
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;
        private readonly Notification _notification;

        public LoginService(Notification notification, ILoginRepository loginRepository)
        {
            _notification = notification;
            _loginRepository = loginRepository;
        }

        public UsuarioDto Get(string email, string senha = null)
        {
            var usuario = _loginRepository.Get(email, senha);

            if (usuario == null)
            {
                _notification.Add("Usuário não encontrado :(");
                return null;
            }

            if (_notification.Any)
                return null;

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
                          $"<p><a href=\"http://localhost:5001/Home/Login/GetConfirmacao?idUsuario={usuario.Id}\" target=\"_blank\">LINK PARA CONFIRMAR CADASTRO (CLIQUE AQUI)</a></p>" +
                          "<p>Este é um e-mail automático. Não é necessário respondê-lo</p>" +
                          "<p>Atenciosamente,</br>ConFin - Controle Financeiro Pessoal </p>";

            EnviaEmail(usuario, body, "Confirmação de Cadastro");
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
                       $"<p><a href=\"http://localhost:5001/Login/RedefinirSenha?idUsuario={usuario.Id}&token={token}\"" +
                            " target=\"_blank\">LINK PARA REDEFINIR SENHA (CLIQUE AQUI)</a></p>" +
                        "<p>Este é um e-mail automático. Não é necessário respondê-lo</p>" +
                        "<p>Atenciosamente,</br>ConFin - Controle Financeiro Pessoal </p>";

            EnviaEmail(usuario, body, "Alteração de Senha");
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
