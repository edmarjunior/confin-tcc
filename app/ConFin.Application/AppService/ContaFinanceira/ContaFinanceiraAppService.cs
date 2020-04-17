using ConFin.Common.Application;
using ConFin.Common.Domain.Dto;
using System.Net.Http;

namespace ConFin.Application.AppService.ContaFinanceira
{
    public class ContaFinanceiraAppService: BaseAppService, IContaFinanceiraAppService
    {
        public ContaFinanceiraAppService() : base("ContaFinanceira")
        {
        }

        public HttpResponseMessage GetAll(int idUsuario)
        {
            return GetRequest("GetAll", new { idUsuario });
        }

        public HttpResponseMessage Get(int idConta, int? idUsuario = null)
        {
            return GetRequest("Get", new { idConta, idUsuario });
        }

        public HttpResponseMessage Post(ContaFinanceiraDto conta)
        {
            return PostRequest("Post", conta);
        }

        public HttpResponseMessage Put(ContaFinanceiraDto conta)
        {
            return PutRequest("Put", conta);
        }

        public HttpResponseMessage Delete(int idUsuario, int idConta)
        {
            return DeleteRequest("Delete", new { idUsuario, idConta });
        }
    }
}
