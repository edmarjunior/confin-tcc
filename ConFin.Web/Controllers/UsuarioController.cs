using ConFin.Application.AppService.Usuario;
using ConFin.Common.Domain.Dto;
using ConFin.Common.Web;
using ConFin.Web.ViewModel;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;

namespace ConFin.Web.Controllers
{
    public class UsuarioController: BaseController
    {
        private readonly IUsuarioAppService _usuarioAppService;

        public UsuarioController(IUsuarioAppService usuarioAppService)
        {
            _usuarioAppService = usuarioAppService;
        }

        public ActionResult ModalDadosUsuario()
        {

            try
            {
                var response = _usuarioAppService.Get(UsuarioLogado.Id);
                if (!response.IsSuccessStatusCode)
                    return Error(response);

                var usuarioDto = JsonConvert.DeserializeObject<UsuarioDto>(response.Content.ReadAsStringAsync().Result);
                return View("_ModalDadosUsuario", new UsuarioViewModel(usuarioDto));
            }
            catch (Exception ex)
            {
                return Error($"Erro ao abrir tela com os dados do usuário: {ex.Message}");
            }
        }
    }
}