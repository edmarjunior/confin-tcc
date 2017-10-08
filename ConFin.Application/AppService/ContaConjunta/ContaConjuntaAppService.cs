using ConFin.Common.Application;
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
            return GetRequest(new { idUsuario, idConta });
        }

        public HttpResponseMessage Post(int idConta, int idUsuarioEnvio, string emailUsuarioConvidado)
        {
            return PostRequest("Post", null, new
            {
                idConta,
                idUsuarioEnvio,
                emailUsuarioConvidado
            });

        }

        public HttpResponseMessage Delete(int idContaConjunta)
        {
            return DeleteRequest(new { idContaConjunta });
        }

        public HttpResponseMessage Put(int idContaConjunta, string indicadorAprovado)
        {
            return PutRequest("Put", null, new
            {
                idContaConjunta,
                indicadorAprovado
            });
        }

    }
}
