using ConFin.Application.AppService.AcessoOpcaoMenu;
using ConFin.Application.AppService.ContaFinanceira;
using ConFin.Application.AppService.ContaFinanceiraTipo;
using ConFin.Common.Domain.Dto;
using ConFin.Common.Web;
using ConFin.Web.ViewModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ConFin.Web.Controllers
{
    [OutputCache(Duration = 0)]
    public class ContaFinanceiraController: BaseController
    {
        private readonly IContaFinanceiraAppService _contaFinanceiraAppService;
        private readonly IContaFinanceiraTipoAppService _contaFinanceiraTipoAppService;
        private readonly IAcessoOpcaoMenuAppService _acessoOpcaoMenuAppService;

        public ContaFinanceiraController(IContaFinanceiraAppService contaFinanceiraAppService, IContaFinanceiraTipoAppService contaFinanceiraTipoAppService, IAcessoOpcaoMenuAppService acessoOpcaoMenuAppService)
        {
            _contaFinanceiraAppService = contaFinanceiraAppService;
            _contaFinanceiraTipoAppService = contaFinanceiraTipoAppService;
            _acessoOpcaoMenuAppService = acessoOpcaoMenuAppService;
        }

        public ActionResult ContaFinanceira()
        {
            var response = _contaFinanceiraAppService.GetAll(UsuarioLogado.Id);
            if (!response.IsSuccessStatusCode)
                return Error(response);

            var contas = JsonConvert.DeserializeObject<IEnumerable<ContaFinanceiraDto>>(response.Content.ReadAsStringAsync().Result);

            var responseOpcMenu = _acessoOpcaoMenuAppService.Post(UsuarioLogado.Id, 2); // 2: codigo opção menu "ContaFinanceira"
            if (!responseOpcMenu.IsSuccessStatusCode)
                return Error(responseOpcMenu);

            ViewBag.PrimeiroAcesso = Deserialize<int>(responseOpcMenu) < 1 ? "S" : "N";
            return View("ContaFinanceira", contas.Select(x => new ContaFinanceiraViewModel(x)));
        }

        public ActionResult GetModalCadastroEdicao(int? idConta)
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

        public ActionResult Post(ContaFinanceiraDto conta)
        {
            conta.IdUsuarioCadastro = UsuarioLogado.Id;
            var response = _contaFinanceiraAppService.Post(conta);
            return response.IsSuccessStatusCode ? Ok("Conta cadastrada com sucesso") : Error(response);
        }

        public ActionResult Put(ContaFinanceiraDto conta)
        {
            conta.IdUsuarioUltimaAlteracao = UsuarioLogado.Id;
            var response = _contaFinanceiraAppService.Put(conta);
            return response.IsSuccessStatusCode ? Ok("Conta editada com sucesso") : Error(response);
        }

        public ActionResult Delete(int idConta)
        {
            var response = _contaFinanceiraAppService.Delete(UsuarioLogado.Id, idConta);
            return response.IsSuccessStatusCode ? Ok("Conta excluida com sucesso") : Error(response);
        }

        public ActionResult GetFilter(int idConta)
        {
            var response = _contaFinanceiraAppService.GetAll(UsuarioLogado.Id);
            if (!response.IsSuccessStatusCode)
                return Error(response);

            var contas = JsonConvert.DeserializeObject<IEnumerable<ContaFinanceiraDto>>(response.Content.ReadAsStringAsync().Result).ToList();
            return !contas.Any() 
                ? Error("A transferência não poderá ser realizada, você possui somente uma conta.") 
                : Json(contas.Where(x => x.Id != idConta), JsonRequestBehavior.AllowGet);
        }
    }
}
