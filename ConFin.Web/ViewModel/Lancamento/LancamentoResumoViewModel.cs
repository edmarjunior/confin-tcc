using System.Collections.Generic;
using System.Linq;

namespace ConFin.Web.ViewModel.Lancamento
{
    public class LancamentoResumoViewModel
    {
        public LancamentoResumoViewModel(List<LancamentoViewModel> lancamentos)
        {
            Lancamentos = lancamentos;
            Resumo = new ResumoLancamentosViewModel(lancamentos.ToList());
        }

        public List<LancamentoViewModel> Lancamentos { get; set; }
        public ResumoLancamentosViewModel Resumo { get; set; }
    }
}