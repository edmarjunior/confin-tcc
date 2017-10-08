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
            SP_UpdContaConjunta,
            SP_SelContaConjuntaCategoria,
            SP_InsContaConjuntaCategoria
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
                        NomeConta = reader.ReadAttr<string>("NomeConta"),
                        IdConta = reader.ReadAttr<int>("IdConta")
                    });
                }

                return usuariosContaConjunta;
            }
        }

        public void Post(ContaConjuntaDto contaConjunta)
        {
            ExecuteProcedure(Procedures.SP_InsContaConjunta);
            AddParameter("IdConta", contaConjunta.IdConta);
            AddParameter("IdUsuarioEnvio", contaConjunta.IdUsuarioEnvio);
            AddParameter("IdUsuarioConvidado", contaConjunta.IdUsuarioConvidado);
            ExecuteNonQuery();
        }

        public void Delete(int idContaConjunta)
        {
            ExecuteProcedure(Procedures.SP_DelContaConjunta);
            AddParameter("Id", idContaConjunta);
            ExecuteNonQuery();
        }

        public void Put(ContaConjuntaDto contaConjunta)
        {
            ExecuteProcedure(Procedures.SP_UpdContaConjunta);
            AddParameter("Id", contaConjunta.Id);
            AddParameter("IndicadorAprovado", contaConjunta.IndicadorAprovado);
            ExecuteNonQuery();
        }

        public IEnumerable<LancamentoCategoriaDto> GetCategoria(int idConta)
        {
            ExecuteProcedure(Procedures.SP_SelContaConjuntaCategoria);
            AddParameter("IdConta", idConta);
            var categorias = new List<LancamentoCategoriaDto>();
            using (var reader = ExecuteReader())
            {
                while (reader.Read())
                {
                    categorias.Add(new LancamentoCategoriaDto
                    {
                        Id = reader.ReadAttr<int>("Id"),
                        Nome = reader.ReadAttr<string>("Nome")
                    });
                }

                return categorias;
            }
        }

        public void PostCategorias(int idConta)
        {
            ExecuteProcedure(Procedures.SP_InsContaConjuntaCategoria);
            AddParameter("IdConta", idConta);
            ExecuteNonQuery();
        }
    }
}
