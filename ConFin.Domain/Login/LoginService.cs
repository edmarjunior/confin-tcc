using ConFin.Common.Domain;
using System.Net;
using System.Net.Mail;

namespace ConFin.Domain.Login
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _usuarioRepository;
        private readonly Notification _notification;


        public LoginService(ILoginRepository usuarioRepository, Notification notification)
        {
            _usuarioRepository = usuarioRepository;
            _notification = notification;
        }

        public Usuario Get(string email, string senha = null)
        {
            var usuario = _usuarioRepository.Get(email, senha);

            if (usuario == null)
            {
                _notification.Add("Usuário não encontrado :(");
                return null;
            }

            if (usuario.DataConfirmCadastro.HasValue)
                return usuario;

            _notification.Add("Aguardando usuário confirmar cadastro :(");
            return null;
        }

        public void Post(Usuario usuario)
        {
            if (usuario == null)
            {
                _notification.Add("Usuário não enviado para cadastro");
                return;
            }

            if (!usuario.IsValid(_notification))
                return;

            if (_usuarioRepository.Get(usuario.Email) != null)
            {
                _notification.Add("E-mail de usuário já cadastrado");
                return;
            }

            _usuarioRepository.OpenTransaction();
            _usuarioRepository.Post(usuario);
            EnviaEmailConfirmacaoCadastro(usuario);
            _usuarioRepository.CommitTransaction();
        }

        private static void EnviaEmailConfirmacaoCadastro(Usuario usuario)
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
                Subject = "Confirmação de Cadastro",
                Body = $"<p>Prezado(a) {usuario.Nome.ToUpper()} </p>" +
                        "<p>Parabéns por tomar a decisão de ter um maior Controle Financeiro Pessoal ao utilizar nossos serviços</p>" +
                        "<p>Para confirmar seu acesso, favor clicar no link abaixo</p>" +
                        $"<p><a href=\"http://localhost:5001/Login/GetConfirmacao?idUsuario={usuario.Id}\" target=\"_blank\">LINK PARA CONFIRMAR CADASTRO (CLIQUE AQUI)</a></p>" +
                        "<p>Este é um e-mail automático. Não é necessário respondê-lo</p>" +
                        "<p>Atenciosamente,</br>ConFin - Controle Financeiro Pessoal </p>",
                IsBodyHtml = true,
            };

            mail.To.Add(new MailAddress(usuario.Email, usuario.Nome));
            client.Send(mail);
        }

    }
}
