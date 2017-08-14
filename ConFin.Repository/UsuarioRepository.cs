using ConFin.Common.Domain;
using ConFin.Common.Domain.Dto;
using ConFin.Common.Repository;
using ConFin.Common.Repository.Extension;
using ConFin.Common.Repository.Infra;
using ConFin.Domain.Usuario;
using System;

namespace ConFin.Repository
{
    public class UsuarioRepository : BaseRepository, IUsuarioRepository
    {
        private readonly Notification _notification;

        public UsuarioRepository(IDatabaseConnection connection, Notification notification) : base(connection)
        {
            _notification = notification;
        }

        private enum Procedures
        {
            SP_SelUsuario,
            SP_UpdUsuarioSenha
        }

        public UsuarioDto Get(int id)
        {
            ExecuteProcedure(Procedures.SP_SelUsuario);
            AddParameter("Id", id);

            using (var reader = ExecuteReader())
            {
                if (reader.Read())
                    return new UsuarioDto
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

                _notification.Add("Usuário não encontrado :(");
                return null;
            }
        }

        public void PutSenha(int id, string novaSenha)
        {
            ExecuteProcedure(Procedures.SP_UpdUsuarioSenha);
            AddParameter("Id", id);
            AddParameter("NovaSenha", novaSenha);
            ExecuteNonQuery();
        }
    }
}
