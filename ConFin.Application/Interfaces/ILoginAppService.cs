using ConFin.Common.Domain;
using System.Net.Http;

namespace ConFin.Application.Interfaces
{
    public interface ILoginAppService
    {
        HttpResponseMessage Get(string email, string senha);
        HttpResponseMessage Post(Usuario usuario);
        HttpResponseMessage PutConfirmacaoCadastro(int idUsuario);
    }
}
