using System.Net;
using ConFin.Common.Domain;
using ConFin.Common.Domain.Dto;
using ConFin.Domain.LancamentoCategoria;
using System.Web.Http;

namespace ConFin.Api.Controllers
{
    public class LancamentoCategoriaController: ApiController
    {
        private readonly ILancamentoCategoriaRepository _lancamentoCategoriaRepository;
        private readonly Notification _notification;

        public LancamentoCategoriaController(ILancamentoCategoriaRepository lancamentoCategoriaRepository, Notification notification)
        {
            _lancamentoCategoriaRepository = lancamentoCategoriaRepository;
            _notification = notification;
        }

        public IHttpActionResult Get(int idUsuario) => Ok(_lancamentoCategoriaRepository.Get(idUsuario));

        public IHttpActionResult Get(int idUsuario, int idCategoria) => Ok(_lancamentoCategoriaRepository.Get(idUsuario, idCategoria));

        public IHttpActionResult Post(LancamentoCategoriaDto categoria)
        {
            _lancamentoCategoriaRepository.Post(categoria);
            if (!_notification.Any)
                return Ok();

            return Content(HttpStatusCode.BadRequest, _notification.Get);
        }

        public IHttpActionResult Put(LancamentoCategoriaDto categoria)
        {
            _lancamentoCategoriaRepository.Put(categoria);
            if (!_notification.Any)
                return Ok();

            return Content(HttpStatusCode.BadRequest, _notification.Get);
        }

        public IHttpActionResult Delete(int idUsuario, int idCategoria)
        {
            _lancamentoCategoriaRepository.Delete(idUsuario, idCategoria);
            if (!_notification.Any)
                return Ok();

            return Content(HttpStatusCode.BadRequest, _notification.Get);
        }
    }
}