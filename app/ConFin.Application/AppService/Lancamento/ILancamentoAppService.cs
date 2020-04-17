using ConFin.Common.Domain.Dto;
using System.Collections.Generic;
using System.Net.Http;

namespace ConFin.Application.AppService.Lancamento
{
    public interface ILancamentoAppService
    {
        HttpResponseMessage GetAll(int idUsuario, byte? mes = null, short? ano = null, int? idConta = null, int? idCategoria = null);
        HttpResponseMessage Get(int idLancamento);
        HttpResponseMessage Post(LancamentoDto lancamento);
        HttpResponseMessage Post(IEnumerable<LancamentoDto> lancamentos);
        HttpResponseMessage Put(LancamentoDto lancamento);
        HttpResponseMessage Delete(int idLancamento, string indTipoDelete, int idUsuario);
        HttpResponseMessage PutIndicadorPagoRecebido(LancamentoDto lancamento);
        HttpResponseMessage GetResumo(int idUsuario, byte mes, short ano, int? idConta = null, int? idCategoria = null);
        HttpResponseMessage GetPeriodo();
    }
}
