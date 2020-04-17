using ConFin.Common.Domain.Dto;
using System.Collections.Generic;

namespace ConFin.Domain.Compromisso
{
    public interface ICompromissoRepository
    {
        int Post(CompromissoDto compromisso);
        void Delete(int id);

        CompromissoDto GetCompromissoLancamento(int idLancamento);
        IEnumerable<CompromissoLancamentoDto> GetCompromissoLancamentos(int idCompromisso);
        int PostCompromissoLancamento(int idCompromisso, int idLancamento, int numeroLancamento);
        void DeleteCompromissoLancamento(int idLancamento);
    }
}
