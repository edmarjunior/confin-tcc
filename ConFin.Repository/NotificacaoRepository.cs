using ConFin.Common.Domain.Dto;
using ConFin.Common.Repository;
using ConFin.Common.Repository.Extension;
using ConFin.Common.Repository.Infra;
using ConFin.Domain.Notificacao;
using System;
using System.Collections.Generic;

namespace ConFin.Repository
{
    public class NotificacaoRepository: BaseRepository, INotificacaoRepository
    {
        public NotificacaoRepository(IDatabaseConnection connection) : base(connection)
        {
        }

        private enum Procedures
        {
            SP_SelNotificacao,
            FNC_PegaTotalNotificacaoNaoLida,
            SP_UpdDataLeituraNotificacao
        }

        public IEnumerable<NotificacaoDto> Get(int idUsuario)
        {
            ExecuteProcedure(Procedures.SP_SelNotificacao);
            AddParameter("IdUsuario", idUsuario);
            var notificacoes = new List<NotificacaoDto>();
            using (var reader = ExecuteReader())
            {
                while (reader.Read())
                {
                    notificacoes.Add(new NotificacaoDto
                    {
                        Id = reader.ReadAttr<int>("Id"),
                        IdTipo = reader.ReadAttr<short>("IdTipo"),
                        DescricaoTipo = reader.ReadAttr<string>("DescricaoTipo"),
                        IdUsuarioEnvio = reader.ReadAttr<int>("IdUsuarioEnvio"),
                        NomeUsuarioEnvio = reader.ReadAttr<string>("NomeUsuarioEnvio"),
                        IdUsuarioDestino = reader.ReadAttr<int>("IdUsuarioDestino"),
                        DataCadastro = reader.ReadAttr<DateTime>("DataCadastro"),
                        DataLeitura = reader.ReadAttr<DateTime?>("DataLeitura"),
                        Mensagem = reader.ReadAttr<string>("Mensagem"),
                        ParametrosUrl = reader.ReadAttr<string>("ParametrosUrl")
                    });
                }

                return notificacoes;
            }
        }

        public int GetTotalNaoLidas(int idUsuario)
        {
            ExecuteProcedure(Procedures.FNC_PegaTotalNotificacaoNaoLida);
            AddParameter("IdUsuario", idUsuario);
            return ExecuteNonQueryWithReturn();
        }

        public void PutDataLeitura(int idNotificacao)
        {
            ExecuteProcedure(Procedures.SP_UpdDataLeituraNotificacao);
            AddParameter("Id", idNotificacao);
            ExecuteNonQuery();
        }
    }
}
