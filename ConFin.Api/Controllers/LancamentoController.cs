using ConFin.Common.Domain;
using ConFin.Common.Domain.Dto;
using ConFin.Domain.Lancamento;
using System.Net;
using System.Web.Http;

namespace ConFin.Api.Controllers
{
    public class LancamentoController: ApiController
    {
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly Notification _notification;

        public LancamentoController(Notification notification, ILancamentoRepository lancamentoRepository)
        {
            _notification = notification;
            _lancamentoRepository = lancamentoRepository;
        }

        public IHttpActionResult GetAll(int idUsuario, int? idConta = null, int? idCategoria = null) 
            => Ok(_lancamentoRepository.GetAll(idUsuario, idConta, idCategoria));

        public IHttpActionResult Get(int idLancamento) => Ok(_lancamentoRepository.Get(idLancamento));

        public IHttpActionResult Post(LancamentoDto lancamento)
        {
            _lancamentoRepository.Post(lancamento);
            if (!_notification.Any)
                return Ok();

            return Content(HttpStatusCode.BadRequest, _notification.Get);
        }

        public IHttpActionResult Put(LancamentoDto lancamento)
        {
            _lancamentoRepository.Put(lancamento);
            if (!_notification.Any)
                return Ok();

            return Content(HttpStatusCode.BadRequest, _notification.Get);
        }

        public IHttpActionResult Delete(int idLancamento)
        {
            _lancamentoRepository.Delete(idLancamento);
            if (!_notification.Any)
                return Ok();

            return Content(HttpStatusCode.BadRequest, _notification.Get);
        }

        public IHttpActionResult PutIndicadorPagoRecebido(LancamentoDto lancamento)
        {
            _lancamentoRepository.PutIndicadorPagoRecebido(lancamento);
            if (!_notification.Any)
                return Ok();

            return Content(HttpStatusCode.BadRequest, _notification.Get);
        }

        public IHttpActionResult GetResumo(int idUsuario, int? idConta = null, int? idCategoria = null) 
            => Ok(_lancamentoRepository.GetResumo(idUsuario, idConta, idCategoria));
    }
}
