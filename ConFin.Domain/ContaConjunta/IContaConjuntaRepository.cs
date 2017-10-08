using ConFin.Common.Domain.Dto;
using ConFin.Common.Repository.Infra;
using System.Collections.Generic;

namespace ConFin.Domain.ContaConjunta
{
    public interface IContaConjuntaRepository : IBaseRepository
    {
        IEnumerable<ContaConjuntaDto> Get(int? idUsuario, int? idConta = null);
        void Post(int idConta, int idUsuarioEnvio, int idUsuarioConvidado);
        void Delete(int idContaConjunta);
        void Put(int idContaConjunta, string indicadorAprovado);
    }
}
