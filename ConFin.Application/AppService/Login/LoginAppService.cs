using ConFin.Common.Application;
using ConFin.Common.Domain.Dto;
using System.Net.Http;

namespace ConFin.Application.AppService.Login
{
    public class LoginAppService : BaseAppService, ILoginAppService
    {
        public LoginAppService() : base("Login")
        {
        }

        public HttpResponseMessage Get(string email, string senha)
        {
            return GetRequest(new { email, senha });
        }

        public HttpResponseMessage Post(UsuarioDto usuario)
        {
            return PostRequest(usuario);
        }

        public HttpResponseMessage PutConfirmacaoCadastro(int idUsuario)
        {
            return PutRequest("PutConfirmacaoCadastro", null, new { idUsuario });
        }

        public HttpResponseMessage PostReenviarSenha(string email)
        {
            return PostRequest("PostReenviarSenha", null, new { email });
        }

        public HttpResponseMessage GetVerificaTokenValidoRedefinirSenha(int idUsuario, string token)
        {
            return GetRequest("GetVerificaTokenValidoRedefinirSenha", new { idUsuario, token });
        }
    }
}
