using ConFin.Web.ViewModel.Lancamento;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace ConFin.Web.Reports.Lancamento
{
    public static class LancamentoReport
    {
        private const double HeightDefault = 26;

        public static ExcelPackage GetExcel(List<LancamentoViewModel> lancamentos)
        {
            var excel = new ExcelPackage();
            using (var modelo = File.OpenRead($@"{AppDomain.CurrentDomain.BaseDirectory}Reports\Lancamento\Lancamento_Mensal.xlsx"))
                excel.Load(modelo);

            var wss = excel.Workbook.Worksheets;

            // colocando o mes e ano nas planilhas
            SetAnoMes(wss, lancamentos);

            // montando as planilhas de Despesas e Receitas (percorrendo os lançamentos)
            MontaLancamentos("D", wss["Despesas"], lancamentos.Where(x => !x.IsReceita));
            MontaLancamentos("R", wss["Receitas"], lancamentos.Where(x => !x.IsDespesa));

            // montando total por despesas agrupadas em aba oculta
            var ws = wss["Despesa_Categoria"];
            var linha = 3;
            foreach (var categoria in lancamentos.Where(x => !x.IsReceita).GroupBy(x => x.IdCategoria))
            {
                ws.InsertRow(linha, 1, linha - 1);
                ws.Cells[linha, 1].Value = $"{categoria.First().NomeCategoria}";
                ws.Cells[linha, 2].Value = categoria.Sum(x => x.Valor);
                linha++;
            }
            ws.DeleteRow(2);
            ws.DeleteRow(linha - 1);

            // atualizando grafico de despesas por categoria
            AtualizaGraficoCategorias(wss, linha - 2, lancamentos.All(x => !x.IsDespesa));

            return excel;
        }

        private static void SetAnoMes(ExcelWorksheets wss, List<LancamentoViewModel> lancamentos)
        {
            var mes = $"{(DateTime)lancamentos.First().Data:MMM}".ToUpper();
            var ano = $"{(DateTime)lancamentos.First().Data:yyyy}";

            (wss["Resumo"].Drawings["Ano do Orçamento"] as ExcelShape).Text = $"{mes} {ano}";
            (wss["Despesas"].Drawings["Ano do Orçamento"] as ExcelShape).Text = $"{mes} {ano}";
            (wss["Receitas"].Drawings["Ano do Orçamento"] as ExcelShape).Text = $"{mes} {ano}";
        }

        private static void MontaLancamentos(string indicadorDespesaReceita, ExcelWorksheet ws, IEnumerable<LancamentoViewModel> lancamentos)
        {
            var lancamentoViewModels = lancamentos as IList<LancamentoViewModel> ?? lancamentos.ToList();
            if (!lancamentoViewModels.Any())
                return;

            var linha = 11;

            foreach (var lancamento in lancamentoViewModels)
            {
                var nomeConta = !lancamento.IsTransferencia 
                    ? lancamento.NomeContaOrigem 
                    : indicadorDespesaReceita == "D"
                        ? lancamento.NomeContaOrigem 
                        : lancamento.NomeContaDestino;

                ws.InsertRow(linha, 1, linha - 1);
                ws.Row(linha).Height = HeightDefault;
                ws.Cells[linha, 2].Value = $"{lancamento.DataLancamento}";
                ws.Cells[linha, 3].Value = lancamento.Descricao;
                ws.Cells[linha, 4].Value = lancamento.Valor;
                ws.Cells[linha, 5].Value = nomeConta;
                ws.Cells[linha, 6].Value = lancamento.NomeCategoria;
                linha++;
            }
            ws.DeleteRow(10);
            ws.DeleteRow(linha - 1);
            ws.Row(linha - 1).Height = 4.5;
        }

        private static void AtualizaGraficoCategorias(ExcelWorksheets wss, int linhaFinalDespesa, bool removeGrafico)
        {
            var grafico = wss["Resumo"].Drawings.FirstOrDefault(x => x.Name == "Gráfico 2") as ExcelPieChart;
            if (grafico == null)
                return;

            if (removeGrafico)
            {
                wss["Resumo"].Drawings.Remove("Gráfico 2");
                return;
            }

            grafico.Series.Delete(0);
            grafico.Series.Add(wss["Despesa_Categoria"].Cells[2, 2, linhaFinalDespesa, 2], wss["Despesa_Categoria"].Cells[2, 1, linhaFinalDespesa, 1]);
            grafico.DataLabel.Font.Color = Color.White;
        }
    }
}
