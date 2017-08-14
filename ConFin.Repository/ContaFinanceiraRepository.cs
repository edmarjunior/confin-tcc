using ConFin.Common.Domain.Dto;
using ConFin.Common.Repository;
using ConFin.Common.Repository.Extension;
using ConFin.Common.Repository.Infra;
using ConFin.Domain.ContaFinanceira;
using System;
using System.Collections.Generic;

namespace ConFin.Repository
{
    public class ContaFinanceiraRepository : BaseRepository, IContaFinanceiraRepository
    {
        public ContaFinanceiraRepository(IDatabaseConnection connection) : base(connection)
        {
        }

        private enum Procedures
        {
            SP_SelContasFinanceira,
            SP_SelContaFinanceira,
            SP_InsContaFinanceira,
            SP_UpdContaFinanceira,
            SP_DelContaFinanceira
        }

        public IEnumerable<ContaFinanceiraDto> GetAll(int idUsuario)
        {
            ExecuteProcedure(Procedures.SP_SelContasFinanceira);
            AddParameter("IdUsuario", idUsuario);
            var contas = new List<ContaFinanceiraDto>();
            using (var reader = ExecuteReader())
                while (reader.Read())
                    contas.Add(new ContaFinanceiraDto
                    {
                        Id = reader.ReadAttr<int>("Id"),
                        Nome = reader.ReadAttr<string>("Nome"),
                        IdTipo = reader.ReadAttr<byte>("IdTipo"),
                        NomeTipo = reader.ReadAttr<string>("NomeTipo"),
                        ValorSaldoInicial = reader.ReadAttr<decimal>("ValorSaldoInicial"),
                        Descricao = reader.ReadAttr<string>("Descricao"),
                        Saldo = reader.ReadAttr<decimal>("Saldo")
                    });

            return contas;
        }

        public ContaFinanceiraDto Get(int idConta)
        {
            ExecuteProcedure(Procedures.SP_SelContaFinanceira);
            AddParameter("IdConta", idConta);

            using (var reader = ExecuteReader())
            {
                return !reader.Read()
                    ? null
                    : new ContaFinanceiraDto
                    {
                        Id = reader.ReadAttr<int>("Id"),
                        Nome = reader.ReadAttr<string>("Nome"),
                        IdTipo = reader.ReadAttr<byte>("IdTipo"),
                        ValorSaldoInicial = reader.ReadAttr<decimal>("ValorSaldoInicial"),
                        Descricao = reader.ReadAttr<string>("Descricao"),
                        Saldo = reader.ReadAttr<decimal>("Saldo"),
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

        public void Post(ContaFinanceiraDto conta)
        {
            ExecuteProcedure(Procedures.SP_InsContaFinanceira);
            AddParameter("Nome", conta.Nome);
            AddParameter("IdTipo", conta.IdTipo);
            AddParameter("ValorSaldoInicial", conta.ValorSaldoInicial);
            AddParameter("Descricao", conta.Descricao);
            AddParameter("IdUsuarioCadastro", conta.IdUsuarioCadastro);
            ExecuteNonQuery();
        }

        public void Put(ContaFinanceiraDto conta)
        {
            ExecuteProcedure(Procedures.SP_UpdContaFinanceira);
            AddParameter("IdConta", conta.Id);
            AddParameter("IdUsuario", conta.IdUsuarioUltimaAlteracao);
            AddParameter("Nome", conta.Nome);
            AddParameter("IdTipo", conta.IdTipo);
            AddParameter("ValorSaldoInicial", conta.ValorSaldoInicial);
            AddParameter("Descricao", conta.Descricao);
            ExecuteNonQuery();
        }

        public void Delete(int idUsuario, int idConta)
        {
            ExecuteProcedure(Procedures.SP_DelContaFinanceira);
            // AddParameter("IdUsuario", idUsuario);
            AddParameter("IdConta", idConta);
            ExecuteNonQuery();
        }
    }
}
