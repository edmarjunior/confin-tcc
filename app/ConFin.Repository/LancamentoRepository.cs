﻿using ConFin.Common.Domain.Dto;
using ConFin.Common.Repository;
using ConFin.Common.Repository.Extension;
using ConFin.Common.Repository.Infra;
using ConFin.Domain.Lancamento;
using System;
using System.Collections.Generic;

namespace ConFin.Repository
{
    public class LancamentoRepository : BaseRepository, ILancamentoRepository
    {
        public LancamentoRepository(IDatabaseConnection connection) : base(connection)
        {
        }

        private enum Procedures
        {
            SP_SelLancamentos,
            SP_SelLancamento,
            SP_InsLancamento,
            SP_UpdLancamento,
            SP_DelLancamento,
            SP_UpdLancamentoIndicadorPagoRecebido,
            SP_SelLancamentosResumo,
            SP_SelPeriodo
        }

        public IEnumerable<LancamentoDto> GetAll(int idUsuario, byte? mes = null, short? ano = null, int? idConta = null, int? idCategoria = null)
        {
            ExecuteProcedure(Procedures.SP_SelLancamentos);
            AddParameter("IdUsuario", idUsuario);
            AddParameter("IdConta", idConta);
            AddParameter("IdCategoria", idCategoria);
            AddParameter("Mes", mes);
            AddParameter("Ano", ano);

            var lancamentos = new List<LancamentoDto>();
            using (var reader = ExecuteReader())
                while (reader.Read())
                    lancamentos.Add(new LancamentoDto
                    {
                        Id = reader.ReadAttr<int>("Id"),
                        IndicadorReceitaDespesa = reader.ReadAttr<string>("IndicadorReceitaDespesa"),
                        Descricao = reader.ReadAttr<string>("Descricao"),
                        Valor = reader.ReadAttr<decimal>("Valor"),
                        Data = reader.ReadAttr<DateTime>("DataLancamento"),
                        IdConta = reader.ReadAttr<int>("IdConta"),
                        NomeContaOrigem = reader.ReadAttr<string>("NomeContaOrigem"),
                        IdContaDestino = reader.ReadAttr<int?>("IdContaDestino"),
                        NomeContaDestino = reader.ReadAttr<string>("NomeContaDestino"),
                        IdCategoria = reader.ReadAttr<int>("IdCategoria"),
                        NomeCategoria = reader.ReadAttr<string>("NomeCategoria"),
                        CorCategoria = reader.ReadAttr<string>("CorCategoria"),
                        IndicadorPagoRecebido = reader.ReadAttr<string>("IndicadorPagoRecebido"),
                        IdCompromisso = reader.ReadAttr<int?>("IdCompromisso"),
                        // manut
                        IdUsuarioCadastro = reader.ReadAttr<int>("IdUsuarioCadastro"),
                        NomeUsuarioCadastro = reader.ReadAttr<string>("NomeUsuarioCadastro"),
                        DataCadastro = reader.ReadAttr<DateTime>("DataCadastro"),
                        IdUsuarioUltimaAlteracao = reader.ReadAttr<int?>("IdUsuarioUltimaAlteracao"),
                        NomeUsuarioUltimaAlteracao = reader.ReadAttr<string>("NomeUsuarioUltimaAlteracao"),
                        DataUltimaAlteracao = reader.ReadAttr<DateTime?>("DataUltimaAlteracao")
                    });

            return lancamentos;
        }

        public LancamentoDto Get(int idLancamento)
        {
            ExecuteProcedure(Procedures.SP_SelLancamento);
            AddParameter("IdLancamento", idLancamento);

            using (var reader = ExecuteReader())
            {
                return !reader.Read()
                    ? null
                    : new LancamentoDto
                    {
                        Id = reader.ReadAttr<int>("Id"),
                        IndicadorReceitaDespesa = reader.ReadAttr<string>("IndicadorReceitaDespesa"),
                        Descricao = reader.ReadAttr<string>("Descricao"),
                        Valor = reader.ReadAttr<decimal>("Valor"),
                        Data = reader.ReadAttr<DateTime>("DataLancamento"),
                        IdConta = reader.ReadAttr<int>("IdConta"),
                        IdCategoria = reader.ReadAttr<int>("IdCategoria"),
                        IndicadorPagoRecebido = reader.ReadAttr<string>("IndicadorPagoRecebido"),
                        IdCompromisso = reader.ReadAttr<int?>("IdCompromisso"),

                        // manut
                        IdUsuarioCadastro = reader.ReadAttr<int>("IdUsuarioCadastro"),
                        NomeUsuarioCadastro = reader.ReadAttr<string>("NomeUsuarioCadastro"),
                        DataCadastro = reader.ReadAttr<DateTime>("DataCadastro"),
                        IdUsuarioUltimaAlteracao = reader.ReadAttr<int?>("IdUsuarioUltimaAlteracao"),
                        NomeUsuarioUltimaAlteracao = reader.ReadAttr<string>("NomeUsuarioUltimaAlteracao"),
                        DataUltimaAlteracao = reader.ReadAttr<DateTime?>("DataUltimaAlteracao")
                    };
            }
        }

        public int Post(LancamentoDto lancamento)
        {
            ExecuteProcedure(Procedures.SP_InsLancamento);
            AddParameter("IndicadorReceitaDespesa", lancamento.IndicadorReceitaDespesa);
            AddParameter("Descricao", lancamento.Descricao);
            AddParameter("Valor", lancamento.Valor);
            AddParameter("DataLancamento", lancamento.Data);
            AddParameter("IdConta", lancamento.IdConta);
            AddParameter("IdCategoria", lancamento.IdCategoria);
            AddParameter("IndicadorPagoRecebido", lancamento.IndicadorPagoRecebido);
            AddParameter("IdUsuario", lancamento.IdUsuarioCadastro);
            return ExecuteNonQueryWithReturn();
        }

        public void Put(LancamentoDto lancamento)
        {
            ExecuteProcedure(Procedures.SP_UpdLancamento);
            AddParameter("IndicadorReceitaDespesa", lancamento.IndicadorReceitaDespesa);
            AddParameter("Descricao", lancamento.Descricao);
            AddParameter("Valor", lancamento.Valor);
            AddParameter("DataLancamento", lancamento.Data);
            AddParameter("IdConta", lancamento.IdConta);
            AddParameter("IdCategoria", lancamento.IdCategoria);
            AddParameter("IndicadorPagoRecebido", lancamento.IndicadorPagoRecebido);
            AddParameter("IdUsuario", lancamento.IdUsuarioUltimaAlteracao);
            AddParameter("IdLancamento", lancamento.Id);
            ExecuteNonQuery();
        }

        public void Delete(int idLancamento)
        {
            ExecuteProcedure(Procedures.SP_DelLancamento);
            AddParameter("IdLancamento", idLancamento);
            ExecuteNonQuery();
        }

        public void PutIndicadorPagoRecebido(LancamentoDto lancamento)
        {
            ExecuteProcedure(Procedures.SP_UpdLancamentoIndicadorPagoRecebido);
            AddParameter("IdLancamento", lancamento.Id);
            AddParameter("IndicadorPagoRecebido", lancamento.IndicadorPagoRecebido);
            AddParameter("IdUsuario", lancamento.IdUsuarioUltimaAlteracao);
            ExecuteNonQuery();
        }

        public LancamentoResumoGeralDto GetResumo(int idUsuario, byte mes, short ano, int? idConta = null, int? idCategoria = null)
        {
            ExecuteProcedure(Procedures.SP_SelLancamentosResumo);
            AddParameter("IdUsuario", idUsuario);
            AddParameter("IdConta", idConta);
            AddParameter("IdCategoria", idCategoria);
            AddParameter("Mes", mes);
            AddParameter("Ano", ano);

            using (var reader = ExecuteReader())
            {
                return !reader.Read()
                    ? null
                    : new LancamentoResumoGeralDto
                    {
                        TotReceitasPrevista = reader.ReadAttr<decimal>("TotReceitasPrevista"),
                        TotReceitasRealizada = reader.ReadAttr<decimal>("TotReceitasRealizada"),
                        TotDespesasPrevista = reader.ReadAttr<decimal>("TotDespesasPrevista"),
                        TotDespesasRealizada = reader.ReadAttr<decimal>("TotDespesasRealizada"),
                        TotSaldoPrevisto = reader.ReadAttr<decimal>("TotSaldoPrevisto"),
                        TotSaldoAtual = reader.ReadAttr<decimal>("TotSaldoAtual"),
                        TotValorSaldoInicialConta = reader.ReadAttr<decimal>("TotValorSaldoInicialConta")
                    };
            }
        }

        public IEnumerable<PeriodoDto> GetPeriodo()
        {
            ExecuteProcedure(Procedures.SP_SelPeriodo);
            var periodos = new List<PeriodoDto>();
            using (var reader = ExecuteReader())
                while (reader.Read())
                    periodos.Add(new PeriodoDto
                    {
                        Id = reader.ReadAttr<byte>("Id"),
                        Descricao = reader.ReadAttr<string>("Descricao"),
                        Quantidade = reader.ReadAttr<byte>("Quantidade"),
                        IndicadorDiaMes = reader.ReadAttr<string>("IndicadorDiaMes")
                    });

            return periodos;
        }

        public PeriodoDto GetPeriodo(byte id)
        {
            ExecuteProcedure(Procedures.SP_SelPeriodo);
            AddParameter("Id", id);
            using (var reader = ExecuteReader())
                return !reader.Read()
                    ? null
                    : new PeriodoDto
                    {
                        Id = reader.ReadAttr<byte>("Id"),
                        Descricao = reader.ReadAttr<string>("Descricao"),
                        Quantidade = reader.ReadAttr<byte>("Quantidade"),
                        IndicadorDiaMes = reader.ReadAttr<string>("IndicadorDiaMes")
                    };
        }

        
    }
}
