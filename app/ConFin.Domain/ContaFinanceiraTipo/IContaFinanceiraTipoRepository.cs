using ConFin.Common.Domain.Dto;
using System.Collections.Generic;

namespace ConFin.Domain.ContaFinanceiraTipo
{
    public interface IContaFinanceiraTipoRepository
    {
        IEnumerable<ContaFinanceiraTipoDto> Get();
    }
}
