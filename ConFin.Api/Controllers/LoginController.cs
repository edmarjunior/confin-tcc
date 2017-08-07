using ConFin.Common.Domain;
using ConFin.Domain.Login;
using System;
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
            try
            {
                var usuario = _loginService.Get(email, senha);
                if(!_notification.Any)
                    return Ok(usuario);

                return Content(HttpStatusCode.BadRequest, _notification.Get);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public IHttpActionResult Post(Usuario usuario)
        {
            try
            {
                _loginService.Post(usuario);

                if (!_notification.Any)
                    return Ok("Cadastro solicitado com sucesso! Acesse a conta de e-mail informada para finalizar o cadastro");

                return Content(HttpStatusCode.BadRequest, _notification.Get);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        public IHttpActionResult PutConfirmacaoCadastro(int idUsuario)
        {
            try
            {
                _loginRepository.PutConfirmacaoCadastro(idUsuario);

                if(!_notification.Any)
                    return Ok();

                return Content(HttpStatusCode.BadRequest, _notification.Get);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public IHttpActionResult PostReenviarSenha(string email)
        {
            try
            {
                _loginService.PostReenviarSenha(email);

                if (!_notification.Any)
                    return Ok();

                return Content(HttpStatusCode.BadRequest, _notification.Get);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public IHttpActionResult GetVerificaTokenValidoRedefinirSenha(int idUsuario, string token)
        {
            try
            {
                _loginService.GetVerificaTokenValidoRedefinirSenha(idUsuario, token);

                if (!_notification.Any)
                    return Ok();

                return Content(HttpStatusCode.BadRequest, _notification.Get);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        

    }
}
