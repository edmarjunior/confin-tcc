using ConFin.Common.Domain.Dto;
using System.Net.Http;

namespace ConFin.Application.AppService.Transferencia
{
    public interface ITransferenciaAppService
    {
        HttpResponseMessage GetAll(int idUsuario);
        HttpResponseMessage Get(int idTransferencia);
        HttpResponseMessage Post(TransferenciaDto transferencia);
        HttpResponseMessage Put(TransferenciaDto transferencia);
        HttpResponseMessage Delete(int idTransferencia);
        HttpResponseMessage PutIndicadorPagoRecebido(TransferenciaDto transferencia);
    }
}
