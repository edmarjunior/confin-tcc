using ConFin.Application.AppService.Lancamento;
using ConFin.Common.Web;
using ConFin.Web.ViewModel.Home;
using System;
using System.Web.Mvc;

namespace ConFin.Web.Controllers
{
    public class HomeController: BaseHomeController
    {
        private readonly ILancamentoAppService _lancamentoAppService;

        public HomeController(ILancamentoAppService lancamentoAppService)
        {
            _lancamentoAppService = lancamentoAppService;
        }

        public ActionResult Home()
        {
            try
            {
                if (UsuarioLogado == null)
                    return View("../Login/Login");

                ViewBag.Email = UsuarioLogado.Email;
                return View("Home", new HomeViewModel());
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult Logout()
        {
            try
            {
                UsuarioLogado = null;
                return RedirectToAction("Home");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
    }
}
