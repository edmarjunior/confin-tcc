using ConFin.Common.Domain.Auxiliar;
using ConFin.Common.Domain.Dto;
using ConFin.Common.Web.Extension;
using System;
using System.Collections.Generic;

namespace ConFin.Web.ViewModel
{
    public class TransferenciaViewModel : DataManut
    {
        public TransferenciaViewModel()
        {
        }

        public TransferenciaViewModel(TransferenciaDto model)
        {
            Id = model.Id;
            IdContaOrigem = model.IdContaOrigem;
            NomeContaOrigem = model.NomeContaOrigem;
            IdContaDestino = model.IdContaDestino;
            NomeContaDestino = model.NomeContaDestino;
            Valor = model.Valor;
            Descricao = model.Descricao;
            Data = model.Data;
            IdCategoria = model.IdCategoria;
            NomeCategoria = model.NomeCategoria;
            CorCategoria = model.CorCategoria;
            IndicadorPagoRecebido = model.IndicadorPagoRecebido;
            // manut
            IdUsuarioCadastro = model.IdUsuarioCadastro;
            NomeUsuarioCadastro = model.NomeUsuarioCadastro;
            DataCadastro = model.DataCadastro;
            IdUsuarioUltimaAlteracao = model.IdUsuarioUltimaAlteracao;
            NomeUsuarioUltimaAlteracao = model.NomeUsuarioUltimaAlteracao;
            DataUltimaAlteracao = model.DataUltimaAlteracao;
        }

        public int Id { get; set; }
        public int IdContaOrigem { get; set; }
        public string NomeContaOrigem { get; set; }
        public int IdContaDestino { get; set; }
        public string NomeContaDestino { get; set; }
        public decimal Valor { get; set; }
        public string Descricao { get; set; }
        public DateTime? Data { get; set; }
        public int IdCategoria { get; set; }
        public string NomeCategoria { get; set; }
        public string CorCategoria { get; set; }
        public string IndicadorPagoRecebido { get; set; }

        public IEnumerable<ContaFinanceiraDto> ContasFinanceira { get; set; }
        public IEnumerable<LancamentoCategoriaDto> Categorias { get; set; }

        public string DataTransferencia => Data?.ToShortDateString();
        public string ValorTransferencia => Valor.ToMoney();

        public string IndicadorPagamentoReceb => IndicadorPagoRecebido ?? "N";
        public bool IsPagoRecebido => IndicadorPagamentoReceb != "N";
        public bool IsVencido => !IsPagoRecebido && DateTime.Today > Data;


    }
}
