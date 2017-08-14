using ConFin.Application.AppService.ContaFinanceira;
using ConFin.Application.AppService.ContaFinanceiraTipo;
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
    public class ContaFinanceiraController: BaseController
    {
        private readonly IContaFinanceiraAppService _contaFinanceiraAppService;
        private readonly IContaFinanceiraTipoAppService _contaFinanceiraTipoAppService;

        public ContaFinanceiraController(IContaFinanceiraAppService contaFinanceiraAppService, IContaFinanceiraTipoAppService contaFinanceiraTipoAppService)
        {
            _contaFinanceiraAppService = contaFinanceiraAppService;
            _contaFinanceiraTipoAppService = contaFinanceiraTipoAppService;
        }

        public ActionResult ContaFinanceira()
        {
            try
            {
                var response = _contaFinanceiraAppService.GetAll(UsuarioLogado.Id);
                if (!response.IsSuccessStatusCode)
                    return Error(response);

                var contas = JsonConvert.DeserializeObject<IEnumerable<ContaFinanceiraDto>>(response.Content.ReadAsStringAsync().Result);
                return View("ContaFinanceira", contas.Select(x => new ContaFinanceiraViewModel(x)));
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult GetModalCadastroEdicao(int? idConta)
        {
            try
            {
                var tiposContaResponse = _contaFinanceiraTipoAppService.Get();
                if (!tiposContaResponse.IsSuccessStatusCode)
                    return Error(tiposContaResponse);

                var tiposConta = JsonConvert.DeserializeObject<IEnumerable<ContaFinanceiraTipoDto>>(tiposContaResponse.Content.ReadAsStringAsync().Result);
                
                // cadastro
                if (!idConta.HasValue)
                {
                    ViewBag.IndicadorCadastro = "S";
                    return View("_ModalCadastroEdicao", new ContaFinanceiraViewModel(tiposConta));
                }

                // alteração
                ViewBag.IndicadorCadastro = "N";

                var response = _contaFinanceiraAppService.Get((int)idConta);
                if (!response.IsSuccessStatusCode)
                    return Error(response);

                var conta = JsonConvert.DeserializeObject<ContaFinanceiraDto>(response.Content.ReadAsStringAsync().Result);
                return View("_ModalCadastroEdicao", new ContaFinanceiraViewModel(conta, tiposConta));
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult Post(ContaFinanceiraDto conta)
        {
            try
            {
                conta.IdUsuarioCadastro = UsuarioLogado.Id;
                var response = _contaFinanceiraAppService.Post(conta);
                return response.IsSuccessStatusCode ? Ok("Conta cadastrada com sucesso") : Error(response);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult Put(ContaFinanceiraDto conta)
        {
            try
            {
                conta.IdUsuarioUltimaAlteracao = UsuarioLogado.Id;
                var response = _contaFinanceiraAppService.Put(conta);
                return response.IsSuccessStatusCode ? Ok("Conta editada com sucesso") : Error(response);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult Delete(int idConta)
        {
            try
            {
                var response = _contaFinanceiraAppService.Delete(UsuarioLogado.Id, idConta);
                return response.IsSuccessStatusCode ? Ok("Conta excluida com sucesso") : Error(response);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
    }
}
