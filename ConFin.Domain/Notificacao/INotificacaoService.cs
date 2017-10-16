using ConFin.Common.Domain.Dto;
using System.Collections.Generic;

namespace ConFin.Domain.Notificacao
{
    public interface INotificacaoService
    {
        IEnumerable<NotificacaoDto> Get(int idUsuario, bool notificacaoLida, out bool nenhumaNotificacao);
        void Post(int idUsuarioEnvio, int idConta, short idTipo, string mensagem, List<int> idsUsuarioNaoEnviar = null);
    }
}
