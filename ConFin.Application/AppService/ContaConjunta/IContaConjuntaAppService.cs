using ConFin.Common.Domain.Dto;
using System.Net.Http;

namespace ConFin.Application.AppService.ContaConjunta
{
    public interface IContaConjuntaAppService
    {
        HttpResponseMessage Get(int? idUsuario, int? idConta = null);
        HttpResponseMessage Post(ContaConjuntaDto contaConjunta);
        HttpResponseMessage Delete(int idContaConjunta);
        HttpResponseMessage Put(ContaConjuntaDto contaConjunta);
    }
}
