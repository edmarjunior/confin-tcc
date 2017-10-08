using ConFin.Common.Domain.Dto;
using ConFin.Common.Repository;
using ConFin.Common.Repository.Extension;
using ConFin.Common.Repository.Infra;
using ConFin.Domain.ContaConjunta;
using System;
using System.Collections.Generic;

namespace ConFin.Repository
{
    public class ContaConjuntaRepository : BaseRepository, IContaConjuntaRepository
    {
        public ContaConjuntaRepository(IDatabaseConnection connection) : base(connection)
        {
        }

        private enum Procedures
        {
            SP_SelContaConjunta,
            SP_InsContaConjunta,
            SP_DelContaConjunta,
            SP_UpdContaConjunta
        }

        public IEnumerable<ContaConjuntaDto> Get(int? idUsuario, int? idConta = null)
        {
            ExecuteProcedure(Procedures.SP_SelContaConjunta);
            AddParameter("IdUsuario", idUsuario);
            AddParameter("IdConta", idConta);
            var usuariosContaConjunta = new List<ContaConjuntaDto>();
            using (var reader = ExecuteReader())
            {
                while (reader.Read())
                {

                    usuariosContaConjunta.Add(new ContaConjuntaDto
                    {
                        Id = reader.ReadAttr<int>("Id"),
                        NomeUsuarioEnvio = reader.ReadAttr<string>("NomeUsuarioEnvio"),
                        EmailUsuarioEnvio = reader.ReadAttr<string>("EmailUsuarioEnvio"),
                        NomeUsuarioConvidado = reader.ReadAttr<string>("NomeUsuarioConvidado"),
                        EmailUsuarioConvidado = reader.ReadAttr<string>("EmailUsuarioConvidado"),
                        DataCadastro = reader.ReadAttr<DateTime>("DataCadastro"),
                        DataAnalise = reader.ReadAttr<DateTime?>("DataAnalise"),
                        IndicadorAprovado = reader.ReadAttr<string>("IndicadorAprovado"),
                        IdUsuarioConvidado = reader.ReadAttr<int>("IdUsuarioConvidado"),
                        NomeConta = reader.ReadAttr<string>("NomeConta")
                    });
                }

                return usuariosContaConjunta;
            }
        }

        public void Post(int idConta, int idUsuarioEnvio, int idUsuarioConvidado)
        {
            ExecuteProcedure(Procedures.SP_InsContaConjunta);
            AddParameter("IdConta", idConta);
            AddParameter("IdUsuarioEnvio", idUsuarioEnvio);
            AddParameter("IdUsuarioConvidado", idUsuarioConvidado);
            ExecuteNonQuery();
        }

        public void Delete(int idContaConjunta)
        {
            ExecuteProcedure(Procedures.SP_DelContaConjunta);
            AddParameter("Id", idContaConjunta);
            ExecuteNonQuery();
        }

        public void Put(int idContaConjunta, string indicadorAprovado)
        {
            ExecuteProcedure(Procedures.SP_UpdContaConjunta);
            AddParameter("Id", idContaConjunta);
            AddParameter("IndicadorAprovado", indicadorAprovado);
            ExecuteNonQuery();
        }

    }
}
