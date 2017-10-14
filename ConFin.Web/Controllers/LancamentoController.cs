using ConFin.Application.AppService.ContaFinanceira;
using ConFin.Application.AppService.Lancamento;
using ConFin.Application.AppService.LancamentoCategoria;
using ConFin.Application.AppService.Transferencia;
using ConFin.Common.Domain.Dto;
using ConFin.Common.Web;
using ConFin.Web.ViewModel;
using ConFin.Web.ViewModel.Home;
using ConFin.Web.ViewModel.Lancamento;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConFin.Web.Controllers
{
    public class LancamentoController: BaseController
    {
        private readonly ILancamentoAppService _lancamentoAppService;
        private readonly ILancamentoCategoriaAppService _lancamentoCategoriaAppService;
        private readonly IContaFinanceiraAppService _contaFinanceiraAppService;
        private readonly ITransferenciaAppService _transferenciaAppService;


        public LancamentoController(ILancamentoAppService lancamentoAppService, ILancamentoCategoriaAppService lancamentoCategoriaAppService, IContaFinanceiraAppService contaFinanceiraAppService, ITransferenciaAppService transferenciaAppService)
        {
            _lancamentoAppService = lancamentoAppService;
            _lancamentoCategoriaAppService = lancamentoCategoriaAppService;
            _contaFinanceiraAppService = contaFinanceiraAppService;
            _transferenciaAppService = transferenciaAppService;
        }

        public ActionResult Lancamento(int? idConta = null, int? idCategoria = null, byte? mes = null, short? ano = null)
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
                var responseCategoria = _lancamentoCategoriaAppService.GetAll(UsuarioLogado.Id);
                if (!responseCategoria.IsSuccessStatusCode)
                    return Error(responseCategoria);

                var categorias = JsonConvert.DeserializeObject<IEnumerable<LancamentoCategoriaDto>>(responseCategoria.Content.ReadAsStringAsync().Result).ToList();
                if (!categorias.Any())
                    return Error("Antes de visualizar lançamentos é necessario cadastrar categorias");
                #endregion

                mes = (byte?)(mes ?? DateTime.Today.Month);
                ano = (short?)(ano ?? DateTime.Today.Year);

                var response = _lancamentoAppService.GetAll(UsuarioLogado.Id, (byte)mes, (short)ano, idConta, idCategoria);
                if (!response.IsSuccessStatusCode)
                    return Error(response);

                var lancamentos = JsonConvert.DeserializeObject<IEnumerable<LancamentoDto>>(response.Content.ReadAsStringAsync().Result)
                    .Select(x => new LancamentoViewModel(x) { UsuarioPodeManipularLancamento = UsuarioLogado.Id == x.IdUsuarioCadastro}).ToList();

                var responsePossuiOpcaoTransferencia = _transferenciaAppService.GetVerificaClientePossuiTransferenciaHabilitada(UsuarioLogado.Id);
                ViewBag.PossuiOpcaoTransferencia = responsePossuiOpcaoTransferencia.IsSuccessStatusCode;

                return View("Lancamento", new LancamentoMasterViewModel(lancamentos, (byte)mes, (short)ano, idConta, idCategoria)
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
                var responseCategoria = _lancamentoCategoriaAppService.GetAll(UsuarioLogado.Id);
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

        public ActionResult Delete(int idLancamento, string indTipoDelete)
        {
            try
            {
                var response = _lancamentoAppService.Delete(idLancamento, indTipoDelete);
                return response.IsSuccessStatusCode ? Ok(response.Content.ReadAsStringAsync().Result.Replace('"', ' ')) : Error(response);
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

        public ActionResult GetResumoLancamento(int? idConta = null, int? idCategoria = null, byte? mes = null, short? ano = null)
        {
            try
            {
                mes = (byte?) (mes ?? DateTime.Today.Month);
                ano = (short?) (ano ?? DateTime.Today.Year);

                var responseResumoLancamento = _lancamentoAppService.GetResumo(UsuarioLogado.Id, (byte)mes, (short)ano, idConta, idCategoria);
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

        public ActionResult GetPeriodos()
        {
            try
            {
                var response = _lancamentoAppService.GetPeriodo();
                if (!response.IsSuccessStatusCode)
                    return Error(response);

                var periodos = JsonConvert.DeserializeObject<IEnumerable<PeriodoDto>>(response.Content.ReadAsStringAsync().Result).ToList();
                return !periodos.Any()
                    ? Error("Falha ao buscar periodos para lançamentos fixo/parcelado.")
                    : Json(periodos, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult GetModalImportacao()
        {
            try
            {
                var response = _contaFinanceiraAppService.GetAll(UsuarioLogado.Id);
                if (!response.IsSuccessStatusCode)
                    return Error(response);

                var contas = JsonConvert.DeserializeObject<IEnumerable<ContaFinanceiraDto>>(response.Content.ReadAsStringAsync().Result).ToList();
                return !contas.Any() 
                    ? Error("Não foi encontrada nenhuma Conta") 
                    : View("_ModalImportacaoLancamento", contas.Select(x => new ContaFinanceiraViewModel(x)));
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult PostImportarLancamentos(HttpPostedFileBase arquivoLancamentos, int idConta)
        {
            try
            {
                var lancamentos = new List<LancamentoDto>();
                const string msgTitulo = "Erro ao fazer upload EXCEL";
                const string msgErro = "EXCEL não contém registros";

                using (var excelPackage = new ExcelPackage(arquivoLancamentos.InputStream))
                {
                    var wb = excelPackage.Workbook;
                    if (wb == null || wb.Worksheets.Count == 0 || wb.Worksheets.First().Cells[2, 1].Value == null)
                        return Error($"{msgTitulo} : {msgErro}");

                    var wc = wb.Worksheets.First().Cells;
                    var totLinhas = wc.Rows;
                    for (var line = 2; line <= totLinhas; line++)
                    {
                        if (string.IsNullOrEmpty(wc[line, 1].Text) || wc[line, 1].Text == "#N/A")
                            break;

                        var valor = Convert.ToDecimal(wc[line, 4].Text);

                        lancamentos.Add(new LancamentoDto
                        {
                            Data = Convert.ToDateTime(wc[line, 1].Text),
                            Descricao = wc[line, 2].Text,
                            NomeCategoria = wc[line, 3].Text,
                            Valor = valor < 0 ? valor * -1 : valor,
                            IndicadorPagoRecebido = wc[line, 5].Text == "Pago" ? "S" : "N",
                            IndicadorReceitaDespesa = valor < 0 ? "D" : "R",
                            IdUsuarioCadastro = UsuarioLogado.Id,
                            IdConta = idConta
                        });
                    };

                }

                if (!lancamentos.Any())
                    return Error($"{msgTitulo} : {msgErro}");

                var response = _lancamentoAppService.Post(lancamentos);
                return response.IsSuccessStatusCode ? Ok("Importação realizada com sucesso, os lançamentos foram cadastrados") : Error(response);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult GetArquivoModeloImportacao()
        {
            try
            {
                using (var excelPackage = new ExcelPackage())
                {
                    var wb = excelPackage.Workbook;
                    var ws = wb.Worksheets.Add("Lancamentos");

                    // cabeçalho
                    ws.Cells[1,1].Value = "Data";
                    ws.Cells[1,2].Value = "Descrição";
                    ws.Cells[1,3].Value = "Categoria";
                    ws.Cells[1,4].Value = "Valor";

                    //exemplos de preenchimento de despesa
                    ws.Cells[2, 1].Value = $"{DateTime.Today:dd-MM-yyyy}";
                    ws.Cells[2, 2].Value = "Camiseta Polo";
                    ws.Cells[2, 3].Value = "Roupas";
                    ws.Cells[2, 4].Value = "-100,90";

                    //exemplos de preenchimento de receita
                    ws.Cells[3, 1].Value = $"{DateTime.Today:dd-MM-yyyy}";
                    ws.Cells[3, 2].Value = "Comissão";
                    ws.Cells[3, 3].Value = "Salário";
                    ws.Cells[3, 4].Value = "500";

                    ws.Cells[ws.Dimension.Address].AutoFitColumns();

                    return File(new MemoryStream(excelPackage.GetAsByteArray()), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"Modelo Lançamentos {DateTime.Now:yyMMddHHmmss}.xlsx");
                }

            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
    }
}
