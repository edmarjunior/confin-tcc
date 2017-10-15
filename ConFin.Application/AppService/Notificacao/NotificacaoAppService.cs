using ConFin.Common.Application;
using System.Net.Http;

namespace ConFin.Application.AppService.Notificacao
{
    public class NotificacaoAppService: BaseAppService, INotificacaoAppService
    {
        public NotificacaoAppService() : base("Notificacao")
        {
        }

        public HttpResponseMessage Get(int idUsuario, bool notificacaoLida)
        {
            return GetRequest("Get", new {idUsuario, notificacaoLida });
        }

        public HttpResponseMessage GetTotalNaoLidas(int idUsuario)
        {
            return GetRequest("GetTotalNaoLidas", new { idUsuario });

        }
    }
}
