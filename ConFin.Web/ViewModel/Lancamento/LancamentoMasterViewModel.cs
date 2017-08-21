using ConFin.Common.Domain.Dto;
using System.Collections.Generic;

namespace ConFin.Web.ViewModel.Lancamento
{
    public class LancamentoMasterViewModel
    {
        public LancamentoMasterViewModel(List<LancamentoViewModel> lancamentos, int? idContaFiltro = null, int? idCategoriaFiltro = null)
        {
            IdContaFiltro = idContaFiltro;
            IdCategoriaFiltro = idCategoriaFiltro;
            Lancamentos = lancamentos;
        }

        public List<LancamentoViewModel> Lancamentos { get; set; }
        public List<ContaFinanceiraDto> Contas { get; set; }
        public List<LancamentoCategoriaDto> Categorias { get; set; }
        public int? IdContaFiltro { get; set; }
        public int? IdCategoriaFiltro { get; set; }
    }
}