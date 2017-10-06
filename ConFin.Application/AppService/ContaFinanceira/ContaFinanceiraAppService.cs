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
            return GetRequest("Get", new { idConta });
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

        public HttpResponseMessage PostConviteContaConjunta(int idConta, int idUsuarioEnvio, string emailUsuarioConvidado)
        {
            return PostRequest("PostConviteContaConjunta", null, new
            {
                idConta,
                idUsuarioEnvio,
                emailUsuarioConvidado
            });

        }

        public HttpResponseMessage GetConviteContaConjunta(int idUsuario)
        {
            return GetRequest("GetConviteContaConjunta", new { idUsuario });
        }

        public HttpResponseMessage PutConviteContaConjunta(int idSolicitacao, int idUsuario, string indicadorAprovado)
        {
            return PutRequest("PutConviteContaConjunta", null, new
            {
                idSolicitacao, idUsuario, indicadorAprovado
            });
        }

        public HttpResponseMessage GetUsuariosContaConjunta(int idConta)
        {
            return GetRequest("GetUsuariosContaConjunta", new { idConta });
        }
    }
}
