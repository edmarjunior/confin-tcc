using ConFin.Common.Domain;
using ConFin.Common.Repository;
using ConFin.Common.Repository.Extension;
using ConFin.Common.Repository.Infra;
using ConFin.Domain.Login;
using ConFin.Domain.Login.Dto;
using System;

namespace ConFin.Repository
{
    public class LoginRepository : BaseRepository, ILoginRepository
    {
        private readonly Notification _notification;

        public LoginRepository(IDatabaseConnection connection, Notification notification) : base(connection)
        {
            _notification = notification;
        }

        private enum Procedures
        {
            SP_InsUsuario,
            SP_SelUsuario,
            SP_UpdConfirmacaoCadastroUsuario,
            SP_InsSolicitacaoTrocaSenhaLogin,
            SP_SelSolicitacaoTrocaSenhaLogin,
            SP_UpdSolicitacaoTrocaSenhaLogin
        }

        public void Post(Usuario usuario)
        {
            ExecuteProcedure(Procedures.SP_InsUsuario);
            AddParameter("Nome", usuario.Nome);
            AddParameter("Email", usuario.Email);
            AddParameter("Senha", usuario.Senha);
            usuario.Id = ExecuteNonQueryWithReturn();
        }

        public Usuario Get(string email, string senha)
        {
            ExecuteProcedure(Procedures.SP_SelUsuario);
            AddParameter("Email", email);
            AddParameter("Senha", senha);

            using (var reader = ExecuteReader())
            {
                return !reader.Read()
                    ? null
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

        public void PostSolicitacaoTrocaSenhaLogin(int idUsuario, string token)
        {
            ExecuteProcedure(Procedures.SP_InsSolicitacaoTrocaSenhaLogin);
            AddParameter("IdUsuario", idUsuario);
            AddParameter("Token", token);
            ExecuteNonQuery();
        }

        public SolicitacaoTrocaSenhaLoginDto GetSolicitacaoTrocaSenhaLogin(int idUsuario, string token)
        {
            ExecuteProcedure(Procedures.SP_SelSolicitacaoTrocaSenhaLogin);
            AddParameter("idUsuario", idUsuario);
            AddParameter("Token", token);

            using (var reader = ExecuteReader())
            {
                return !reader.Read()
                    ? null
                    : new SolicitacaoTrocaSenhaLoginDto
                    {
                        IdUsuario = reader.ReadAttr<int>("IdUsuario"),
                        Token = reader.ReadAttr<string>("Token"),
                        DataCadastro = reader.ReadAttr<DateTime>("DataCadastro"),
                        DataUsuarioConfirmacao = reader.ReadAttr<DateTime?>("DataUsuarioConfirmacao")
                    };
            }
        }

        public void PutSolicitacaoTrocaSenhaLogin(int idUsuario, string token)
        {
            ExecuteProcedure(Procedures.SP_UpdSolicitacaoTrocaSenhaLogin);
            AddParameter("idUsuario", idUsuario);
            AddParameter("Token", token);
            ExecuteNonQuery();
        }
    }
}
