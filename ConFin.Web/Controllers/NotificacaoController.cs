using ConFin.Application.AppService.Notificacao;
using ConFin.Common.Domain.Dto;
using ConFin.Common.Web;
using ConFin.Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ConFin.Web.Controllers
{
    public class NotificacaoController: BaseController
    {
        private readonly INotificacaoAppService _notificacaoAppService;

        public NotificacaoController(INotificacaoAppService notificacaoAppService)
        {
            _notificacaoAppService = notificacaoAppService;
        }

        public ActionResult Get(string indicadorNotificacaoLida)
        {
            try
            {
                var response = _notificacaoAppService.Get(UsuarioLogado.Id, indicadorNotificacaoLida == "S");
                if (HttpStatusCode.NoContent == response.StatusCode)
                    return Json(new {noContent = true}, JsonRequestBehavior.AllowGet);

                return !response.IsSuccessStatusCode
                    ? Error(response) 
                    : Json(Deserialize<IEnumerable<NotificacaoDto>>(response).Select(x => new NotificacaoViewModel(x)), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult GetTotalNaoLidas()
        {
            try
            {
                var response = _notificacaoAppService.GetTotalNaoLidas(UsuarioLogado.Id);
                return !response.IsSuccessStatusCode
                    ? Error(response)
                    : Json(Deserialize<int>(response), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

    }
}