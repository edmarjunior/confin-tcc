using ConFin.Common.Domain.Auxiliar;
using ConFin.Common.Domain.Dto;
using ConFin.Common.Web.Extension;
using System;
using System.Collections.Generic;

namespace ConFin.Web.ViewModel
{
    public class LancamentoViewModel: DataManut
    {
        public LancamentoViewModel()
        {
        }

        public LancamentoViewModel(LancamentoDto model)
        {
            Id = model.Id;
            IndicadorReceitaDespesa = model.IndicadorReceitaDespesa;
            Descricao = model.Descricao;
            Valor = model.Valor;
            Data = model.Data;
            IdConta = model.IdConta;
            NomeConta = model.NomeConta;
            IdCategoria = model.IdCategoria;
            NomeCategoria = model.NomeCategoria;
            CorCategoria = model.CorCategoria;
            IndicadorPago = model.IndicadorPago;

            IdUsuarioCadastro = model.IdUsuarioCadastro;
            NomeUsuarioCadastro = model.NomeUsuarioCadastro;
            DataCadastro = model.DataCadastro;
            IdUsuarioUltimaAlteracao = model.IdUsuarioUltimaAlteracao;
            NomeUsuarioUltimaAlteracao = model.NomeUsuarioUltimaAlteracao;
            DataUltimaAlteracao = model.DataUltimaAlteracao;

        }

        public int? Id { get; set; }
        public string IndicadorReceitaDespesa { get; set; }
        public string Descricao { get; set; }
        public decimal? Valor { get; set; }
        public DateTime? Data { get; set; }
        public int? IdConta { get; set; }
        public string NomeConta { get; set; }
        public int? IdCategoria { get; set; }
        public string NomeCategoria { get; set; }
        public string CorCategoria { get; set; }
        public string IndicadorPago { get; set; }

        public IEnumerable<ContaFinanceiraDto> ContasFinanceira{ get; set; }
        public IEnumerable<LancamentoCategoriaDto> Categorias { get; set; }

        public string IndicadorPagamento => IndicadorPago ?? "N";

        public string DataLancamento => Data?.ToShortDateString();
        public string ValorLancamento => Valor.ToMoney();

        public string ShouldCheckReceitaDespesa(string indicadorReceitaDespesa)
        {
            return IndicadorReceitaDespesa.Equals(indicadorReceitaDespesa) ? "checked" : string.Empty;
        }

        public bool IsReceita => IndicadorReceitaDespesa == "R";
        public bool IsPago => IndicadorPagamento != "N";
        public bool IsVencido => !IsPago && DateTime.Today > Data;

    }
}
