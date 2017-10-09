using ConFin.Common.Domain.Dto;
using System.Net.Http;

namespace ConFin.Application.AppService.LancamentoCategoria
{
    public interface ILancamentoCategoriaAppService
    {
        HttpResponseMessage Get(int idUsuario);
        HttpResponseMessage Get(int idUsuario, int idCategoria);
        HttpResponseMessage GetCategorias(int idUsuario, int idConta);
        HttpResponseMessage Post(LancamentoCategoriaDto categoria);
        HttpResponseMessage Put(LancamentoCategoriaDto categoria);
        HttpResponseMessage Delete(int idUsuario, int idCategoria);
    }
}
