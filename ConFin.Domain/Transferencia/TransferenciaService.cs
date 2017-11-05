using ConFin.Common.Domain;
using ConFin.Common.Domain.Dto;
using ConFin.Domain.ContaFinanceira;
using ConFin.Domain.Notificacao;

namespace ConFin.Domain.Transferencia
{
    public class TransferenciaService: ITransferenciaService
    {
        private readonly ITransferenciaRepository _transferenciaRepository;
        private readonly IContaFinanceiraRepository _contaFinanceiraRepository;
        private readonly INotificacaoService _notificacaoService;
        private readonly Notification _notification;

        public TransferenciaService(ITransferenciaRepository transferenciaRepository, Notification notification, 
            INotificacaoService notificacaoService, IContaFinanceiraRepository contaFinanceiraRepository)
        {
            _transferenciaRepository = transferenciaRepository;
            _notification = notification;
            _notificacaoService = notificacaoService;
            _contaFinanceiraRepository = contaFinanceiraRepository;
        }

        public void Post(TransferenciaDto transferencia)
        {
            var contaOrigem = _contaFinanceiraRepository.Get(transferencia.IdContaOrigem);
            var contaDestino = _contaFinanceiraRepository.Get(transferencia.IdContaDestino);
            var msg = $"Transferiu ({transferencia.Descricao} o valor de R$ {transferencia.Valor:##.###,##}) da conta {contaOrigem.Nome.ToUpper()} para a conta {contaDestino.Nome.ToUpper()}";

            _transferenciaRepository.OpenTransaction();
            _notificacaoService.Post(transferencia.IdUsuarioCadastro, contaOrigem.Id, 7, msg); // 7: Cadastro de transferência em conta conjunta
            _notificacaoService.Post(transferencia.IdUsuarioCadastro, contaDestino.Id, 7, msg);
            _transferenciaRepository.Post(transferencia);
            _transferenciaRepository.CommitTransaction();
        }

        public void Put(TransferenciaDto transferencia)
        {
            var contaOrigem = _contaFinanceiraRepository.Get(transferencia.IdContaOrigem);
            var contaDestino = _contaFinanceiraRepository.Get(transferencia.IdContaDestino);
            var msg = $"Editou a transferência ({transferencia.Descricao} no valor de R$ {transferencia.Valor:N}) da conta {contaOrigem.Nome.ToUpper()} para a conta {contaDestino.Nome.ToUpper()}";

            if (transferencia.IdUsuarioUltimaAlteracao == null)
            {
                _notification.Add("O Id do usuário que esta realizando a alteração não foi recebido, favor entrar em contato com o suporte");
                return;
            }

            _transferenciaRepository.OpenTransaction();
            _notificacaoService.Post((int)transferencia.IdUsuarioUltimaAlteracao, contaOrigem.Id, 8, msg); // 8: Edição de transferência em conta conjunta
            _notificacaoService.Post((int)transferencia.IdUsuarioUltimaAlteracao, contaDestino.Id, 8, msg); 
            _transferenciaRepository.Put(transferencia);
            _transferenciaRepository.CommitTransaction();
        }

        public void Delete(int idTransferencia)
        {
            var transferencia = _transferenciaRepository.Get(idTransferencia);
            if (transferencia == null)
            {
                _notification.Add("Transferência não encontrada.");
                return;
            }

            var contaOrigem = _contaFinanceiraRepository.Get(transferencia.IdContaOrigem);
            var contaDestino = _contaFinanceiraRepository.Get(transferencia.IdContaDestino);
            var msg = $"Excluiu a transferência ({transferencia.Descricao} do valor de R$ {transferencia.Valor:##.###,##}) das contas {contaOrigem.Nome.ToUpper()} / {contaDestino.Nome.ToUpper()}";

            _transferenciaRepository.OpenTransaction();
            _notificacaoService.Post(transferencia.IdUsuarioCadastro, contaOrigem.Id, 9, msg); // 9: Remoção de transferência em conta conjunta
            _notificacaoService.Post(transferencia.IdUsuarioCadastro, contaDestino.Id, 9, msg);
            _transferenciaRepository.Delete(idTransferencia);
            _transferenciaRepository.CommitTransaction();
        }
    }
}
