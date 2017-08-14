using ConFin.Application.AppService.Lancamento;
using ConFin.Common.Domain.Dto;
using ConFin.Common.Web;
using ConFin.Web.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ConFin.Web.Controllers
{
    public class LancamentoController: BaseController
    {
        private readonly ILancamentoAppService _lancamentoAppService;

        public LancamentoController(ILancamentoAppService lancamentoAppService)
        {
            _lancamentoAppService = lancamentoAppService;
        }

        public ActionResult Lancamento()
        {
            try
            {
                var response = _lancamentoAppService.GetAll(UsuarioLogado.Id);
                if (!response.IsSuccessStatusCode)
                    return Error(response);

                var lancamentos = JsonConvert.DeserializeObject<IEnumerable<LancamentoDto>>(response.Content.ReadAsStringAsync().Result);
                return View("Lancamento", lancamentos.Select(x => new LancamentoViewModel(x)));
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult GetModalCadastroEdicao(int? lancamento)
        {
            try
            {
                // cadastro
                if (!lancamento.HasValue)
                {
                    ViewBag.IndicadorCadastro = "S";
                    return View("_ModalCadastroEdicaoLancamento", new LancamentoViewModel());
                }

                // alteração
                ViewBag.IndicadorCadastro = "N";

                var response = _lancamentoAppService.Get((int)lancamento);
                if (!response.IsSuccessStatusCode)
                    return Error(response);

                var lancamentoDto = JsonConvert.DeserializeObject<LancamentoDto>(response.Content.ReadAsStringAsync().Result);
                return View("_ModalCadastroEdicaoLancamento", new LancamentoViewModel(lancamentoDto));
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult Post(LancamentoDto lancamento)
        {
            try
            {
                lancamento.IdUsuarioCadastro = UsuarioLogado.Id;
                var response = _lancamentoAppService.Post(lancamento);
                return response.IsSuccessStatusCode ? Ok("Lançamento cadastrado com sucesso") : Error(response);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult Put(LancamentoDto lancamento)
        {
            try
            {
                lancamento.IdUsuarioUltimaAlteracao = UsuarioLogado.Id;
                var response = _lancamentoAppService.Put(lancamento);
                return response.IsSuccessStatusCode ? Ok("Lançamento editado com sucesso") : Error(response);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult Delete(int idLancamento)
        {
            try
            {
                var response = _lancamentoAppService.Delete(idLancamento);
                return response.IsSuccessStatusCode ? Ok("Lançamento excluido com sucesso") : Error(response);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
    }
}
