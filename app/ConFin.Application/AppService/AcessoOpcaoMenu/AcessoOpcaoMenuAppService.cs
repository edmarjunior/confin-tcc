using ConFin.Common.Application;
using System.Net.Http;

namespace ConFin.Application.AppService.AcessoOpcaoMenu
{
    public class AcessoOpcaoMenuAppService : BaseAppService, IAcessoOpcaoMenuAppService
    {
        public AcessoOpcaoMenuAppService() : base("AcessoOpcaoMenu")
        {
        }

        public HttpResponseMessage Post(int idUsuario, int codigoOpcaoMenu)
        {
            return PostRequest("Post", null, new { idUsuario, codigoOpcaoMenu });
        }

    }
}
