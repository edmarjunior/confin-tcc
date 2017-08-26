using ConFin.Common.Domain;
using ConFin.Common.Domain.Dto;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace ConFin.Common.Web
{
    public class BaseController: Controller
    {
        public UsuarioDto UsuarioLogado
        {
            get { return SessionManagement.Get<UsuarioDto>("UsuarioLogado"); }
            set { SessionManagement.Update("UsuarioLogado", value); }
        }


        protected ActionResult Ok(string mensagem = null)
        {
            Response.StatusCode = (int) HttpStatusCode.OK;
            return Content(mensagem ?? "Operação realizada com sucesso !!!");
        }

        protected ActionResult Error(HttpResponseMessage response)
        {
            Response.StatusCode = (int) response.StatusCode;
            Response.TrySkipIisCustomErrors = true;
            return Content(response.Content.ReadAsStringAsync().Result.Replace('[', ' ').Replace(']', ' ').Replace('"', ' '));
        }

        protected ActionResult Error(string mensagem)
        {
            Response.StatusCode = (int) HttpStatusCode.BadRequest;
            Response.TrySkipIisCustomErrors = true;
            return Content(mensagem);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(UsuarioLogado == null)
                filterContext.Result = new RedirectResult(new Parameters().UriWeb + "Home");
        }
    }
}
