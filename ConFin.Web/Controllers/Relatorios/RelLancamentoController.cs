using ConFin.Application.AppService.AcessoOpcaoMenu;
using ConFin.Application.AppService.Lancamento;
using ConFin.Common.Domain.Dto;
using ConFin.Common.Web;
using ConFin.Web.Reports.Lancamento;
using ConFin.Web.ViewModel.Lancamento;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace ConFin.Web.Controllers.Relatorios
{
    [OutputCache(Duration = 0)]
    public class RelLancamentoController: BaseController
    {
        private readonly ILancamentoAppService _lancamentoAppService;
        private readonly IAcessoOpcaoMenuAppService _acessoOpcaoMenuAppService;

        public RelLancamentoController(ILancamentoAppService lancamentoAppService, IAcessoOpcaoMenuAppService acessoOpcaoMenuAppService)
        {
            _lancamentoAppService = lancamentoAppService;
            _acessoOpcaoMenuAppService = acessoOpcaoMenuAppService;
        }

        public ActionResult RelLancamento()
        {
            var responseOpcMenu = _acessoOpcaoMenuAppService.Post(UsuarioLogado.Id, 8); // 8: codigo opção menu "Relatório Lançamento"
            if (!responseOpcMenu.IsSuccessStatusCode)
                return Error(responseOpcMenu);

            ViewBag.PrimeiroAcesso = Deserialize<int>(responseOpcMenu) < 1 ? "S" : "N";

            return View();
        }

        public ActionResult Gerar(byte mes, short ano)
        {
            var response = _lancamentoAppService.GetAll(UsuarioLogado.Id, mes, ano);
            if(!response.IsSuccessStatusCode)
                return Error(response);

            var lancamentos = Deserialize<IEnumerable<LancamentoDto>>(response).Select(x => new LancamentoViewModel(x)).ToList();

            if(!lancamentos.Any())
                return Error("Você não possui movimentações para este mês");

            var excel = LancamentoReport.GetExcel(lancamentos);

            return File(new MemoryStream(excel.GetAsByteArray()), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Lancamentos {DateTime.Now:yyyyMMddHHmmss}.xlsx");
        }
    }
}
