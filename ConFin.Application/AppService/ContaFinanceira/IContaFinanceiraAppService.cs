using ConFin.Common.Domain.Dto;
using System.Net.Http;

namespace ConFin.Application.AppService.ContaFinanceira
{
    public interface IContaFinanceiraAppService
    {
        HttpResponseMessage GetAll(int idUsuario);
        HttpResponseMessage Get(int idConta);
        HttpResponseMessage Post(ContaFinanceiraDto conta);
        HttpResponseMessage Put(ContaFinanceiraDto conta);
        HttpResponseMessage Delete(int idUsuario, int idConta);
    }
}
