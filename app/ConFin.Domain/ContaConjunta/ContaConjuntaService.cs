using ConFin.Common.Domain;
using ConFin.Common.Domain.Dto;
using ConFin.Domain.ContaFinanceira;
using ConFin.Domain.LancamentoCategoria;
using ConFin.Domain.Notificacao;
using ConFin.Domain.Usuario;
using System.Collections.Generic;
using System.Linq;

namespace ConFin.Domain.ContaConjunta
{
    public class ContaConjuntaService: IContaConjuntaService
    {
        private readonly IContaConjuntaRepository _contaConjuntaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILancamentoCategoriaRepository _lancamentoCategoriaRepository;
        private readonly INotificacaoRepository _notificacaoRepository;
        private readonly INotificacaoService _notificacaoService;
        private readonly IContaFinanceiraRepository _contaFinanceiraRepository;
        private readonly Notification _notification;

        public ContaConjuntaService(IContaConjuntaRepository contaConjuntaRepository, IUsuarioRepository usuarioRepository,
            Notification notification, ILancamentoCategoriaRepository lancamentoCategoriaRepository, 
            INotificacaoRepository notificacaoRepository, INotificacaoService notificacaoService, IContaFinanceiraRepository contaFinanceiraRepository)
        {
            _contaConjuntaRepository = contaConjuntaRepository;
            _usuarioRepository = usuarioRepository;
            _notification = notification;
            _lancamentoCategoriaRepository = lancamentoCategoriaRepository;
            _notificacaoRepository = notificacaoRepository;
            _notificacaoService = notificacaoService;
            _contaFinanceiraRepository = contaFinanceiraRepository;
        }

        public void Post(ContaConjuntaDto contaConjunta)
        {
            var usuarioConvidado = _usuarioRepository.Get(null, contaConjunta.EmailUsuarioConvidado);

            if (usuarioConvidado == null)
            {
                _notification.Add("Não foi encontrado nenhum usuário com o e-mail informado.");
                return;
            }

            if (usuarioConvidado.Id == contaConjunta.IdUsuarioEnvio)
            {
                _notification.Add("O e-mail informado não pode ser o mesmo do usuário desta conta");
                return;
            }

            if (_contaConjuntaRepository.Get(null, contaConjunta.IdConta).Any(x => x.IdUsuarioConvidado == usuarioConvidado.Id && contaConjunta.IndicadorAprovado == "A"))
            {
                _notification.Add("O usuário já esta vinculado com esta conta");
                return;
            }

            contaConjunta.IdUsuarioConvidado = usuarioConvidado.Id;

            _contaConjuntaRepository.OpenTransaction();

            _contaConjuntaRepository.Post(contaConjunta);

            var conta = _contaFinanceiraRepository.Get(contaConjunta.IdConta);

            _notificacaoRepository.Post(new NotificacaoDto
            {
                IdTipo = 1, // Convite para conta conjunta
                IdUsuarioEnvio = contaConjunta.IdUsuarioEnvio,
                IdUsuarioDestino = contaConjunta.IdUsuarioConvidado,
                Mensagem = $"Deseja compartilhar sua conta {conta.Nome.ToUpper()} com você",
                ParametrosUrl = null // Não implementado nessa versão do sistema
            });

            // Cadastra notificações para todos os usuarios (caso seja conta conjunta)
            var msg = $"Enviou um convite para compartilhar a conta {conta.Nome.ToUpper()} com o usuário {usuarioConvidado.Nome}";
            _notificacaoService.Post(contaConjunta.IdUsuarioEnvio, conta.Id, 1, msg, new List<int> { contaConjunta.IdUsuarioConvidado }); // 1: Convite para conta conjunta

            _contaConjuntaRepository.CommitTransaction();
        }

        public void Put(ContaConjuntaDto contaConjunta)
        {
            _contaConjuntaRepository.OpenTransaction();

            _contaConjuntaRepository.Put(contaConjunta);

            var conta = _contaFinanceiraRepository.Get(contaConjunta.IdConta);

            var msg = $"{(contaConjunta.IndicadorAprovado == "A" ? "Aceitou" : "Recusou")} seu convite para compartilhar a conta {conta.Nome.ToUpper()}";

            // IdTipo - 2: Aceitação de convite para conta conjunta. 3: Recuso de convite para conta conjunta
            var idTipoNotificacao = (short) (contaConjunta.IndicadorAprovado == "A" ? 2 : 3);

            _notificacaoRepository.Post(new NotificacaoDto
            {
                
                IdTipo = idTipoNotificacao, 
                IdUsuarioEnvio = contaConjunta.IdUsuarioConvidado,
                IdUsuarioDestino = contaConjunta.IdUsuarioEnvio,
                Mensagem = msg,
                ParametrosUrl = null // Não implementado nessa versão do sistema
            });

            var usuarioEnvio = _usuarioRepository.Get(contaConjunta.IdUsuarioEnvio);
            msg = $"{(contaConjunta.IndicadorAprovado == "A" ? "Aceitou" : "Recusou")} o convite do {usuarioEnvio.Nome} para compartilhar a conta {conta.Nome.ToUpper()}";

            // Cadastra notificações para todos os usuarios (caso seja conta conjunta)
            _notificacaoService.Post(contaConjunta.IdUsuarioConvidado, contaConjunta.IdConta, idTipoNotificacao, msg, new List<int> { contaConjunta.IdUsuarioEnvio });

            // caso o usuario RECUSE o convite para conta conjunta
            if (contaConjunta.IndicadorAprovado != "A")
            {
                _contaConjuntaRepository.CommitTransaction();
                return;
            }

            // caso o usuario ACEITE o convite para conta conjunta

            var categoriasConta = _lancamentoCategoriaRepository.GetCategoriasConta(contaConjunta.IdConta).ToList();
            var categoriasContaConjunta = _contaConjuntaRepository.GetCategoria(contaConjunta.IdConta).ToList();

            // insere as categorias que estão faltando ir para a tabela de categorias da conta conjunta
            foreach (var categoria in categoriasConta.Where(x => categoriasContaConjunta.All(y => y.Id != x.Id)))
                _contaConjuntaRepository.PostCategoria(contaConjunta.IdConta, categoria.Id);

            // exclui as categorias que estão na tabela de "contas conjuntas categorias" que não estão sendo utilizadas
            foreach (var categoriaContaConjunta in categoriasContaConjunta.Where(x => categoriasConta.All(y => y.Id != x.Id)))
                _contaConjuntaRepository.DeleteCategoria(contaConjunta.IdConta, categoriaContaConjunta.Id);

            _contaConjuntaRepository.CommitTransaction();
        }

        public void Delete(int idContaConjunta, int idUsuario)
        {
            var contaConjunta = _contaConjuntaRepository.Get(null, null, idContaConjunta).First();
            if (contaConjunta.IndicadorAprovado == "R")
            {
                _contaConjuntaRepository.Delete(idContaConjunta);
                return;
            }

            var conta = _contaFinanceiraRepository.Get(contaConjunta.IdConta);
            var usuarioConvidado = _usuarioRepository.Get(contaConjunta.IdUsuarioConvidado);

            string msg;

            _contaConjuntaRepository.OpenTransaction();

            if (idUsuario == usuarioConvidado.Id)
            {
                msg = $"Desvinculou-se da conta conjunta {conta.Nome.ToUpper()}";
                _notificacaoService.Post(idUsuario, contaConjunta.IdConta, 10, msg);
            }
            else
            {
                _notificacaoRepository.Post(new NotificacaoDto
                {

                    IdTipo = 10, // Cancelamento de compartilhamento de conta conjunta
                    IdUsuarioEnvio = idUsuario,
                    IdUsuarioDestino = usuarioConvidado.Id,
                    Mensagem = $"Excluiu o seu compartilhamento da conta {conta.Nome.ToUpper()}",
                    ParametrosUrl = null // Não implementado nessa versão do sistema
                });

                msg = $"Excluiu o compartilhamento da conta {conta.Nome.ToUpper()} com o {usuarioConvidado.Nome}";
                _notificacaoService.Post(idUsuario, contaConjunta.IdConta, 10, msg, new List<int> { usuarioConvidado.Id });
            }

            _contaConjuntaRepository.Delete(idContaConjunta);
            _contaConjuntaRepository.CommitTransaction();

        }
    }
}
