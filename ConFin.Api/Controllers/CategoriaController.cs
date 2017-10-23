using ConFin.Common.Domain;
using ConFin.Common.Domain.Dto;
using ConFin.Domain.LancamentoCategoria;
using System;
using System.Net;
using System.Web.Http;

namespace ConFin.Api.Controllers
{
    [RoutePrefix("usuarios/{idUsuario}")]
    public class CategoriaController : ApiController
    {
        #region Design de APIs RESTful
        /*
         *  01 - URI
         *  02 - Resources
         *  03 - Operações (Verbos)
         *  04 - Versionamento
         *  05 - Media Types
         *  06 - Status & Error Codes
         *  07 - Filtros e paginação
         *  08 - Caching
         *  09 - Segurança
         *  10 - CallBack
         *  11 - Hipermedia
         *  12 - Documentação
         *  referencia: https://www.youtube.com/watch?v=psLrAsdHltQ
         */
        #endregion

        private readonly ILancamentoCategoriaRepository _lancamentoCategoriaRepository;
        private readonly ILancamentoCategoriaService _lancamentoCategoriaService;
        private readonly Notification _notification;

        public CategoriaController(ILancamentoCategoriaRepository lancamentoCategoriaRepository, Notification notification, ILancamentoCategoriaService lancamentoCategoriaService)
        {
            _lancamentoCategoriaRepository = lancamentoCategoriaRepository;
            _notification = notification;
            _lancamentoCategoriaService = lancamentoCategoriaService;
        }

        [HttpGet]
        [Route("categorias")]
        public IHttpActionResult GetAll(int idUsuario) => Ok(_lancamentoCategoriaRepository.GetAll(idUsuario));

        [HttpGet]
        [Route("categorias/{idCategoria}")]
        public IHttpActionResult Get(int idCategoria, int? idUsuario = null) => Ok(_lancamentoCategoriaRepository.Get(idCategoria, idUsuario));

        [HttpGet]
        [Route("contas{idConta}/categorias")]
        public IHttpActionResult GetCategorias(int idUsuario, int idConta) => Ok(_lancamentoCategoriaService.GetCategorias(idUsuario, idConta));

        [HttpPost]
        [Route("categorias")]
        public IHttpActionResult Post(LancamentoCategoriaDto categoria)
        {
            _lancamentoCategoriaService.Post(categoria);
            if (!_notification.Any)
                return Created(new Uri(new Parameters().UriApi + "usuarios/" + categoria.IdUsuarioCadastro + "/categorias/" + categoria.Id), categoria);

            return Content(HttpStatusCode.BadRequest, _notification.Get);
        }

        [HttpPut]
        [Route("categorias/{idCategoria}")]
        public IHttpActionResult Put(int idUsuario, int idCategoria, LancamentoCategoriaDto categoria)
        {
            _lancamentoCategoriaService.Put(categoria);
            if (!_notification.Any)
                return Ok();

            return Content(HttpStatusCode.BadRequest, _notification.Get);
        }

        [HttpDelete]
        [Route("categorias/{idCategoria}")]
        public IHttpActionResult Delete(int idUsuario, int idCategoria)
        {
            _lancamentoCategoriaService.Delete(idUsuario, idCategoria);
            if (!_notification.Any)
                return Ok();

            return Content(HttpStatusCode.BadRequest, _notification.Get);
        }

    }
}