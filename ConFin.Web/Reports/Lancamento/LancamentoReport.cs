using ConFin.Web.ViewModel.Lancamento;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConFin.Web.Reports.Lancamento
{
    public static class LancamentoReport
    {
        public static ExcelPackage GetExcel(List<LancamentoViewModel> lancamentos)
        {
            var ep = new ExcelPackage();
            using (var modelo = File.OpenRead($@"{AppDomain.CurrentDomain.BaseDirectory}Reports\Lancamento\Lancamento_Mensal.xlsx"))
                ep.Load(modelo);

            var ws = ep.Workbook.Worksheets["Orçamento mensal simples"];
            const double heightDefault = 26;
            ws.Row(4).Height = heightDefault;
            ws.Row(6).Height = heightDefault;
            ws.Row(7).Height = heightDefault;
            ws.Row(9).Height = heightDefault;
            ws.Row(10).Height = heightDefault;


            var linha = 11;
            foreach (var receita in lancamentos.Where(x => x.IndicadorReceitaDespesa == "R"))
            {
                ws.InsertRow(linha, 1, linha + 1);
                ws.Row(linha).Height = heightDefault;
                ws.Cells[linha, 2].Value = receita.Descricao;
                ws.Cells[linha, 3].Value = receita.Valor;
                linha++;
            }
            ws.DeleteRow(linha);

            ws.Row(linha).Height = heightDefault;
            ws.Row(linha + 1).Height = heightDefault;
            ws.Row(linha + 2).Height = heightDefault;

            linha += 3;
            foreach (var despesa in lancamentos.Where(x => x.IndicadorReceitaDespesa == "D"))
            {
                ws.InsertRow(linha, 1, linha + 1);
                ws.Row(linha).Height = heightDefault;
                ws.Cells[linha, 2].Value = despesa.Descricao;
                ws.Cells[linha, 3].Value = despesa.Valor;
                linha++;
            }
            ws.DeleteRow(linha);

            return ep;
        }
    }
}