using ConFin.Common.Domain.Dto;
using ConFin.Domain.ContaConjunta;
using System.Collections.Generic;
using System.Linq;

namespace ConFin.Domain.Notificacao
{
    public class NotificacaoService: INotificacaoService
    {
        public readonly INotificacaoRepository NotificacaoRepository;
        public readonly IContaConjuntaRepository ContaConjuntaRepository;

        public NotificacaoService(INotificacaoRepository notificacaoRepository, IContaConjuntaRepository contaConjuntaRepository)
        {
            NotificacaoRepository = notificacaoRepository;
            ContaConjuntaRepository = contaConjuntaRepository;
        }

        public IEnumerable<NotificacaoDto> Get(int idUsuario, bool notificacaoLida, out bool nenhumaNotificacao)
        {
            nenhumaNotificacao = false;
            var notificacoes = NotificacaoRepository.Get(idUsuario).OrderByDescending(x => x.DataCadastro).ToList();

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

            NotificacaoRepository.OpenTransaction();

            foreach (var notificacao in notificacoes.Where(x => x.DataLeitura == null))
                NotificacaoRepository.PutDataLeitura(notificacao.Id);

            NotificacaoRepository.CommitTransaction();

            return notificacoes;
        }

        public void Post(int idUsuarioEnvio, int idConta, short idTipo, string mensagem, List<int> idsUsuarioNaoEnviar = null)
        {
            var usuariosVinculados = ContaConjuntaRepository.Get(null, idConta).Where(x => x.IndicadorAprovado == "A").ToList();
            if (!usuariosVinculados.Any())
                return;

            var usuariosNotificacao = usuariosVinculados.Select(x => new { Id = x.IdUsuarioEnvio }).ToList();
            usuariosNotificacao.AddRange(usuariosVinculados.Select(x => new { Id = x.IdUsuarioConvidado }));
            usuariosNotificacao = usuariosNotificacao.Distinct().Where(x => x.Id != idUsuarioEnvio).ToList();

            if (idsUsuarioNaoEnviar != null)
                usuariosNotificacao = usuariosNotificacao.Where(x => !idsUsuarioNaoEnviar.Contains(x.Id)).ToList();

            foreach (var usuario in usuariosNotificacao)
            {
                NotificacaoRepository.Post(new NotificacaoDto
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
