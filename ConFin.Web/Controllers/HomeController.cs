using ConFin.Web.Model;
using System.Web.Mvc;

namespace ConFin.Web.Controllers
{
    public class HomeController: Controller
    {
        public ActionResult Home(Usuario usuario)
        {
            ViewBag.Email = usuario.Email;
            return View("Home", usuario);
        }
    }
}
