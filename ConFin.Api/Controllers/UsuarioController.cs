using ConFin.Common.Domain;
using ConFin.Domain.Usuario;
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
            var usuario = _usuarioRepository.Get(id);
            if (!_notification.Any)
                return Ok(usuario);

            return Content(HttpStatusCode.BadRequest, _notification.Get);
        }

        public IHttpActionResult PutSenha(int id, string token, string novaSenha)
        {
            _usuarioService.PutSenha(id, token, novaSenha);
            if (!_notification.Any)
                return Ok();

            return Content(HttpStatusCode.BadRequest, _notification.Get);
        }
    }
}
