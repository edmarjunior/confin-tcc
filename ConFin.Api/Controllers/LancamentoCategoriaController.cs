using ConFin.Common.Domain;
using ConFin.Common.Domain.Dto;
using ConFin.Domain.LancamentoCategoria;
using System.Net;
using System.Web.Http;

namespace ConFin.Api.Controllers
{
    public class LancamentoCategoriaController: ApiController
    {
        private readonly ILancamentoCategoriaRepository _lancamentoCategoriaRepository;
        private readonly ILancamentoCategoriaService _lancamentoCategoriaService;
        private readonly Notification _notification;

        public LancamentoCategoriaController(ILancamentoCategoriaRepository lancamentoCategoriaRepository, Notification notification, ILancamentoCategoriaService lancamentoCategoriaService)
        {
            _lancamentoCategoriaRepository = lancamentoCategoriaRepository;
            _notification = notification;
            _lancamentoCategoriaService = lancamentoCategoriaService;
        }

        public IHttpActionResult Get(int idUsuario) => Ok(_lancamentoCategoriaRepository.Get(idUsuario));

        public IHttpActionResult Get(int idUsuario, int idCategoria) => Ok(_lancamentoCategoriaRepository.Get(idUsuario, idCategoria));

        public IHttpActionResult Post(LancamentoCategoriaDto categoria)
        {
            _lancamentoCategoriaService.Post(categoria);
            if (!_notification.Any)
                return Ok();

            return Content(HttpStatusCode.BadRequest, _notification.Get);
        }

        public IHttpActionResult Put(LancamentoCategoriaDto categoria)
        {
            _lancamentoCategoriaService.Put(categoria);
            if (!_notification.Any)
                return Ok();

            return Content(HttpStatusCode.BadRequest, _notification.Get);
        }

        public IHttpActionResult Delete(int idUsuario, int idCategoria)
        {
            _lancamentoCategoriaService.Delete(idUsuario, idCategoria);
            if (!_notification.Any)
                return Ok();

            return Content(HttpStatusCode.BadRequest, _notification.Get);
        }
    }
}