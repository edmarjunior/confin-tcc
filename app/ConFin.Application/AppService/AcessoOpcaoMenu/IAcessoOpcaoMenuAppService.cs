using System.Net.Http;

namespace ConFin.Application.AppService.AcessoOpcaoMenu
{
    public interface IAcessoOpcaoMenuAppService
    {
        HttpResponseMessage Post(int idUsuario, int codigoOpcaoMenu);
    }
}
