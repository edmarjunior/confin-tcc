using ConFin.Common.Domain.Dto;
using ConFin.Common.Web.Extension;

namespace ConFin.Web.ViewModel.Home
{
    public class LancamentoResumoGeralViewModel
    {
        public LancamentoResumoGeralViewModel(LancamentoResumoGeralDto model)
        {
            TotReceitasPrevista = model.TotReceitasPrevista;
            TotReceitasRealizada = model.TotReceitasRealizada;
            TotDespesasPrevista = model.TotDespesasPrevista;
            TotDespesasRealizada = model.TotDespesasRealizada;
            TotSaldoPrevisto = model.TotSaldoPrevisto;
            TotSaldoAtual = model.TotSaldoAtual;
            TotValorSaldoInicialConta = model.TotValorSaldoInicialConta;
        }

        public decimal TotReceitasPrevista { get; set; }
        public decimal TotReceitasRealizada { get; set; }
        public decimal TotDespesasPrevista { get; set; }
        public decimal TotDespesasRealizada { get; set; }
        public decimal TotSaldoPrevisto { get; set; }
        public decimal TotSaldoAtual { get; set; }
        public decimal TotValorSaldoInicialConta { get; set; }

        public string TotalReceitasPrevista => TotReceitasPrevista.ToMoney("0,00");
        public string TotalReceitasRealizada => TotReceitasRealizada.ToMoney("0,00");
        public string TotalDespesasPrevista => TotDespesasPrevista.ToMoney("0,00");
        public string TotalDespesasRealizada => TotDespesasRealizada.ToMoney("0,00");
        public string TotalSaldoPrevisto => TotSaldoPrevisto.ToMoney("0,00");
        public string TotalSaldoAtual => TotSaldoAtual.ToMoney("0,00");
        public string TotalValorSaldoInicialConta => TotValorSaldoInicialConta.ToMoney("0,00");
    }
}
