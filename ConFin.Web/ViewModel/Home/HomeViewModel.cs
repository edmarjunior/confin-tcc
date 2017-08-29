using ConFin.Web.ViewModel.Lancamento;
using System.Collections.Generic;

namespace ConFin.Web.ViewModel.Home
{
    public class HomeViewModel
    {
        public LancamentoResumoGeralViewModel LancamentoResumoGeral { get; set; }
        public IEnumerable<LancamentoViewModel> Lancamentos { get; set; }
    }
}
