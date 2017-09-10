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
        private readonly ILancamentoService _lancamentoService;
        private readonly Notification _notification;

        public LancamentoController(Notification notification, ILancamentoRepository lancamentoRepository, ILancamentoService lancamentoService)
        {
            _notification = notification;
            _lancamentoRepository = lancamentoRepository;
            _lancamentoService = lancamentoService;
        }

        public IHttpActionResult GetAll(int idUsuario, byte? mes = null, short? ano = null, int? idConta = null, int? idCategoria = null)
        {
            var lancamentos = _lancamentoService.GetAll(idUsuario, mes, ano, idConta, idCategoria);
            if(!_notification.Any)
                return Ok(lancamentos);

            return Content(HttpStatusCode.BadRequest, _notification.Get);

        }

        public IHttpActionResult Get(int idLancamento) => Ok(_lancamentoRepository.Get(idLancamento));

        public IHttpActionResult Post(LancamentoDto lancamento)
        {
            _lancamentoService.Post(lancamento);
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

        public IHttpActionResult Delete(int idLancamento, string indTipoDelete)
        {
            _lancamentoService.Delete(idLancamento, indTipoDelete);
            if (_notification.Any)
                return Content(HttpStatusCode.BadRequest, _notification.Get);

            var msg = string.IsNullOrEmpty(indTipoDelete) || indTipoDelete == "U"
                ? "Lançamento excluido com sucesso!"
                : indTipoDelete == "P"
                    ? "Foi excluido com sucesso este e os próximo lançamentos fixo/parcelado vínculados"
                    : "Foi excluido com sucesso este e todos os lançamentos fixo/parcelado vínculados";

            return Ok(msg);

        }

        public IHttpActionResult PutIndicadorPagoRecebido(LancamentoDto lancamento)
        {
            _lancamentoRepository.PutIndicadorPagoRecebido(lancamento);
            if (!_notification.Any)
                return Ok();

            return Content(HttpStatusCode.BadRequest, _notification.Get);
        }

        public IHttpActionResult GetResumo(int idUsuario, byte mes, short ano, int? idConta = null, int? idCategoria = null) 
            => Ok(_lancamentoRepository.GetResumo(idUsuario, mes, ano, idConta, idCategoria));

        public IHttpActionResult GetPeriodo() => Ok(_lancamentoRepository.GetPeriodo());

    }
}
