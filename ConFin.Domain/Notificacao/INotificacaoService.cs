using ConFin.Common.Domain.Dto;
using System.Collections.Generic;

namespace ConFin.Domain.Notificacao
{
    public interface INotificacaoService
    {
        IEnumerable<NotificacaoDto> Get(int idUsuario, bool notificacaoLida, out bool nenhumaNotificacao);
    }
}
