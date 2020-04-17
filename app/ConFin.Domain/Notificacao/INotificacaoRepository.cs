using ConFin.Common.Domain.Dto;
using ConFin.Common.Repository.Infra;
using System.Collections.Generic;

namespace ConFin.Domain.Notificacao
{
    public interface INotificacaoRepository: IBaseRepository
    {
        IEnumerable<NotificacaoDto> Get(int idUsuario);
        int GetTotalNaoLidas(int idUsuario);
        void PutDataLeitura(int idNotificacao);
        void Post(NotificacaoDto notificacao);
    }
}
