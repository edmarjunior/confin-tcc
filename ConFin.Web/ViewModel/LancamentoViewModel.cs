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
            Descricao = model.Descricao;
            Valor = model.Valor;
            Data = model.Data;
            IdConta = model.IdConta;
            IdCategoria = model.IdCategoria;
            IndicadorPago = model.IndicadorPago;

            IdUsuarioCadastro = model.IdUsuarioCadastro;
            NomeUsuarioCadastro = model.NomeUsuarioCadastro;
            DataCadastro = model.DataCadastro;
            IdUsuarioUltimaAlteracao = model.IdUsuarioUltimaAlteracao;
            NomeUsuarioUltimaAlteracao = model.NomeUsuarioUltimaAlteracao;
            DataUltimaAlteracao = model.DataUltimaAlteracao;

        }

        public int Id { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public int IdConta { get; set; }
        public int IdCategoria { get; set; }
        public string IndicadorPago { get; set; }
    }
}
