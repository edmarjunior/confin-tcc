using ConFin.Common.Application;
using ConFin.Common.Domain.Dto;
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
            return GetRequest("Get", new { id });
        }

        public HttpResponseMessage PutSenha(int id, string token, string novaSenha)
        {
            return PutRequest("PutSenha", null, new { id, token, novaSenha });
        }

        public HttpResponseMessage Put(UsuarioDto usuario)
        {
            return PutRequest("Put", usuario);
        }
    }
}
