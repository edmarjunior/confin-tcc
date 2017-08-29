using ConFin.Common.Domain;
using ConFin.Common.Domain.Dto;
using ConFin.Domain.Transferencia;
using System.Net;
using System.Web.Http;

namespace ConFin.Api.Controllers
{
    public class TransferenciaController: ApiController
    {

        private readonly ITransferenciaRepository _transferenciaRepository;
        private readonly Notification _notification;

        public TransferenciaController(ITransferenciaRepository transferenciaRepository, Notification notification)
        {
            _transferenciaRepository = transferenciaRepository;
            _notification = notification;
        }

        public IHttpActionResult GetAll(int idUsuario) => Ok(_transferenciaRepository.GetAll(idUsuario));

        public IHttpActionResult Get(int idTransferencia) => Ok(_transferenciaRepository.Get(idTransferencia));

        public IHttpActionResult Post(TransferenciaDto transferencia)
        {
            _transferenciaRepository.Post(transferencia);
            if (!_notification.Any)
                return Ok();

            return Content(HttpStatusCode.BadRequest, _notification.Get);
        }

        public IHttpActionResult Put(TransferenciaDto transferencia)
        {
            _transferenciaRepository.Put(transferencia);
            if (!_notification.Any)
                return Ok();

            return Content(HttpStatusCode.BadRequest, _notification.Get);
        }

        public IHttpActionResult Delete(int idTransferencia)
        {
            _transferenciaRepository.Delete(idTransferencia);
            if (!_notification.Any)
                return Ok();

            return Content(HttpStatusCode.BadRequest, _notification.Get);
        }

        public IHttpActionResult PutIndicadorPagoRecebido(TransferenciaDto transferencia)
        {
            _transferenciaRepository.PutIndicadorPagoRecebido(transferencia);
            if (!_notification.Any)
                return Ok();

            return Content(HttpStatusCode.BadRequest, _notification.Get);
        }

        public IHttpActionResult GetVerificaClientePossuiTransferenciaHabilitada(int idUsuario)
        {
            if (_transferenciaRepository.GetVerificaClientePossuiTransferenciaHabilitada(idUsuario))
                return Ok();

            return Content(HttpStatusCode.NotAcceptable, "O cliente não possui a opção de fazer transferência");
        }

    }
}