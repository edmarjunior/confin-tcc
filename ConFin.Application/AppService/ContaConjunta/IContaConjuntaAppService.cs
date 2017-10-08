using System.Net.Http;

namespace ConFin.Application.AppService.ContaConjunta
{
    public interface IContaConjuntaAppService
    {
        HttpResponseMessage Get(int? idUsuario, int? idConta = null);
        HttpResponseMessage Post(int idConta, int idUsuarioEnvio, string emailUsuarioConvidado);
        HttpResponseMessage Delete(int idContaConjunta);
        HttpResponseMessage Put(int idContaConjunta, string indicadorAprovado);
    }
}
