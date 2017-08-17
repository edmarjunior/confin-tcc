using ConFin.Application.AppService.LancamentoCategoria;
using ConFin.Common.Domain.Dto;
using ConFin.Common.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ConFin.Web.Controllers
{
    public class LancamentoCategoriaController: BaseController
    {
        private readonly ILancamentoCategoriaAppService _lancamentoCategoriaAppService;

        public LancamentoCategoriaController(ILancamentoCategoriaAppService lancamentoCategoriaAppService)
        {
            _lancamentoCategoriaAppService = lancamentoCategoriaAppService;
        }

        public ActionResult LancamentoCategoria()
        {
            try
            {
                var response = _lancamentoCategoriaAppService.Get(UsuarioLogado.Id);
                if (!response.IsSuccessStatusCode)
                    return Error(response);

                var categorias = JsonConvert.DeserializeObject<IEnumerable<LancamentoCategoriaDto>>(response.Content.ReadAsStringAsync().Result);
                return View("LancamentoCategoria", categorias);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult GetModalCadastroEdicao(int? idCategoria)
        {
            try
            {
                // cadastro
                if (!idCategoria.HasValue)
                {
                    ViewBag.IndicadorCadastro = "S";
                    return View("_ModalCadastroEdicaoLancamentoCategoria", new LancamentoCategoriaDto());
                }

                // alteração
                ViewBag.IndicadorCadastro = "N";

                var response = _lancamentoCategoriaAppService.Get(UsuarioLogado.Id, (int) idCategoria);
                if (!response.IsSuccessStatusCode)
                    return Error(response);

                var categoria = JsonConvert.DeserializeObject<LancamentoCategoriaDto>(response.Content.ReadAsStringAsync().Result);
                return View("_ModalCadastroEdicaoLancamentoCategoria", categoria);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult Post(LancamentoCategoriaDto categoria)
        {
            try
            {
                categoria.IdUsuarioCadastro = UsuarioLogado.Id;
                var response = _lancamentoCategoriaAppService.Post(categoria);
                return response.IsSuccessStatusCode ? Ok("Categoria cadastrada com sucesso") : Error(response);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult Put(LancamentoCategoriaDto categoria)
        {
            try
            {
                categoria.IdUsuarioUltimaAlteracao = UsuarioLogado.Id;
                var response = _lancamentoCategoriaAppService.Put(categoria);
                return response.IsSuccessStatusCode ? Ok("Categoria editada com sucesso") : Error(response);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult Delete(int idCategoria)
        {
            try
            {
                var response = _lancamentoCategoriaAppService.Delete(UsuarioLogado.Id, idCategoria);
                return response.IsSuccessStatusCode ? Ok("Categoria excluida com sucesso") : Error(response);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }   
    }
}
