using ConFin.Common.Application;
using System.Net.Http;

namespace ConFin.Application.AppService.Login
{
    public class LoginAppService : BaseAppService, ILoginAppService
    {
        public LoginAppService() : base("http://localhost:5002/api/Login")
        {}

        public HttpResponseMessage Get(string email, string senha)
        {
            return GetRequest(new {email, senha});
        }

        public HttpResponseMessage Post(Common.Domain.Usuario usuario)
        {
            return PostRequest(usuario);
        }

        public HttpResponseMessage PutConfirmacaoCadastro(int idUsuario)
        {
            return PutRequest("PutConfirmacaoCadastro", null, new {idUsuario});
        }

        public HttpResponseMessage PostReenviarSenha(string email)
        {
            return PostRequest("PostReenviarSenha", null, new {email});
        }

        public HttpResponseMessage GetVerificaTokenValidoRedefinirSenha(int idUsuario, string token)
        {
            return GetRequest("GetVerificaTokenValidoRedefinirSenha", new {idUsuario, token});
        }
    }
}
