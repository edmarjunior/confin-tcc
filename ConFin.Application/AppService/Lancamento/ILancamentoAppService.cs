using ConFin.Common.Domain.Dto;
using System.Net.Http;

namespace ConFin.Application.AppService.Lancamento
{
    public interface ILancamentoAppService
    {
        HttpResponseMessage GetAll(int idUsuario, byte? mes = null, short? ano = null, int? idConta = null, int? idCategoria = null);
        HttpResponseMessage Get(int idLancamento);
        HttpResponseMessage Post(LancamentoDto lancamento);
        HttpResponseMessage Put(LancamentoDto lancamento);
        HttpResponseMessage Delete(int idLancamento);
        HttpResponseMessage PutIndicadorPagoRecebido(LancamentoDto lancamento);
        HttpResponseMessage GetResumo(int idUsuario, byte mes, short ano, int? idConta = null, int? idCategoria = null);
        HttpResponseMessage GetPeriodo();

    }
}
