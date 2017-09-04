using ConFin.Common.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ConFin.Web.ViewModel.Lancamento
{
    public class LancamentoMasterViewModel
    {
        public LancamentoMasterViewModel(List<LancamentoViewModel> lancamentos, byte mes, short ano, int? idContaFiltro = null, int? idCategoriaFiltro = null)
        {
            IdContaFiltro = idContaFiltro;
            IdCategoriaFiltro = idCategoriaFiltro;
            Lancamentos = lancamentos;
            DataPesquisa = new DateTime(ano, mes, 1);
        }

        public List<LancamentoViewModel> Lancamentos { get; set; }
        public List<ContaFinanceiraDto> Contas { get; set; }
        public List<LancamentoCategoriaDto> Categorias { get; set; }
        public int? IdContaFiltro { get; set; }
        public int? IdCategoriaFiltro { get; set; }
        public DateTime DataPesquisa { get; set; }

        public string NomeMesDataPesquisa => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(DataPesquisa.ToString("MMMM", CultureInfo.CurrentCulture).ToLower());
        public string MesDataPesquisa => DataPesquisa.Month.ToString();
        public string AnoDataPesquisa => DataPesquisa.Year.ToString();
    }
}