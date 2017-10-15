using ConFin.Common.Domain;
using ConFin.Common.Domain.Dto;
using System.Collections.Generic;
using System.Linq;

namespace ConFin.Domain.Notificacao
{
    public class NotificacaoService: INotificacaoService
    {
        private readonly INotificacaoRepository _notificacaoRepository;
        private readonly Notification _notification;

        public NotificacaoService(INotificacaoRepository notificacaoRepository, Notification notification)
        {
            _notificacaoRepository = notificacaoRepository;
            _notification = notification;
        }

        public IEnumerable<NotificacaoDto> Get(int idUsuario, bool notificacaoLida, out bool nenhumaNotificacao)
        {
            nenhumaNotificacao = false;
            var notificacoes = _notificacaoRepository.Get(idUsuario).ToList();

            if (!notificacoes.Any())
            {
                nenhumaNotificacao = true;
                return notificacoes;
            }

            if (notificacaoLida)
                return notificacoes.Where(x => x.DataLeitura != null);

            notificacoes = notificacoes.Where(x => x.DataLeitura == null).ToList();

            if (!notificacoes.Any())
                return notificacoes;

            _notificacaoRepository.OpenTransaction();

            foreach (var notificacao in notificacoes.Where(x => x.DataLeitura == null))
                _notificacaoRepository.PutDataLeitura(notificacao.Id);

            _notificacaoRepository.CommitTransaction();

            return notificacoes;
        }
    }
}
