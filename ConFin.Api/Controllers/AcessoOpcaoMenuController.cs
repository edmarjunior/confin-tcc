using ConFin.Domain.AcessoOpcaoMenu;
using System.Web.Http;

namespace ConFin.Api.Controllers
{
    public class AcessoOpcaoMenuController: ApiController
    {
        private readonly IAcessoOpcaoMenuService _acessoOpcaoMenuService;

        public AcessoOpcaoMenuController(IAcessoOpcaoMenuService acessoOpcaoMenuService)
        {
            _acessoOpcaoMenuService = acessoOpcaoMenuService;
        }

        public IHttpActionResult Post(int idUsuario, int codigoOpcaoMenu)
        {
            return Ok(_acessoOpcaoMenuService.Post(idUsuario, codigoOpcaoMenu));
        }
    }
}
