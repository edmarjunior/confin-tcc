using ConFin.Common.Domain.Auxiliar;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace ConFin.Common.Api.Infra
{
    public class ExceptionFilter: ExceptionFilterAttribute
    {

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
           
            try
            {
                var body = $"Ocorreu uma falha na api-confin: {actionExecutedContext.ActionContext.Request.RequestUri}{Environment.NewLine}" +
                      $"Data......: {DateTime.Now}.{Environment.NewLine}" +
                      $"Exception.: {actionExecutedContext.Exception.Message}{Environment.NewLine}" +
                      $"StackTrace: {actionExecutedContext.Exception.StackTrace}";

                Email.Enviar("Exception Filter API", "confinpessoal@outlook.com", body, "ConFin automático");

                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Ocorreu um erro ao realizar a operação. Já estamos trabalhando para corrigir.")
                };
            }
            catch (Exception ex)
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Ocorreu um erro ao realizar a operação. Já estamos trabalhando para corrigir.")
                };
            }
            
        }
    }
}
