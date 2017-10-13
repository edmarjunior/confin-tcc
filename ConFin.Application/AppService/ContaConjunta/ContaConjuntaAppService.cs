using ConFin.Common.Application;
using ConFin.Common.Domain.Dto;
using System.Net.Http;

namespace ConFin.Application.AppService.ContaConjunta
{
    public class ContaConjuntaAppService : BaseAppService, IContaConjuntaAppService
    {
        public ContaConjuntaAppService() : base("ContaConjunta")
        {
        }

        public HttpResponseMessage Get(int? idUsuario, int? idConta = null)
        {
            return GetRequest("Get", new { idUsuario, idConta });
        }

        public HttpResponseMessage Post(ContaConjuntaDto contaConjunta)
        {
            return PostRequest("Post", contaConjunta);

        }

        public HttpResponseMessage Delete(int idContaConjunta)
        {
            return DeleteRequest("Delete", new { idContaConjunta });
        }

        public HttpResponseMessage Put(ContaConjuntaDto contaConjunta)
        {
            return PutRequest("Put", contaConjunta);
        }

    }
}
