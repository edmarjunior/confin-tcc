using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace ConFin.Common.Api.Infra
{
    public class ExceptionFilter: ExceptionFilterAttribute
    {

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            ////Montando nome da API
            //var indexOfEnd = actionExecutedContext.Request.RequestUri.AbsolutePath.IndexOf("/api/", StringComparison.Ordinal);
            //var api = actionExecutedContext.Request.RequestUri.AbsolutePath.Substring(1, indexOfEnd - 1).Replace("/", ".");
            //var collection = api.Replace(".", "_").ToUpper();

            ////Informações da Exception
            //var exception = new
            //{
            //    Origem = actionExecutedContext.Request.RequestUri.AbsoluteUri,
            //    actionExecutedContext.Exception.Message,
            //    actionExecutedContext.Exception.StackTrace,
            //    InnerException = actionExecutedContext.Exception.InnerException?.InnerException ?? (object)"null",
            //    Data = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
            //};

            ////Salva Exception no Mongo
            //_mongo.GetCollection<object>(collection).InsertOneAsync(exception);

            ////Envia aviso por E-Mail
            //new Thread(() =>
            //{
            //    var pathLog = @"C:\Log\Api";
            //    try
            //    {
            //        Email.Send(new GKS_EnvioEmail
            //        {
            //            Nom_Assunto = $"Falha na API {api} - {_ambiente}",
            //            Nom_EmailTO = _emailExceptionFilter,
            //            Nom_Corpo = $"Houve uma falha na API {api}. Consulte o log no Mongo: GrupoKasil_Log > {collection}\n\n" +
            //                        $"Message: {exception.Message}\n" +
            //                        $"StackTrace: {exception.StackTrace}\n" +
            //                        $"InnerException: {exception.InnerException}\n" +
            //                        $"Data: {exception.Data}"
            //        });
            //        Log.Write(pathLog, collection, $"{_emailExceptionFilter} - Falha ao executar {exception.Origem}. Erro: {exception.Message}");
            //    }
            //    catch (Exception ex)
            //    {
            //        Log.Write(pathLog, collection, $"Falha ao enviar E-Mail no ExceptionFilter na API {api}. Erro: {ex.Message}");
            //    }
            //}).Start();

            actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("Ocorreu um erro ao realizar a operação. Já estamos trabalhando para corrigir.")
            };

            base.OnException(actionExecutedContext);
        }
    }
}
