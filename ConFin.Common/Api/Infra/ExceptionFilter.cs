using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace ConFin.Common.Api.Infra
{
    public class ExceptionFilter: ExceptionFilterAttribute
    {

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("Ocorreu um erro ao realizar a operação. Já estamos trabalhando para corrigir.")
            };

            base.OnException(actionExecutedContext);
        }
    }
}
