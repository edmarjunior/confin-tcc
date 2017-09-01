using ConFin.Common.Web;
using System.Web.Mvc;

namespace ConFin.Web.Controllers
{
    public class ChatController : BaseHomeController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}