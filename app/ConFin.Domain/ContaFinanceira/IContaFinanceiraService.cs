using ConFin.Common.Domain.Dto;

namespace ConFin.Domain.ContaFinanceira
{
    public interface IContaFinanceiraService
    {
        void Post(ContaFinanceiraDto conta);
        void Put(ContaFinanceiraDto conta);
        void Delete(int idUsuario, int idConta);
    }
}
