using ConFin.Common.Domain.Dto;
using ConFin.Common.Repository;
using ConFin.Common.Repository.Extension;
using ConFin.Common.Repository.Infra;
using ConFin.Domain.Compromisso;
using System;
using System.Collections.Generic;

namespace ConFin.Repository
{
    public class CompromissoRepository : BaseRepository, ICompromissoRepository
    {
        public CompromissoRepository(IDatabaseConnection connection) : base(connection)
        {
        }

        private enum Procedures
        {
            SP_InsCompromisso,
            SP_DelCompromisso,
            SP_SelCompromissoLancamento,
            SP_SelCompromissoLancamentos,
            SP_InsCompromissoLancamento,
            SP_DelCompromissoLancamento
        }

        public int Post(CompromissoDto compromisso)
        {
            ExecuteProcedure(Procedures.SP_InsCompromisso);
            AddParameter("Descricao", compromisso.Descricao);
            AddParameter("IdPeriodo", compromisso.IdPeriodo);
            AddParameter("DataInicio", compromisso.DataInicio);
            AddParameter("TotalParcelasOriginal", compromisso.TotalParcelasOriginal);
            AddParameter("IdUsuarioCadastro", compromisso.IdUsuarioCadastro);
            AddParameter("DataCadastro", compromisso.DataCadastro);
            AddParameter("IdConta", compromisso.IdConta);
            return ExecuteNonQueryWithReturn();
        }

        public void Delete(int id)
        {
            ExecuteProcedure(Procedures.SP_DelCompromisso);
            AddParameter("Id", id);
            ExecuteNonQuery();
        }

        public CompromissoDto GetCompromissoLancamento(int idLancamento)
        {
            ExecuteProcedure(Procedures.SP_SelCompromissoLancamento);
            AddParameter("IdLancamento", idLancamento);

            using (var reader = ExecuteReader())
                return !reader.Read()
                    ? null
                    : new CompromissoDto { Id = reader.ReadAttr<int>("Id") };
        }

        public IEnumerable<CompromissoLancamentoDto> GetCompromissoLancamentos(int idCompromisso)
        {
            ExecuteProcedure(Procedures.SP_SelCompromissoLancamentos);
            AddParameter("IdCompromisso", idCompromisso);

            var lancamentosCompromisso = new List<CompromissoLancamentoDto>();

            using (var reader = ExecuteReader())
                while (reader.Read())
                    lancamentosCompromisso.Add(new CompromissoLancamentoDto
                    {
                        IdCompromisso = reader.ReadAttr<int>("IdCompromisso"),
                        IdLancamento = reader.ReadAttr<int>("IdLancamento"),
                        NumeroLancamento = reader.ReadAttr<short>("NumeroLancamento"),
                        DataLancamento = reader.ReadAttr<DateTime>("DataLancamento")
                    });

            return lancamentosCompromisso;
        }

        public int PostCompromissoLancamento(int idCompromisso, int idLancamento, int numeroLancamento)
        {
            ExecuteProcedure(Procedures.SP_InsCompromissoLancamento);
            AddParameter("IdCompromisso", idCompromisso);
            AddParameter("IdLancamento", idLancamento);
            AddParameter("NumeroLancamento", numeroLancamento);
            return ExecuteNonQueryWithReturn();
        }

        public void DeleteCompromissoLancamento(int idLancamento)
        {
            ExecuteProcedure(Procedures.SP_DelCompromissoLancamento);
            AddParameter("IdLancamento", idLancamento);
            ExecuteNonQuery();
        }
    }
}
