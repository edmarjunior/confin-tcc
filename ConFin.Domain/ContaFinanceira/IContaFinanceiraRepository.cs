using ConFin.Common.Domain.Dto;
using ConFin.Common.Repository.Infra;
using System.Collections.Generic;

namespace ConFin.Domain.ContaFinanceira
{
    public interface IContaFinanceiraRepository: IBaseRepository
    {
        IEnumerable<ContaFinanceiraDto> GetAll(int idUsuario);
        ContaFinanceiraDto Get(int idConta, int? idUsuario = null);
        void Post(ContaFinanceiraDto conta);
        void Put(ContaFinanceiraDto conta);
        void Delete(int idUsuario, int idConta);
        bool PossuiVinculos(int idConta);
    }
}
