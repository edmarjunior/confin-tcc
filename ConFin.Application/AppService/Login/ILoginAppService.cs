using System.Net.Http;

namespace ConFin.Application.AppService.Login
{
    public interface ILoginAppService
    {
        HttpResponseMessage Get(string email, string senha);
        HttpResponseMessage Post(Common.Domain.Usuario usuario);
        HttpResponseMessage PutConfirmacaoCadastro(int idUsuario);
        HttpResponseMessage PostReenviarSenha(string email);
        HttpResponseMessage GetVerificaTokenValidoRedefinirSenha(int idUsuario, string token);
    }
}
