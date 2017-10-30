using ConFin.Common.Web;
using System;
using System.Web.Mvc;

namespace ConFin.Web.Controllers.Relatorios
{
    [OutputCache(Duration = 0)]
    public class RelLancamentoController: BaseController
    {
        public ActionResult RelLancamento()
        {
            return View();
        }

        public ActionResult Gerar(DateTime dataInicial, DateTime dataFinal)
        {
            return File("C:/teste.xls", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}
