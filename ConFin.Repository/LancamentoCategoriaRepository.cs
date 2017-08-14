using ConFin.Common.Domain.Dto;
using ConFin.Common.Repository;
using ConFin.Common.Repository.Extension;
using ConFin.Common.Repository.Infra;
using ConFin.Domain.LancamentoCategoria;
using System;
using System.Collections.Generic;

namespace ConFin.Repository
{
    public class LancamentoCategoriaRepository : BaseRepository, ILancamentoCategoriaRepository
    {
        public LancamentoCategoriaRepository(IDatabaseConnection connection) : base(connection)
        {
        }

        private enum Procedures
        {
            SP_SelLancamentoCategorias,
            SP_SelLancamentoCategoria,
            SP_InsLancamentoCategoria,
            SP_UpdLancamentoCategoria,
            SP_DelLancamentoCategoria
        }

        public IEnumerable<LancamentoCategoriaDto> Get(int idUsuario)
        {
            ExecuteProcedure(Procedures.SP_SelLancamentoCategorias);
            AddParameter("IdUsuario", idUsuario);
            var categorias = new List<LancamentoCategoriaDto>();
            using (var reader = ExecuteReader())
                while (reader.Read())
                    categorias.Add(new LancamentoCategoriaDto
                    {
                        Id = reader.ReadAttr<int>("Id"),
                        Nome = reader.ReadAttr<string>("Nome"),
                        Cor = reader.ReadAttr<string>("Cor")
                    });

            return categorias;
        }

        public LancamentoCategoriaDto Get(int idUsuario, int idCategoria)
        {
            ExecuteProcedure(Procedures.SP_SelLancamentoCategoria);
            AddParameter("IdUsuario", idUsuario);
            AddParameter("IdCategoria", idCategoria);

            using (var reader = ExecuteReader())
            {
                return !reader.Read()
                    ? null
                    : new LancamentoCategoriaDto
                    {
                        Id = reader.ReadAttr<int>("Id"),
                        Nome = reader.ReadAttr<string>("Nome"),
                        Cor = reader.ReadAttr<string>("Cor"),

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

        public void Post(LancamentoCategoriaDto categoria)
        {
            ExecuteProcedure(Procedures.SP_InsLancamentoCategoria);
            AddParameter("Nome", categoria.Nome);
            AddParameter("IdCategoriaSuperior", categoria.IdCategoriaSuperior);
            AddParameter("Cor", categoria.Cor);
            AddParameter("IdUsuarioCadastro", categoria.IdUsuarioCadastro);
            ExecuteNonQuery();
        }

        public void Put(LancamentoCategoriaDto categoria)
        {
            ExecuteProcedure(Procedures.SP_UpdLancamentoCategoria);
            AddParameter("Nome", categoria.Nome);
            AddParameter("IdCategoriaSuperior", categoria.IdCategoriaSuperior);
            AddParameter("Cor", categoria.Cor);
            AddParameter("IdUsuario", categoria.IdUsuarioUltimaAlteracao);
            AddParameter("IdCategoria", categoria.Id);
            ExecuteNonQuery();
        }

        public void Delete(int idUsuario, int idCategoria)
        {
            ExecuteProcedure(Procedures.SP_DelLancamentoCategoria);
            AddParameter("IdCategoria", idCategoria);
            ExecuteNonQuery();
        }
    }
}
