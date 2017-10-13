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
            return GetRequest("Get", new {idUsuario});
        }

        public HttpResponseMessage Get(int idUsuario, int idCategoria)
        {
            return GetRequest("Get", new { idUsuario, idCategoria });

        }

        public HttpResponseMessage GetCategorias(int idUsuario, int idConta)
        {
            return GetRequest("GetCategorias", new { idUsuario, idConta });
        }

        public HttpResponseMessage Post(LancamentoCategoriaDto categoria)
        {
            return PostRequest("Post", categoria);
        }

        public HttpResponseMessage Put(LancamentoCategoriaDto categoria)
        {
            return PutRequest("Put", categoria);
        }

        public HttpResponseMessage Delete(int idUsuario, int idCategoria)
        {
            return DeleteRequest("Delete", new { idUsuario, idCategoria });

        }
    }
}
