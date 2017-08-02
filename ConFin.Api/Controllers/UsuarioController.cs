using ConFin.Common.Domain;
using ConFin.Domain.Usuario;
using ConFin.Domain.Usuario.Dto;
using System;
using System.Net;
using System.Web.Http;

namespace ConFin.Api.Controllers
{
    public class UsuarioController: ApiController
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IUsuarioRepository _usuarioRepository;
        private Notification _notification;

        public UsuarioController(IUsuarioService usuarioService, IUsuarioRepository usuarioRepository, Notification notification)
        {
            _usuarioService = usuarioService;
            _usuarioRepository = usuarioRepository;
            _notification = notification;
        }

        public IHttpActionResult Post(UsuarioDto usuario)
        {
            try
            {
                _usuarioService.Post(usuario);

                if (!_notification.Any)
                    return Ok("Cadastro solicitado com sucesso! Acesse a conta de e-mail informada para finalizar o cadastro");

                return Content(HttpStatusCode.BadRequest, _notification.Get);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        public IHttpActionResult GetConfirmacaoCadastro(int idUsuario)
        {
            try
            {
                _usuarioRepository.PutConfirmacaoCadastro(idUsuario);

                if(!_notification.Any)
                    return Ok();

                return Content(HttpStatusCode.BadRequest, _notification.Get);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public IHttpActionResult Get(string email, string senha)
        {
            try
            {
                return Ok(_usuarioRepository.Get(email, senha));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

    }
}