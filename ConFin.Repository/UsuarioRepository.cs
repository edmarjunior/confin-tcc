using ConFin.Common.Repository;
using ConFin.Common.Repository.Extension;
using ConFin.Common.Repository.Infra;
using ConFin.Domain.Usuario;
using ConFin.Domain.Usuario.Dto;
using System;

namespace ConFin.Repository
{
    public class UsuarioRepository : BaseRepository, IUsuarioRepository
    {
        public UsuarioRepository(IDatabaseConnection connection) : base(connection)
        {
        }

        private enum Procedures
        {
            SP_InsUsuario,
            SP_SelUsuario
        }

        public void Post(UsuarioDto usuario)
        {
            ExecuteProcedure(Procedures.SP_InsUsuario);
            AddParameter("Nome", usuario.Nome);
            AddParameter("Email", usuario.Email);
            AddParameter("Senha", usuario.Senha);
            ExecuteNonQuery();
        }

        public UsuarioDto Get(string email, string senha)
        {
            ExecuteProcedure(Procedures.SP_SelUsuario);
            AddParameter("Email", email);
            AddParameter("Senha", senha);

            using (var reader = ExecuteReader())
            {
                return !reader.Read() ? null
                    : new UsuarioDto
                    {
                        Id = reader.ReadAttr<int>("Id"),
                        Nome = reader.ReadAttr<string>("Nome"),
                        Email = reader.ReadAttr<string>("Email"),
                        Senha = reader.ReadAttr<string>("Senha"),
                        DataCadastro = reader.ReadAttr<DateTime>("DataCadastro"),
                        DataConfirmacaoEmail = reader.ReadAttr<DateTime?>("DataConfirmacaoEmail"),
                        DataUltimaAlteracao = reader.ReadAttr<DateTime?>("DataUltimaAlteracao"),
                        DataDesativacao = reader.ReadAttr<DateTime?>("DataDesativacao")
                    };
            }
        }
    }
}
