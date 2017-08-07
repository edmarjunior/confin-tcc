using System.Net.Http;

namespace ConFin.Application.AppService.Usuario
{
    public interface IUsuarioAppService
    {
        HttpResponseMessage Get(int id);
        HttpResponseMessage PutSenha(int id, string token, string novaSenha);
    }
}
