using ConFin.Common.Domain.Dto;

namespace ConFin.Domain.ContaConjunta
{
    public interface IContaConjuntaService
    {
        void Post(ContaConjuntaDto contaConjunta);
        void Put(ContaConjuntaDto contaConjunta);
    }
}
