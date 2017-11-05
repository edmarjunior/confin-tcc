using ConFin.Common.Domain.Dto;
using ConFin.Domain.ContaConjunta;
using System.Collections.Generic;
using System.Linq;

namespace ConFin.Domain.Notificacao
{
    public class NotificacaoService: INotificacaoService
    {
        private readonly INotificacaoRepository _notificacaoRepository;
        private readonly IContaConjuntaRepository _contaConjuntaRepository;

        public NotificacaoService(INotificacaoRepository notificacaoRepository, IContaConjuntaRepository contaConjuntaRepository)
        {
            _notificacaoRepository = notificacaoRepository;
            _contaConjuntaRepository = contaConjuntaRepository;
        }

        public IEnumerable<NotificacaoDto> Get(int idUsuario, bool notificacaoLida, out bool nenhumaNotificacao)
        {
            nenhumaNotificacao = false;
            var notificacoes = _notificacaoRepository.Get(idUsuario).OrderByDescending(x => x.DataCadastro).ToList();

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

        public void Post(int idUsuarioEnvio, int idConta, short idTipo, string mensagem, List<int> idsUsuarioNaoEnviar = null)
        {
            var usuariosVinculados = _contaConjuntaRepository.Get(null, idConta).Where(x => x.IndicadorAprovado == "A").ToList();
            if (!usuariosVinculados.Any())
                return;

            var usuariosNotificacao = usuariosVinculados.Select(x => new { Id = x.IdUsuarioEnvio }).ToList();
            usuariosNotificacao.AddRange(usuariosVinculados.Select(x => new { Id = x.IdUsuarioConvidado }));
            usuariosNotificacao = usuariosNotificacao.Distinct().Where(x => x.Id != idUsuarioEnvio).ToList();

            if (idsUsuarioNaoEnviar != null)
                usuariosNotificacao = usuariosNotificacao.Where(x => !idsUsuarioNaoEnviar.Contains(x.Id)).ToList();

            foreach (var usuario in usuariosNotificacao)
            {
                _notificacaoRepository.Post(new NotificacaoDto
                {
                    IdTipo = idTipo,
                    IdUsuarioEnvio = idUsuarioEnvio,
                    IdUsuarioDestino = usuario.Id,
                    Mensagem = mensagem,
                    ParametrosUrl = null // Não implementado nessa versão do sistema
                });
            }
        }
    }
}
