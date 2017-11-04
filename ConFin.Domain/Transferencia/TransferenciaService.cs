using ConFin.Common.Domain;
using ConFin.Common.Domain.Dto;
using ConFin.Domain.ContaFinanceira;
using ConFin.Domain.Notificacao;

namespace ConFin.Domain.Transferencia
{
    public class TransferenciaService: ITransferenciaService
    {
        public readonly ITransferenciaRepository TransferenciaRepository;
        public readonly IContaFinanceiraRepository ContaFinanceiraRepository;
        public readonly INotificacaoService NotificacaoService;
        public readonly Notification Notification;

        public TransferenciaService(ITransferenciaRepository transferenciaRepository, Notification notification, 
            INotificacaoService notificacaoService, IContaFinanceiraRepository contaFinanceiraRepository)
        {
            TransferenciaRepository = transferenciaRepository;
            Notification = notification;
            NotificacaoService = notificacaoService;
            ContaFinanceiraRepository = contaFinanceiraRepository;
        }

        public void Post(TransferenciaDto transferencia)
        {
            var contaOrigem = ContaFinanceiraRepository.Get(transferencia.IdContaOrigem);
            var contaDestino = ContaFinanceiraRepository.Get(transferencia.IdContaDestino);
            var msg = $"Transferiu ({transferencia.Descricao} o valor de R$ {transferencia.Valor:##.###,##}) da conta {contaOrigem.Nome.ToUpper()} para a conta {contaDestino.Nome.ToUpper()}";

            TransferenciaRepository.OpenTransaction();
            NotificacaoService.Post(transferencia.IdUsuarioCadastro, contaOrigem.Id, 7, msg); // 7: Cadastro de transferência em conta conjunta
            NotificacaoService.Post(transferencia.IdUsuarioCadastro, contaDestino.Id, 7, msg);
            TransferenciaRepository.Post(transferencia);
            TransferenciaRepository.CommitTransaction();
        }

        public void Put(TransferenciaDto transferencia)
        {
            var contaOrigem = ContaFinanceiraRepository.Get(transferencia.IdContaOrigem);
            var contaDestino = ContaFinanceiraRepository.Get(transferencia.IdContaDestino);
            var msg = $"Editou a transferência ({transferencia.Descricao} no valor de R$ {transferencia.Valor:N}) da conta {contaOrigem.Nome.ToUpper()} para a conta {contaDestino.Nome.ToUpper()}";

            if (transferencia.IdUsuarioUltimaAlteracao == null)
            {
                Notification.Add("O Id do usuário que esta realizando a alteração não foi recebido, favor entrar em contato com o suporte");
                return;
            }

            TransferenciaRepository.OpenTransaction();
            NotificacaoService.Post((int)transferencia.IdUsuarioUltimaAlteracao, contaOrigem.Id, 8, msg); // 8: Edição de transferência em conta conjunta
            NotificacaoService.Post((int)transferencia.IdUsuarioUltimaAlteracao, contaDestino.Id, 8, msg); 
            TransferenciaRepository.Put(transferencia);
            TransferenciaRepository.CommitTransaction();
        }

        public void Delete(int idTransferencia)
        {
            var transferencia = TransferenciaRepository.Get(idTransferencia);
            if (transferencia == null)
            {
                Notification.Add("Transferência não encontrada.");
                return;
            }

            var contaOrigem = ContaFinanceiraRepository.Get(transferencia.IdContaOrigem);
            var contaDestino = ContaFinanceiraRepository.Get(transferencia.IdContaDestino);
            var msg = $"Excluiu a transferência ({transferencia.Descricao} do valor de R$ {transferencia.Valor:##.###,##}) das contas {contaOrigem.Nome.ToUpper()} / {contaDestino.Nome.ToUpper()}";

            TransferenciaRepository.OpenTransaction();
            NotificacaoService.Post(transferencia.IdUsuarioCadastro, contaOrigem.Id, 9, msg); // 9: Remoção de transferência em conta conjunta
            NotificacaoService.Post(transferencia.IdUsuarioCadastro, contaDestino.Id, 9, msg);
            TransferenciaRepository.Delete(idTransferencia);
            TransferenciaRepository.CommitTransaction();
        }
    }
}
