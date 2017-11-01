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

        public RelLancamentoController(ILancamentoAppService lancamentoAppService)
        {
            _lancamentoAppService = lancamentoAppService;
        }

        public ActionResult RelLancamento()
        {
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
