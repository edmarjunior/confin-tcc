using ConFin.Common.Domain;
using ConFin.Common.Domain.Dto;
using ConFin.Domain.ContaConjunta;
using System.Net;
using System.Web.Http;

namespace ConFin.Api.Controllers
{
    public class ContaConjuntaController: ApiController
    {
        private readonly IContaConjuntaRepository _contaConjuntaRepository;
        private readonly IContaConjuntaService _contaConjuntaService;
        private readonly Notification _notification;


        public ContaConjuntaController(IContaConjuntaRepository contaConjuntaRepository, IContaConjuntaService contaConjuntaService, Notification notification)
        {
            _contaConjuntaRepository = contaConjuntaRepository;
            _contaConjuntaService = contaConjuntaService;
            _notification = notification;
        }

        public IHttpActionResult Get(int? idUsuario, int? idConta = null) => Ok(_contaConjuntaRepository.Get(idUsuario, idConta));

        public IHttpActionResult Post(ContaConjuntaDto contaConjunta)
        {
            _contaConjuntaService.Post(contaConjunta);
            if (!_notification.Any)
                return Ok();

            return Content(HttpStatusCode.BadRequest, _notification.Get);
        }

        public IHttpActionResult Delete(int idContaConjunta)
        {
            _contaConjuntaRepository.Delete(idContaConjunta);
            if (!_notification.Any)
                return Ok();

            return Content(HttpStatusCode.BadRequest, _notification.Get);
        }

        public IHttpActionResult Put(ContaConjuntaDto contaConjunta)
        {
            _contaConjuntaService.Put(contaConjunta);
            if (!_notification.Any)
                return Ok();

            return Content(HttpStatusCode.BadRequest, _notification.Get);
        }
    }
}