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
        public readonly IContaConjuntaRepository ContaConjuntaRepository;
        public readonly IUsuarioRepository UsuarioRepository;
        public readonly ILancamentoCategoriaRepository LancamentoCategoriaRepository;
        public readonly INotificacaoRepository NotificacaoRepository;
        public readonly INotificacaoService NotificacaoService;
        public readonly IContaFinanceiraRepository ContaFinanceiraRepository;

        public readonly Notification Notification;

        public ContaConjuntaService(IContaConjuntaRepository contaConjuntaRepository, IUsuarioRepository usuarioRepository,
            Notification notification, ILancamentoCategoriaRepository lancamentoCategoriaRepository, 
            INotificacaoRepository notificacaoRepository, INotificacaoService notificacaoService, IContaFinanceiraRepository contaFinanceiraRepository)
        {
            ContaConjuntaRepository = contaConjuntaRepository;
            UsuarioRepository = usuarioRepository;
            Notification = notification;
            LancamentoCategoriaRepository = lancamentoCategoriaRepository;
            NotificacaoRepository = notificacaoRepository;
            NotificacaoService = notificacaoService;
            ContaFinanceiraRepository = contaFinanceiraRepository;
        }

        public void Post(ContaConjuntaDto contaConjunta)
        {
            var usuarioConvidado = UsuarioRepository.Get(null, contaConjunta.EmailUsuarioConvidado);

            if (usuarioConvidado == null)
            {
                Notification.Add("Não foi encontrado nenhum usuário com o e-mail informado.");
                return;
            }

            if (usuarioConvidado.Id == contaConjunta.IdUsuarioEnvio)
            {
                Notification.Add("O e-mail informado não pode ser o mesmo do usuário desta conta");
                return;
            }

            if (ContaConjuntaRepository.Get(null, contaConjunta.IdConta).Any(x => x.IdUsuarioConvidado == usuarioConvidado.Id && contaConjunta.IndicadorAprovado == "A"))
            {
                Notification.Add("O usuário já esta vinculado com esta conta");
                return;
            }

            contaConjunta.IdUsuarioConvidado = usuarioConvidado.Id;

            ContaConjuntaRepository.OpenTransaction();

            ContaConjuntaRepository.Post(contaConjunta);

            var conta = ContaFinanceiraRepository.Get(contaConjunta.IdConta);

            NotificacaoRepository.Post(new NotificacaoDto
            {
                IdTipo = 1, // Convite para conta conjunta
                IdUsuarioEnvio = contaConjunta.IdUsuarioEnvio,
                IdUsuarioDestino = contaConjunta.IdUsuarioConvidado,
                Mensagem = $"Deseja compartilhar sua conta {conta.Nome.ToUpper()} com você",
                ParametrosUrl = null // Não implementado nessa versão do sistema
            });

            // Cadastra notificações para todos os usuarios (caso seja conta conjunta)
            var msg = $"Enviou um convite para compartilhar a conta {conta.Nome.ToUpper()} com o usuário {usuarioConvidado.Nome}";
            NotificacaoService.Post(contaConjunta.IdUsuarioEnvio, conta.Id, 1, msg, new List<int> { contaConjunta.IdUsuarioConvidado }); // 1: Convite para conta conjunta

            ContaConjuntaRepository.CommitTransaction();
        }

        public void Put(ContaConjuntaDto contaConjunta)
        {
            ContaConjuntaRepository.OpenTransaction();

            ContaConjuntaRepository.Put(contaConjunta);

            var conta = ContaFinanceiraRepository.Get(contaConjunta.IdConta);

            var msg = $"{(contaConjunta.IndicadorAprovado == "A" ? "Aceitou" : "Recusou")} seu convite para compartilhar a conta {conta.Nome.ToUpper()}";

            // IdTipo - 2: Aceitação de convite para conta conjunta. 3: Recuso de convite para conta conjunta
            var idTipoNotificacao = (short) (contaConjunta.IndicadorAprovado == "A" ? 2 : 3);

            NotificacaoRepository.Post(new NotificacaoDto
            {
                
                IdTipo = idTipoNotificacao, 
                IdUsuarioEnvio = contaConjunta.IdUsuarioConvidado,
                IdUsuarioDestino = contaConjunta.IdUsuarioEnvio,
                Mensagem = msg,
                ParametrosUrl = null // Não implementado nessa versão do sistema
            });

            var usuarioEnvio = UsuarioRepository.Get(contaConjunta.IdUsuarioEnvio);
            msg = $"{(contaConjunta.IndicadorAprovado == "A" ? "Aceitou" : "Recusou")} o convite do {usuarioEnvio.Nome} para compartilhar a conta {conta.Nome.ToUpper()}";

            // Cadastra notificações para todos os usuarios (caso seja conta conjunta)
            NotificacaoService.Post(contaConjunta.IdUsuarioConvidado, contaConjunta.IdConta, idTipoNotificacao, msg, new List<int> { contaConjunta.IdUsuarioEnvio });

            // caso o usuario RECUSE o convite para conta conjunta
            if (contaConjunta.IndicadorAprovado != "A")
            {
                ContaConjuntaRepository.CommitTransaction();
                return;
            }

            // caso o usuario ACEITE o convite para conta conjunta

            var categoriasConta = LancamentoCategoriaRepository.GetCategoriasConta(contaConjunta.IdConta).ToList();
            var categoriasContaConjunta = ContaConjuntaRepository.GetCategoria(contaConjunta.IdConta).ToList();

            // insere as categorias que estão faltando ir para a tabela de categorias da conta conjunta
            foreach (var categoria in categoriasConta.Where(x => categoriasContaConjunta.All(y => y.Id != x.Id)))
                ContaConjuntaRepository.PostCategoria(contaConjunta.IdConta, categoria.Id);

            // exclui as categorias que estão na tabela de "contas conjuntas categorias" que não estão sendo utilizadas
            foreach (var categoriaContaConjunta in categoriasContaConjunta.Where(x => categoriasConta.All(y => y.Id != x.Id)))
                ContaConjuntaRepository.DeleteCategoria(contaConjunta.IdConta, categoriaContaConjunta.Id);

            ContaConjuntaRepository.CommitTransaction();
        }

        public void Delete(int idContaConjunta, int idUsuario)
        {
            ContaConjuntaRepository.OpenTransaction();
            var contaConjunta = ContaConjuntaRepository.Get(null, null, idContaConjunta).First();
            var conta = ContaFinanceiraRepository.Get(contaConjunta.IdConta);
            var usuarioConvidado = UsuarioRepository.Get(contaConjunta.IdUsuarioConvidado);

            string msg;

            if (idUsuario == usuarioConvidado.Id)
            {
                msg = $"Desvinculou-se da conta conjunta {conta.Nome.ToUpper()}";
                NotificacaoService.Post(idUsuario, contaConjunta.IdConta, 10, msg);
            }
            else
            {
                NotificacaoRepository.Post(new NotificacaoDto
                {

                    IdTipo = 10, // Cancelamento de compartilhamento de conta conjunta
                    IdUsuarioEnvio = idUsuario,
                    IdUsuarioDestino = usuarioConvidado.Id,
                    Mensagem = $"Excluiu o seu compartilhamento da conta {conta.Nome.ToUpper()}",
                    ParametrosUrl = null // Não implementado nessa versão do sistema
                });

                msg = $"Excluiu o compartilhamento da conta {conta.Nome.ToUpper()} com o {usuarioConvidado.Nome}";
                NotificacaoService.Post(idUsuario, contaConjunta.IdConta, 10, msg, new List<int> { usuarioConvidado.Id });
            }

            ContaConjuntaRepository.Delete(idContaConjunta);
            ContaConjuntaRepository.CommitTransaction();

        }
    }
}
