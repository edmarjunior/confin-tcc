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

        public HttpResponseMessage Get(int idConta)
        {
            return GetRequest(new { idConta });
        }

        public HttpResponseMessage Post(ContaFinanceiraDto conta)
        {
            return PostRequest(conta);

        }

        public HttpResponseMessage Put(ContaFinanceiraDto conta)
        {
            return PutRequest(conta);

        }

        public HttpResponseMessage Delete(int idUsuario, int idConta)
        {
            return DeleteRequest(new { idUsuario, idConta });
        }
    }
}
