﻿using ConFin.Application.AppService.ContaFinanceira;
using ConFin.Application.AppService.Lancamento;
using ConFin.Application.AppService.LancamentoCategoria;
using ConFin.Common.Domain.Dto;
using ConFin.Common.Web;
using ConFin.Web.ViewModel.Home;
using ConFin.Web.ViewModel.Lancamento;
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
        private readonly ILancamentoCategoriaAppService _lancamentoCategoriaAppService;
        private readonly IContaFinanceiraAppService _contaFinanceiraAppService;

        public LancamentoController(ILancamentoAppService lancamentoAppService, ILancamentoCategoriaAppService lancamentoCategoriaAppService, IContaFinanceiraAppService contaFinanceiraAppService)
        {
            _lancamentoAppService = lancamentoAppService;
            _lancamentoCategoriaAppService = lancamentoCategoriaAppService;
            _contaFinanceiraAppService = contaFinanceiraAppService;
        }

        public ActionResult Lancamento(int? idConta = null, int? idCategoria = null)
        {
            try
            {
                #region combo conta
                var responseConta = _contaFinanceiraAppService.GetAll(UsuarioLogado.Id);
                if (!responseConta.IsSuccessStatusCode)
                    return Error(responseConta);

                var contas = JsonConvert.DeserializeObject<IEnumerable<ContaFinanceiraDto>>(responseConta.Content.ReadAsStringAsync().Result).ToList();
                if (!contas.Any())
                    return Error("Antes de visualizar lançamentos é necessario cadastrar uma conta");
                #endregion

                #region combo categoria
                var responseCategoria = _lancamentoCategoriaAppService.Get(UsuarioLogado.Id);
                if (!responseCategoria.IsSuccessStatusCode)
                    return Error(responseCategoria);

                var categorias = JsonConvert.DeserializeObject<IEnumerable<LancamentoCategoriaDto>>(responseCategoria.Content.ReadAsStringAsync().Result).ToList();
                if (!categorias.Any())
                    return Error("Antes de visualizar lançamentos é necessario cadastrar categorias");
                #endregion

                var response = _lancamentoAppService.GetAll(UsuarioLogado.Id, idConta, idCategoria);
                if (!response.IsSuccessStatusCode)
                    return Error(response);

                var lancamentos = JsonConvert.DeserializeObject<IEnumerable<LancamentoDto>>(response.Content.ReadAsStringAsync().Result)
                    .Select(x => new LancamentoViewModel(x)).ToList();

                return View("Lancamento", new LancamentoMasterViewModel(lancamentos, idConta, idCategoria)
                {
                    Contas = contas,
                    Categorias = categorias
                });
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult GetModalCadastroEdicao(int? idLancamento, string indicadorReceitaDespesa)
        {
            try
            {
                #region combo conta
                var responseConta = _contaFinanceiraAppService.GetAll(UsuarioLogado.Id);
                if (!responseConta.IsSuccessStatusCode)
                    return Error(responseConta);

                var contas = JsonConvert.DeserializeObject<IEnumerable<ContaFinanceiraDto>>(responseConta.Content.ReadAsStringAsync().Result).ToList();
                if(!contas.Any())
                    return Error("Não foi encontrada nenhuma Conta para cadastrar o lançamento");
                #endregion

                #region combo categoria
                var responseCategoria = _lancamentoCategoriaAppService.Get(UsuarioLogado.Id);
                if (!responseCategoria.IsSuccessStatusCode)
                    return Error(responseCategoria);

                var categorias = JsonConvert.DeserializeObject<IEnumerable<LancamentoCategoriaDto>>(responseCategoria.Content.ReadAsStringAsync().Result).ToList();
                if (!categorias.Any())
                    return Error("Não foi encontrada nenhuma Categoria para cadastrar o lançamento");
                #endregion

                // cadastro
                if (!idLancamento.HasValue)
                {
                    ViewBag.IndicadorCadastro = "S";

                    return View("_ModalCadastroEdicaoLancamento", new LancamentoViewModel
                    {
                        IndicadorReceitaDespesa = indicadorReceitaDespesa,
                        ContasFinanceira = contas,
                        Categorias = categorias
                    });
                }

                // alteração
                ViewBag.IndicadorCadastro = "N";

                var response = _lancamentoAppService.Get((int)idLancamento);
                if (!response.IsSuccessStatusCode)
                    return Error(response);

                var lancamentoDto = JsonConvert.DeserializeObject<LancamentoDto>(response.Content.ReadAsStringAsync().Result);
                return View("_ModalCadastroEdicaoLancamento", new LancamentoViewModel(lancamentoDto)
                {
                    ContasFinanceira = contas,
                    Categorias = categorias
                });
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

        public ActionResult PutIndicadorPagoRecebido(LancamentoDto lancamento)
        {
            try
            {
                lancamento.IdUsuarioUltimaAlteracao = UsuarioLogado.Id;
                var response = _lancamentoAppService.PutIndicadorPagoRecebido(lancamento);
                return response.IsSuccessStatusCode ? Ok() : Error(response);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult GetResumoLancamento(int? idConta = null, int? idCategoria = null)
        {
            try
            {
                var responseResumoLancamento = _lancamentoAppService.GetResumo(UsuarioLogado.Id, idConta, idCategoria);
                if (!responseResumoLancamento.IsSuccessStatusCode)
                    return Error(responseResumoLancamento);

                var resumoLancamentoDto = JsonConvert.DeserializeObject<LancamentoResumoGeralDto>(responseResumoLancamento.Content.ReadAsStringAsync().Result);
                return Json(new LancamentoResumoGeralViewModel(resumoLancamentoDto), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
    }
}
