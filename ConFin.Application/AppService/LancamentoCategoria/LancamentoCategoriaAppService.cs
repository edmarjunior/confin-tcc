using ConFin.Common.Application;
using ConFin.Common.Domain.Dto;
using System.Net.Http;

namespace ConFin.Application.AppService.LancamentoCategoria
{
    public class LancamentoCategoriaAppService: BaseAppService, ILancamentoCategoriaAppService
    {
        public LancamentoCategoriaAppService() : base("LancamentoCategoria")
        {
        }

        public HttpResponseMessage Get(int idUsuario)
        {
            return GetRequest(new {idUsuario});
        }

        public HttpResponseMessage Get(int idUsuario, int idCategoria)
        {
            return GetRequest(new { idUsuario, idCategoria });

        }

        public HttpResponseMessage Post(LancamentoCategoriaDto categoria)
        {
            return PostRequest(categoria);
        }

        public HttpResponseMessage Put(LancamentoCategoriaDto categoria)
        {
            return PutRequest(categoria);
        }

        public HttpResponseMessage Delete(int idUsuario, int idCategoria)
        {
            return DeleteRequest(new { idUsuario, idCategoria });

        }
    }
}
