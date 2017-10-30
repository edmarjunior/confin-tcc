using ConFin.Application.AppService.LancamentoCategoria;
using ConFin.Application.AppService.Transferencia;
using ConFin.Common.Domain.Dto;
using ConFin.Common.Web;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ConFin.Web.Controllers
{
    [OutputCache(Duration = 0)]
    public class LancamentoCategoriaController: BaseController
    {
        private readonly ILancamentoCategoriaAppService _lancamentoCategoriaAppService;

        public LancamentoCategoriaController(ILancamentoCategoriaAppService lancamentoCategoriaAppService, ITransferenciaAppService transferenciaAppService)
        {
            _lancamentoCategoriaAppService = lancamentoCategoriaAppService;
        }

        public ActionResult LancamentoCategoria()
        {
            var response = _lancamentoCategoriaAppService.GetAll(UsuarioLogado.Id);
            if (!response.IsSuccessStatusCode)
                return Error(response);

            var categorias = JsonConvert.DeserializeObject<IEnumerable<LancamentoCategoriaDto>>(response.Content.ReadAsStringAsync().Result);
            return View("LancamentoCategoria", categorias);
        }

        public ActionResult GetModalCadastroEdicao(int? idCategoria)
        {
            if (!idCategoria.HasValue)
            {
                ViewBag.IndicadorCadastro = "S";
                return View("_ModalCadastroEdicaoLancamentoCategoria", new LancamentoCategoriaDto());
            }

            ViewBag.IndicadorCadastro = "N";

            var response = _lancamentoCategoriaAppService.Get((int)idCategoria);
            if (!response.IsSuccessStatusCode)
                return Error(response);

            var categoria = JsonConvert.DeserializeObject<LancamentoCategoriaDto>(response.Content.ReadAsStringAsync().Result);
            return View("_ModalCadastroEdicaoLancamentoCategoria", categoria);
        }

        public ActionResult Post(LancamentoCategoriaDto categoria)
        {
            categoria.IdUsuarioCadastro = UsuarioLogado.Id;
            var response = _lancamentoCategoriaAppService.Post(categoria);
            return response.IsSuccessStatusCode ? Ok("Categoria cadastrada com sucesso") : Error(response);
        }

        public ActionResult Put(LancamentoCategoriaDto categoria)
        {
            categoria.IdUsuarioUltimaAlteracao = UsuarioLogado.Id;
            var response = _lancamentoCategoriaAppService.Put(categoria);
            return response.IsSuccessStatusCode ? Ok("Categoria editada com sucesso") : Error(response);
        }

        public ActionResult Delete(int idCategoria)
        {
            var response = _lancamentoCategoriaAppService.Delete(UsuarioLogado.Id, idCategoria);
            return response.IsSuccessStatusCode ? Ok("Categoria excluida com sucesso") : Error(response);
        }

        public ActionResult GetCategorias(int idConta)
        {
            var response = _lancamentoCategoriaAppService.GetCategorias(UsuarioLogado.Id, idConta);
            return !response.IsSuccessStatusCode 
                ? Error(response) 
                : Json(JsonConvert.DeserializeObject<IEnumerable<LancamentoCategoriaDto>>(response.Content.ReadAsStringAsync().Result), JsonRequestBehavior.AllowGet);
        }
    }
}
