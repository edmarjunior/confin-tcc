using ConFin.Application.AppService.ContaConjunta;
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
    public class ContaConjuntaController: BaseController
    {
        private readonly IContaConjuntaAppService _contaConjuntaAppService;

        public ContaConjuntaController(IContaConjuntaAppService contaConjuntaAppService)
        {
            _contaConjuntaAppService = contaConjuntaAppService;
        }

        public ActionResult ContaConjunta()
        {
            try
            {
                var response = _contaConjuntaAppService.Get(UsuarioLogado.Id);
                if (!response.IsSuccessStatusCode)
                    return Error(response);

                var contasConjunta = JsonConvert.DeserializeObject<IEnumerable<ContaConjuntaDto>>(response.Content.ReadAsStringAsync().Result);
                return View("ContaConjunta", contasConjunta.Select(x => new ContaConjuntaViewModel(x)));
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult Put(int idContaConjunta, string indicadorAprovado)
        {
            try
            {
                var response = _contaConjuntaAppService.Put(idContaConjunta, indicadorAprovado);
                return response.IsSuccessStatusCode
                    ? Ok($"Convite {(indicadorAprovado == "A" ? "Aprovado" : "Recusado")} com sucesso")
                    : Error(response);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }



        // métodos utilizados na tela de Contas Financeira

        public ActionResult GetModalContaConjunta(int idConta)
        {
            try
            {
                ViewBag.IdConta = idConta;
                return View("_ModalContaConjunta");

            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult GetGridUsuarios(int idConta)
        {
            try
            {
                var response = _contaConjuntaAppService.Get(null, idConta);
                if (!response.IsSuccessStatusCode)
                    return Error(response);

                var usuariosContaConjunta = JsonConvert.DeserializeObject<IEnumerable<ContaConjuntaDto>>(response.Content.ReadAsStringAsync().Result);
                ViewBag.IdConta = idConta;
                return View("_GridUsuariosContaConjunta", usuariosContaConjunta.Select(x => new ContaConjuntaViewModel(x)));

            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult Post(int idConta, string emailUsuarioConvidado)
        {
            try
            {
                var response = _contaConjuntaAppService.Post(idConta, UsuarioLogado.Id, emailUsuarioConvidado);
                return response.IsSuccessStatusCode
                    ? Ok("Convite enviado com sucesso! Assim que o usuário aceitar, a conta será compartilhada")
                    : Error(response);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult Delete(int idContaConjunta)
        {
            try
            {
                var response = _contaConjuntaAppService.Delete(idContaConjunta);
                return response.IsSuccessStatusCode
                    ? Ok("Compartilhamento excluído com sucesso!")
                    : Error(response);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

    }
}
