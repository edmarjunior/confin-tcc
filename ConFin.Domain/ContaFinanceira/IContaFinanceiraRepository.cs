using ConFin.Common.Domain.Dto;
using System.Collections.Generic;

namespace ConFin.Domain.ContaFinanceira
{
    public interface IContaFinanceiraRepository
    {
        IEnumerable<ContaFinanceiraDto> GetAll(int idUsuario);
        ContaFinanceiraDto Get(int idConta);
        void Post(ContaFinanceiraDto conta);
        void Put(ContaFinanceiraDto conta);
        void Delete(int idUsuario, int idConta);
        bool PossuiVinculos(int idConta);
    }
}
