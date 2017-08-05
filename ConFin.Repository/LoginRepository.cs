using ConFin.Common.Domain;
using ConFin.Common.Repository;
using ConFin.Common.Repository.Extension;
using ConFin.Common.Repository.Infra;
using ConFin.Domain.Login;
using System;

namespace ConFin.Repository
{
    public class LoginRepository : BaseRepository, ILoginRepository
    {
        public LoginRepository(IDatabaseConnection connection) : base(connection)
        {
        }

        private enum Procedures
        {
            SP_InsUsuario,
            SP_SelUsuario,
            SP_UpdConfirmacaoCadastroUsuario
        }

        public void Post(Usuario usuario)
        {
            ExecuteProcedure(Procedures.SP_InsUsuario);
            AddParameter("Nome", usuario.Nome);
            AddParameter("Email", usuario.Email);
            AddParameter("Senha", usuario.Senha);
            usuario.Id =  ExecuteNonQueryWithReturn();
        }

        public Usuario Get(string email, string senha)
        {
            ExecuteProcedure(Procedures.SP_SelUsuario);
            AddParameter("Email", email);
            AddParameter("Senha", senha);

            using (var reader = ExecuteReader())
            {
                return !reader.Read() ? null
                    : new Usuario
                    {
                        Id = reader.ReadAttr<int>("Id"),
                        Nome = reader.ReadAttr<string>("Nome"),
                        Email = reader.ReadAttr<string>("Email"),
                        Senha = reader.ReadAttr<string>("Senha"),
                        DataCadastro = reader.ReadAttr<DateTime>("DataCadastro"),
                        DataSolConfirmCadastro = reader.ReadAttr<DateTime?>("DataSolConfirmCadastro"),
                        DataConfirmCadastro = reader.ReadAttr<DateTime?>("DataConfirmCadastro"),
                        DataUltimaAlteracao = reader.ReadAttr<DateTime?>("DataUltimaAlteracao"),
                        DataDesativacao = reader.ReadAttr<DateTime?>("DataDesativacao")
                    };
            }
        }

        public void PutConfirmacaoCadastro(int idUsuario)
        {
            ExecuteProcedure(Procedures.SP_UpdConfirmacaoCadastroUsuario);
            AddParameter("Id", idUsuario);
            ExecuteNonQuery();
        }
    }
}
