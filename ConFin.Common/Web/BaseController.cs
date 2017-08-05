using System.Web.Mvc;

namespace ConFin.Common.Web
{
    public class BaseController: Controller
    {
        public Usuario UsuarioLogado
        {
            get { return SessionManagement.Get<Usuario>("UsuarioLogado"); }
            set { SessionManagement.Update("UsuarioLogado", value); }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(UsuarioLogado == null)
                filterContext.Result = new RedirectResult("localhost:5001/Home/Home");
        }
    }
}
