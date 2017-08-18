using ConFin.Common.Web.Extension;
using System.Collections.Generic;
using System.Linq;

namespace ConFin.Web.ViewModel.Lancamento
{
    public class ResumoLancamentosViewModel
    {
        public ResumoLancamentosViewModel(List<LancamentoViewModel> lancamentos)
        {
            TotalDespesasRealizada = lancamentos.Where(x => x.IsDespesaRealizada).Sum(x => x.Valor);
            TotalDespesasPrevista = lancamentos.Where(x => x.IsDespesa).Sum(x => x.Valor);
            TotalReceitasRealizada = lancamentos.Where(x => x.IsReceitaRealizada).Sum(x => x.Valor);
            TotalReceitasPrevista = lancamentos.Where(x => x.IsReceita).Sum(x => x.Valor);
        }


        public decimal? TotalDespesasRealizada { get; set; }
        public decimal? TotalDespesasPrevista { get; set; }

        public decimal? TotalReceitasRealizada { get; set; }
        public decimal? TotalReceitasPrevista { get; set; }

        public decimal? SaldoAtual => (TotalReceitasRealizada ?? 0) - (TotalDespesasRealizada ?? 0);
        public decimal? SaldoPrevisto => (TotalReceitasPrevista ?? 0) - (TotalDespesasPrevista ?? 0);



        public string TotDespesasRealizada => TotalDespesasRealizada.ToMoney("0,00");
        public string TotDespesasPrevista => TotalDespesasPrevista.ToMoney("0,00");
        public string TotReceitasRealizada => TotalReceitasRealizada.ToMoney("0,00");
        public string TotReceitasPrevista => TotalReceitasPrevista.ToMoney("0,00");
        public string TotSaldoAtual => SaldoAtual.ToMoney("0,00");
        public string TotSaldoPrevisto => SaldoPrevisto.ToMoney("0,00");

    }
}
