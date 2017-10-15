using ConFin.Common.Domain;
using ConFin.Domain.Notificacao;
using System.Net;
using System.Web.Http;

namespace ConFin.Api.Controllers
{
    public class NotificacaoController: ApiController
    {
        private readonly INotificacaoRepository _notificacaoRepository;
        private readonly INotificacaoService _notificacaoService;
        private readonly Notification _notification;

        public NotificacaoController(INotificacaoRepository notificacaoRepository, INotificacaoService notificacaoService, 
            Notification notification)
        {
            _notificacaoRepository = notificacaoRepository;
            _notificacaoService = notificacaoService;
            _notification = notification;
        }

        public IHttpActionResult Get(int idUsuario, bool notificacaoLida)
        {
            bool nenhumaNotificacao;
            var notificacoes = _notificacaoService.Get(idUsuario, notificacaoLida, out nenhumaNotificacao);

            if(nenhumaNotificacao)
                return Content(HttpStatusCode.NoContent, notificacoes);

            return Ok(notificacoes);
        } 
        public IHttpActionResult GetTotalNaoLidas(int idUsuario) => Ok(_notificacaoRepository.GetTotalNaoLidas(idUsuario));
    }
}
