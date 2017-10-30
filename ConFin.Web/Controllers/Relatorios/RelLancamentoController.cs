using ConFin.Common.Web;
using OfficeOpenXml;
using System;
using System.IO;
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
            using (var excelPackage = new ExcelPackage())
            {
                var wb = excelPackage.Workbook;
                var ws = wb.Worksheets.Add("Lancamentos");

                ws.Cells[1, 1].Value = "Data";
                ws.Cells[1, 2].Value = "Descrição";
                ws.Cells[1, 3].Value = "Categoria";
                ws.Cells[1, 4].Value = "Valor";

                return File(new MemoryStream(excelPackage.GetAsByteArray()), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Lancamentos {DateTime.Now:yyMMddHHmmss}.xlsx");
            }
        }
    }
}
