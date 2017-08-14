using ConFin.Common.Domain.Auxiliar;
using ConFin.Common.Domain.Dto;
using System;

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

        public int Id { get; set; }
        public string IndicadorReceitaDespesa { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public int IdConta { get; set; }
        public string NomeConta { get; set; }
        public int IdCategoria { get; set; }
        public string NomeCategoria { get; set; }
        public string CorCategoria { get; set; }
        public string IndicadorPago { get; set; }
    }
}
