using ConFin.Common.Application;
using ConFin.Common.Domain.Dto;
using System.Net.Http;

namespace ConFin.Application.AppService.Lancamento
{
    public class LancamentoAppService : BaseAppService, ILancamentoAppService
    {
        public LancamentoAppService() : base("Lancamento")
        {
        }

        public HttpResponseMessage GetAll(int idUsuario, byte? mes = null, short? ano = null, int? idConta = null, int? idCategoria = null)
        {
            return GetRequest("GetAll", new { idUsuario, mes, ano, idConta, idCategoria });
        }

        public HttpResponseMessage Get(int idLancamento)
        {
            return GetRequest(new { idLancamento });

        }

        public HttpResponseMessage Post(LancamentoDto lancamento)
        {
            return PostRequest(lancamento);

        }

        public HttpResponseMessage Put(LancamentoDto lancamento)
        {
            return PutRequest("Put", lancamento);

        }

        public HttpResponseMessage Delete(int idLancamento)
        {
            return DeleteRequest(new { idLancamento });
        }

        public HttpResponseMessage PutIndicadorPagoRecebido(LancamentoDto lancamento)
        {
            return PutRequest("PutIndicadorPagoRecebido", lancamento);
        }

        public HttpResponseMessage GetResumo(int idUsuario, byte mes, short ano, int? idConta = null, int? idCategoria = null)
        {
            return GetRequest("GetResumo", new { idUsuario, mes, ano, idConta, idCategoria });
        }

        public HttpResponseMessage GetPeriodo()
        {
            return GetRequest("GetPeriodo");
        }
    }
}
