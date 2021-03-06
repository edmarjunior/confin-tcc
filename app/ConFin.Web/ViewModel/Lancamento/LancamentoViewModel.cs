﻿using ConFin.Common.Domain.Auxiliar;
using ConFin.Common.Domain.Dto;
using ConFin.Common.Web.Extension;
using System;
using System.Collections.Generic;

namespace ConFin.Web.ViewModel.Lancamento
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
            IdContaDestino = model.IdContaDestino;
            NomeContaOrigem = model.NomeContaOrigem;
            NomeContaDestino = model.NomeContaDestino;
            IdCategoria = model.IdCategoria;
            NomeCategoria = model.NomeCategoria;
            CorCategoria = model.CorCategoria;
            IndicadorPagoRecebido = model.IndicadorPagoRecebido;
            IdCompromisso = model.IdCompromisso;
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
        public string NomeContaOrigem { get; set; }
        public int? IdContaDestino { get; set; }
        public string NomeContaDestino { get; set; }
        public int? IdCategoria { get; set; }
        public string NomeCategoria { get; set; }
        public string CorCategoria { get; set; }
        public string IndicadorPagoRecebido { get; set; }
        public int? IdCompromisso { get; set; }
        public bool UsuarioPodeManipularLancamento { get; set; }

        public IEnumerable<ContaFinanceiraDto> ContasFinanceira{ get; set; }
        public IEnumerable<LancamentoCategoriaDto> Categorias { get; set; }

        public string IndicadorPagamentoReceb => IndicadorPagoRecebido ?? "N";

        public string DataLancamento => Data?.ToShortDateString();
        public string ValorLancamento => Valor.ToMoney();

        public string ShouldCheckReceitaDespesa(string indicadorReceitaDespesa)
        {
            return IndicadorReceitaDespesa == indicadorReceitaDespesa ? "checked" : string.Empty;
        }

        public bool IsReceita => IndicadorReceitaDespesa == "R";
        public bool IsReceitaRealizada => IsReceita && IsPagoRecebido;

        public bool IsDespesa => IndicadorReceitaDespesa == "D";
        public bool IsDespesaRealizada => IsDespesa && IsPagoRecebido;

        public bool IsPagoRecebido => IndicadorPagamentoReceb != "N";
        public bool IsVencido => !IsPagoRecebido && DateTime.Today > Data;
        public bool DataLancamentoMenorHoje => DateTime.Today > Data;

        public bool IsTransferencia => IndicadorReceitaDespesa == "T";
        public bool UsuarioBloqueadoAlterar => IsTransferencia && !UsuarioPodeManipularLancamento;
    }
}
