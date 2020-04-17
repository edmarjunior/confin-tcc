using ConFin.Common.Domain.Auxiliar;
using ConFin.Common.Domain.Dto;
using Newtonsoft.Json;
using System;
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

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (UsuarioLogado == null)
                filterContext.Result = Error("Sua sessão foi expirada");
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                var body = $"Ocorreu uma falha na web-confin: {filterContext.RequestContext.HttpContext.Request.Url}{Environment.NewLine}" +
                       $"Data......: {DateTime.Now}.{Environment.NewLine}" +
                       $"Exception.: {filterContext.Exception.Message}{Environment.NewLine}" +
                       $"StackTrace: {filterContext.Exception.StackTrace}";

                Email.Enviar("Exception Filter WEB", "confinpessoal@outlook.com", body, "ConFin automático");
                filterContext.ExceptionHandled = true;
                filterContext.Result = Error("Ocorreu um erro ao realizar a operação. Já estamos trabalhando para corrigir.");
                base.OnException(filterContext);
            }
            catch (Exception ex)
            {
                filterContext.ExceptionHandled = true;
                filterContext.Result = Error("Ocorreu um erro ao realizar a operação. Já estamos trabalhando para corrigir.");
                base.OnException(filterContext);
            }
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

        protected T Deserialize<T>(HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
        }
    }
}
