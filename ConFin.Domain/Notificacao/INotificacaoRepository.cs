using ConFin.Common.Domain.Dto;
using System.Collections.Generic;

namespace ConFin.Domain.Notificacao
{
    public interface INotificacaoRepository
    {
        IEnumerable<NotificacaoDto> Get(int idUsuario);
    }
}
