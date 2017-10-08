using ConFin.Common.Domain;
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

        public IHttpActionResult Post(int idConta, int idUsuarioEnvio, string emailUsuarioConvidado)
        {
            _contaConjuntaService.Post(idConta, idUsuarioEnvio, emailUsuarioConvidado);
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

        public IHttpActionResult Put(int idContaConjunta, string indicadorAprovado)
        {
            _contaConjuntaRepository.Put(idContaConjunta, indicadorAprovado);
            if (!_notification.Any)
                return Ok();

            return Content(HttpStatusCode.BadRequest, _notification.Get);
        }
    }
}