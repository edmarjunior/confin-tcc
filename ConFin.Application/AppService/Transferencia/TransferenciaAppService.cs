using ConFin.Common.Application;
using ConFin.Common.Domain.Dto;
using System.Net.Http;

namespace ConFin.Application.AppService.Transferencia
{
    public class TransferenciaAppService : BaseAppService, ITransferenciaAppService
    {
        public TransferenciaAppService() : base("Transferencia")
        {
        }

        public HttpResponseMessage GetAll(int idUsuario)
        {
            return GetRequest("GetAll", new { idUsuario });
        }

        public HttpResponseMessage Get(int idTransferencia)
        {
            return GetRequest("Get", new { idTransferencia });
        }

        public HttpResponseMessage Post(TransferenciaDto transferencia)
        {
            return PostRequest(transferencia);
        }

        public HttpResponseMessage Put(TransferenciaDto transferencia)
        {
            return PutRequest("Put", transferencia);
        }

        public HttpResponseMessage Delete(int idTransferencia)
        {
            return DeleteRequest(new { idTransferencia });
        }

        public HttpResponseMessage PutIndicadorPagoRecebido(TransferenciaDto transferencia)
        {
            return PutRequest("PutIndicadorPagoRecebido", transferencia);
        }
    }
}
