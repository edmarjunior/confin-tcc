using ConFin.Common.Domain;
using ConFin.Domain.Login;
using System.Net;
using System.Web.Http;

namespace ConFin.Api.Controllers
{
    public class LoginController: ApiController
    {
        private readonly ILoginService _loginService;
        private readonly ILoginRepository _loginRepository;
        private readonly Notification _notification;


        public LoginController(ILoginService loginService, ILoginRepository loginRepository, Notification notification)
        {
            _loginService = loginService;
            _loginRepository = loginRepository;
            _notification = notification;
        }

        public IHttpActionResult Get(string email, string senha)
        {
            var usuario = _loginService.Get(email, senha);
            if(!_notification.Any)
                return Ok(usuario);

            return Content(HttpStatusCode.BadRequest, _notification.Get);
        }

        public IHttpActionResult Post(Usuario usuario)
        {
            _loginService.Post(usuario);

            if (!_notification.Any)
                return Ok("Cadastro solicitado com sucesso! Acesse a conta de e-mail informada para finalizar o cadastro");

            return Content(HttpStatusCode.BadRequest, _notification.Get);
        }

        public IHttpActionResult PutConfirmacaoCadastro(int idUsuario)
        {
                _loginRepository.PutConfirmacaoCadastro(idUsuario);

                if(!_notification.Any)
                    return Ok();

                return Content(HttpStatusCode.BadRequest, _notification.Get);
        }

        public IHttpActionResult PostReenviarSenha(string email)
        {
                _loginService.PostReenviarSenha(email);

                if (!_notification.Any)
                    return Ok();

                return Content(HttpStatusCode.BadRequest, _notification.Get);
        }

        public IHttpActionResult GetVerificaTokenValidoRedefinirSenha(int idUsuario, string token)
        {
                _loginService.GetVerificaTokenValidoRedefinirSenha(idUsuario, token);

                if (!_notification.Any)
                    return Ok();

                return Content(HttpStatusCode.BadRequest, _notification.Get);
        }
    }
}
