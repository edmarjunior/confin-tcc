using ConFin.Common.Domain.Dto;
using ConFin.Common.Repository;
using ConFin.Common.Repository.Extension;
using ConFin.Common.Repository.Infra;
using ConFin.Domain.Lancamento;
using System;
using System.Collections.Generic;

namespace ConFin.Repository
{
    public class LancamentoRepository: BaseRepository, ILancamentoRepository
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
            SP_DelLancamento
        }

        public IEnumerable<LancamentoDto> GetAll(int idUsuario, int? idConta = null)
        {
            ExecuteProcedure(Procedures.SP_SelLancamentos);
            AddParameter("IdUsuario", idUsuario);
            AddParameter("IdConta", idConta);
            var lancamentos = new List<LancamentoDto>();
            using (var reader = ExecuteReader())
                while (reader.Read())
                    lancamentos.Add(new LancamentoDto
                    {
                        Id = reader.ReadAttr<int>("Id"),
                        Descricao = reader.ReadAttr<string>("Descricao"),
                        Valor = reader.ReadAttr<decimal>("Valor"),
                        Data = reader.ReadAttr<DateTime>("Data"),
                        IdConta = reader.ReadAttr<int>("IdConta"),
                        IdCategoria = reader.ReadAttr<int>("IdCategoria"),
                        NomeCategoria = reader.ReadAttr<string>("NomeCategoria"),
                        CorCategoria = reader.ReadAttr<string>("CorCategoria"),
                        IndicadorPago = reader.ReadAttr<string>("IndicadorPago")
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
                        Descricao = reader.ReadAttr<string>("Descricao"),
                        Valor = reader.ReadAttr<decimal>("Valor"),
                        Data = reader.ReadAttr<DateTime>("Data"),
                        IdConta = reader.ReadAttr<int>("IdConta"),
                        IdCategoria = reader.ReadAttr<int>("IdCategoria"),
                        IndicadorPago = reader.ReadAttr<string>("IndicadorPago"),

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

        public void Post(LancamentoDto lancamento)
        {
            ExecuteProcedure(Procedures.SP_InsLancamento);
            AddParameter("Descricao", lancamento.Descricao);
            AddParameter("Valor", lancamento.Valor);
            AddParameter("Data", lancamento.Data);
            AddParameter("IdConta", lancamento.IdConta);
            AddParameter("IdCategoria", lancamento.IdCategoria);
            AddParameter("IndicadorPago", lancamento.IndicadorPago);
            AddParameter("IdUsuario", lancamento.IdUsuarioCadastro);
            ExecuteNonQuery();
        }

        public void Put(LancamentoDto lancamento)
        {
            ExecuteProcedure(Procedures.SP_UpdLancamento);
            AddParameter("Descricao", lancamento.Descricao);
            AddParameter("Valor", lancamento.Valor);
            AddParameter("Data", lancamento.Data);
            AddParameter("IdConta", lancamento.IdConta);
            AddParameter("IdCategoria", lancamento.IdCategoria);
            AddParameter("IndicadorPago", lancamento.IndicadorPago);
            AddParameter("IdUsuario", lancamento.IdUsuarioUltimaAlteracao);
            ExecuteNonQuery();
        }

        public void Delete(int idLancamento)
        {
            ExecuteProcedure(Procedures.SP_DelLancamento);
            AddParameter("IdLancamento", idLancamento);
            ExecuteNonQuery();
        }
    }
}
