using ConFin.Application.AppService.Lancamento;
using ConFin.Application.AppService.Transferencia;
using ConFin.Common.Domain.Dto;
using ConFin.Common.Web;
using ConFin.Web.ViewModel.Home;
using ConFin.Web.ViewModel.Lancamento;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ConFin.Web.Controllers
{
    [OutputCache(Duration = 0)]
    public class HomeController: BaseHomeController
    {
        private readonly ILancamentoAppService _lancamentoAppService;
        private readonly ITransferenciaAppService _transferenciaAppService;


        public HomeController(ILancamentoAppService lancamentoAppService, ITransferenciaAppService transferenciaAppService)
        {
            _lancamentoAppService = lancamentoAppService;
            _transferenciaAppService = transferenciaAppService;
        }

        public ActionResult Home()
        {
            if (UsuarioLogado == null)
                return View("../Login/Login");

            var response = _lancamentoAppService.GetAll(UsuarioLogado.Id);
            if (!response.IsSuccessStatusCode)
                return Error(response);

            var lancamentos = JsonConvert.DeserializeObject<IEnumerable<LancamentoDto>>(response.Content.ReadAsStringAsync().Result)
                .Select(x => new LancamentoViewModel(x)).ToList();

            var responsePossuiOpcaoTransferencia = _transferenciaAppService.GetVerificaClientePossuiTransferenciaHabilitada(UsuarioLogado.Id);
            ViewBag.PossuiOpcaoTransferencia = responsePossuiOpcaoTransferencia.IsSuccessStatusCode;

            ViewBag.Email = UsuarioLogado.Email;
            ViewBag.Id = UsuarioLogado.Id;
            return View("Home", new HomeViewModel { Lancamentos = lancamentos });
        }

        public ActionResult Logout()
        {
            UsuarioLogado = null;
            return RedirectToAction("Home");
        }
    }
}
