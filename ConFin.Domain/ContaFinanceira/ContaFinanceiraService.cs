using ConFin.Common.Domain;

namespace ConFin.Domain.ContaFinanceira
{
    public class ContaFinanceiraService: IContaFinanceiraService
    {
        private readonly IContaFinanceiraRepository _contaFinanceiraRepository;
        private readonly Notification _notification;

        public ContaFinanceiraService(Notification notification, IContaFinanceiraRepository contaFinanceiraRepository)
        {
            _notification = notification;
            _contaFinanceiraRepository = contaFinanceiraRepository;
        }
    }
}
