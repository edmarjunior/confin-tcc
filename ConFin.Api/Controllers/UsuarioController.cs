using ConFin.Common.Domain;
using ConFin.Domain.Usuario;
using System;
using System.Net;
using System.Web.Http;

namespace ConFin.Api.Controllers
{
    public class UsuarioController: ApiController
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly Notification _notification;

        public UsuarioController(Notification notification, IUsuarioRepository usuarioRepository, IUsuarioService usuarioService)
        {
            _notification = notification;
            _usuarioRepository = usuarioRepository;
            _usuarioService = usuarioService;
        }

        public IHttpActionResult Get(int id)
        {
            try
            {
                var usuario = _usuarioRepository.Get(id);
                if (!_notification.Any)
                    return Ok(usuario);

                return Content(HttpStatusCode.BadRequest, _notification.Get);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }


        public IHttpActionResult PutSenha(int id, string token, string novaSenha)
        {
            try
            {
                _usuarioService.PutSenha(id, token, novaSenha);
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