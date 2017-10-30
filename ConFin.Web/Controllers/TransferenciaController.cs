using ConFin.Application.AppService.ContaFinanceira;
using ConFin.Application.AppService.LancamentoCategoria;
using ConFin.Application.AppService.Transferencia;
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
    public class TransferenciaController : BaseController
    {
        private readonly ITransferenciaAppService _transferenciaAppService;
        private readonly ILancamentoCategoriaAppService _lancamentoCategoriaAppService;
        private readonly IContaFinanceiraAppService _contaFinanceiraAppService;


        public TransferenciaController(ITransferenciaAppService transferenciaAppService,
            IContaFinanceiraAppService contaFinanceiraAppService, ILancamentoCategoriaAppService lancamentoCategoriaAppService)
        {
            _transferenciaAppService = transferenciaAppService;
            _contaFinanceiraAppService = contaFinanceiraAppService;
            _lancamentoCategoriaAppService = lancamentoCategoriaAppService;
        }

        public ActionResult Transferencia()
        {
            var response = _transferenciaAppService.GetAll(UsuarioLogado.Id);
            if (!response.IsSuccessStatusCode)
                return Error(response);

            var transferencias = JsonConvert.DeserializeObject<IEnumerable<TransferenciaDto>>(response.Content.ReadAsStringAsync().Result);
            return View("Transferencia", transferencias.Select(x => new TransferenciaViewModel(x) { UsuarioPodeEditarTransferencia = x.IdUsuarioCadastro == UsuarioLogado.Id }));
        }

        public ActionResult GetModalCadastroEdicao(int? idTransferencia)
        {
            // combo de Contas
            var responseContas = _contaFinanceiraAppService.GetAll(UsuarioLogado.Id);
            if (!responseContas.IsSuccessStatusCode)
                return Error(responseContas);

            var contas = JsonConvert.DeserializeObject<IEnumerable<ContaFinanceiraDto>>(responseContas.Content.ReadAsStringAsync().Result).ToList();
            if (!contas.Any())
                return Error("Não foi encontrada nenhuma Conta para cadastrar a transferência");

            // combo de Categorias de lançamento
            var responseCategorias = _lancamentoCategoriaAppService.GetAll(UsuarioLogado.Id);
            if (!responseCategorias.IsSuccessStatusCode)
                return Error(responseCategorias);

            var categorias = JsonConvert.DeserializeObject<IEnumerable<LancamentoCategoriaDto>>(responseCategorias.Content.ReadAsStringAsync().Result).ToList();
            if (!categorias.Any())
                return Error("Não foi encontrada nenhuma Categoria para cadastrar a transferência");

            // cadastro
            if (!idTransferencia.HasValue)
            {
                ViewBag.IndicadorCadastro = "S";

                return View("_ModalCadastroEdicaoTransferencia", new TransferenciaViewModel
                {
                    ContasFinanceira = contas,
                    Categorias = categorias,
                    IndicadorCadastro = "S"
                });
            }

            // alteração
            ViewBag.IndicadorCadastro = "N";

            var response = _transferenciaAppService.Get((int)idTransferencia);
            if (!response.IsSuccessStatusCode)
                return Error(response);

            var transferencia = JsonConvert.DeserializeObject<TransferenciaDto>(response.Content.ReadAsStringAsync().Result);

            // buscando conta de Origem (caso não esteja no combo de contas)
            if (contas.All(x => x.Id != transferencia.IdContaOrigem))
            {
                var responseContaOrigem = _contaFinanceiraAppService.Get(transferencia.IdContaOrigem, UsuarioLogado.Id);
                if (!responseContaOrigem.IsSuccessStatusCode)
                    return Error(responseContaOrigem);

                contas.Add(JsonConvert.DeserializeObject<ContaFinanceiraDto>(responseContaOrigem.Content.ReadAsStringAsync().Result));
            }

            // buscando conta de Destino (caso não esteja no combo de contas)
            if (contas.All(x => x.Id != transferencia.IdContaDestino))
            {
                var responseContaDestino = _contaFinanceiraAppService.Get(transferencia.IdContaDestino, UsuarioLogado.Id);
                if (!responseContaDestino.IsSuccessStatusCode)
                    return Error(responseContaDestino);

                contas.Add(JsonConvert.DeserializeObject<ContaFinanceiraDto>(responseContaDestino.Content.ReadAsStringAsync().Result));
            }

            // buscando categoria da conta (caso não esteja no combo de categorias)
            if (categorias.All(x => x.Id != transferencia.IdCategoria))
            {
                var responseCategoria = _lancamentoCategoriaAppService.Get(transferencia.IdCategoria, UsuarioLogado.Id);
                if (!responseCategoria.IsSuccessStatusCode)
                    return Error(responseCategoria);

                categorias.Add(JsonConvert.DeserializeObject<LancamentoCategoriaDto>(responseCategoria.Content.ReadAsStringAsync().Result));
            }

            return View("_ModalCadastroEdicaoTransferencia", new TransferenciaViewModel(transferencia)
            {
                UsuarioPodeEditarTransferencia = transferencia.IdUsuarioCadastro == UsuarioLogado.Id,
                IndicadorCadastro = "N",
                ContasFinanceira = contas,
                Categorias = categorias
            });
        }

        public ActionResult Post(TransferenciaDto transferencia)
        {
            transferencia.IdUsuarioCadastro = UsuarioLogado.Id;
            var response = _transferenciaAppService.Post(transferencia);
            return response.IsSuccessStatusCode ? Ok("Transferência cadastrada com sucesso") : Error(response);
        }

        public ActionResult Put(TransferenciaDto transferencia)
        {
            transferencia.IdUsuarioUltimaAlteracao = UsuarioLogado.Id;
            var response = _transferenciaAppService.Put(transferencia);
            return response.IsSuccessStatusCode ? Ok("Transferência editada com sucesso") : Error(response);
        }

        public ActionResult Delete(int idTransferencia)
        {
            var response = _transferenciaAppService.Delete(idTransferencia);
            return response.IsSuccessStatusCode ? Ok("Transferência excluida com sucesso") : Error(response);
        }

        public ActionResult PutIndicadorPagoRecebido(TransferenciaDto transferencia)
        {
            transferencia.IdUsuarioUltimaAlteracao = UsuarioLogado.Id;
            var response = _transferenciaAppService.PutIndicadorPagoRecebido(transferencia);
            return response.IsSuccessStatusCode ? Ok() : Error(response);
        }

        public ActionResult PossuiOpcaoTransferencia()
        {
            var responsePossuiOpcaoTransferencia = _transferenciaAppService.GetVerificaClientePossuiTransferenciaHabilitada(UsuarioLogado.Id);
            return responsePossuiOpcaoTransferencia.IsSuccessStatusCode ? Ok("1") : Ok("0");
        }

    }
}
