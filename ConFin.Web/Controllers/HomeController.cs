using ConFin.Common.Web;
using System.Web.Mvc;

namespace ConFin.Web.Controllers
{
    public class HomeController: BaseHomeController
    {
        public ActionResult Home()
        {
            if (UsuarioLogado == null)
                return View("../Login/Login");

            ViewBag.Email = UsuarioLogado.Email;
            return View("Home", UsuarioLogado);
        }

        public ActionResult Logout()
        {
            UsuarioLogado = null;
            return RedirectToAction("Home");
        }
    }
}
