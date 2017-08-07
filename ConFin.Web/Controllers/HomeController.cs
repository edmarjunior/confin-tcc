using ConFin.Common.Web;
using System;
using System.Web.Mvc;

namespace ConFin.Web.Controllers
{
    public class HomeController: BaseHomeController
    {
        public ActionResult Home()
        {
            try
            {
                if (UsuarioLogado == null)
                    return View("../Login/Login");

                ViewBag.Email = UsuarioLogado.Email;
                return View("Home", UsuarioLogado);
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
