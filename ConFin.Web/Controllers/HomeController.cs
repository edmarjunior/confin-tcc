using ConFin.Application.AppService.Lancamento;
using ConFin.Common.Domain.Dto;
using ConFin.Common.Web;
using ConFin.Web.ViewModel.Home;
using ConFin.Web.ViewModel.Lancamento;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ConFin.Web.Controllers
{
    public class HomeController: BaseHomeController
    {
        private readonly ILancamentoAppService _lancamentoAppService;

        public HomeController(ILancamentoAppService lancamentoAppService)
        {
            _lancamentoAppService = lancamentoAppService;
        }

        public ActionResult Home()
        {
            try
            {
                if (UsuarioLogado == null)
                    return View("../Login/Login");

                var response = _lancamentoAppService.GetAll(UsuarioLogado.Id);
                if (!response.IsSuccessStatusCode)
                    return Error(response);

                var lancamentos = JsonConvert.DeserializeObject<IEnumerable<LancamentoDto>>(response.Content.ReadAsStringAsync().Result)
                    .Select(x => new LancamentoViewModel(x)).ToList();

                ViewBag.Email = UsuarioLogado.Email;
                return View("Home", new HomeViewModel { Lancamentos = lancamentos });
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult Logout()
        {
            try
            {
                UsuarioLogado = null;
                return RedirectToAction("Home");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
    }
}
