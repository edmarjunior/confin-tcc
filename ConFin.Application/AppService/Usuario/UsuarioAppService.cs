using ConFin.Common.Application;
using System.Net.Http;

namespace ConFin.Application.AppService.Usuario
{
    public class UsuarioAppService : BaseAppService, IUsuarioAppService
    {
        public UsuarioAppService() : base("Usuario")
        {
        }

        public HttpResponseMessage Get(int id)
        {
            return GetRequest(new { id });
        }

        public HttpResponseMessage PutSenha(int id, string token, string novaSenha)
        {
            return PutRequest("PutSenha", null, new { id, token, novaSenha });
        }
    }
}
