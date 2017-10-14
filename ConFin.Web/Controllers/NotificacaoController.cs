using ConFin.Application.AppService.Notificacao;
using ConFin.Common.Domain.Dto;
using ConFin.Common.Web;
using System;
using System.Collections.Generic;
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

        public ActionResult Get()
        {
            try
            {
                var response = _notificacaoAppService.Get(UsuarioLogado.Id);
                return !response.IsSuccessStatusCode 
                    ? Error(response) 
                    : Json(Deserialize<IEnumerable<NotificacaoDto>>(response), JsonRequestBehavior.AllowGet);
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