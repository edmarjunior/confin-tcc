using ConFin.Application.AppService.ContaConjunta;
using ConFin.Common.Domain.Dto;
using ConFin.Common.Web;
using ConFin.Web.ViewModel;
using Newtonsoft.Json;
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
            var response = _contaConjuntaAppService.Get(UsuarioLogado.Id);
            if (!response.IsSuccessStatusCode)
                return Error(response);

            var contasConjunta = JsonConvert.DeserializeObject<IEnumerable<ContaConjuntaDto>>(response.Content.ReadAsStringAsync().Result);
            return View("ContaConjunta", contasConjunta.Select(x => new ContaConjuntaViewModel(x)));
        }

        public ActionResult Put(ContaConjuntaDto contaConjunta)
        {
            contaConjunta.IdUsuarioConvidado = UsuarioLogado.Id;
            var response = _contaConjuntaAppService.Put(contaConjunta);
            return response.IsSuccessStatusCode
                ? Ok($"Convite {(contaConjunta.IndicadorAprovado == "A" ? "Aprovado" : "Recusado")} com sucesso")
                : Error(response);
        }

        public ActionResult GetModalContaConjunta(int idConta, string indicadorProprietarioConta)
        {
            ViewBag.IdConta = idConta;
            ViewBag.IndicadorProprietarioConta = indicadorProprietarioConta;
            return View("_ModalContaConjunta");
        }

        public ActionResult GetGridUsuarios(int idConta, string indicadorProprietarioConta)
        {
            var response = _contaConjuntaAppService.Get(null, idConta);
            if (!response.IsSuccessStatusCode)
                return Error(response);

            ViewBag.IdConta = idConta;
            var usuariosContaConjunta = Deserialize<IEnumerable<ContaConjuntaDto>>(response).OrderByDescending(x => x.DataAnalise).ThenByDescending(x => x.DataCadastro);
            return View("_GridUsuariosContaConjunta", usuariosContaConjunta.Select(x => new ContaConjuntaViewModel(x, indicadorProprietarioConta, UsuarioLogado.Id)));
        }

        public ActionResult Post(ContaConjuntaDto contaConjunta)
        {
            contaConjunta.IdUsuarioEnvio = UsuarioLogado.Id;
            var response = _contaConjuntaAppService.Post(contaConjunta);
            return response.IsSuccessStatusCode
                ? Ok("Convite enviado com sucesso! Assim que o usuário aceitar, a conta será compartilhada")
                : Error(response);
        }

        public ActionResult Delete(int idContaConjunta)
        {
            var response = _contaConjuntaAppService.Delete(idContaConjunta, UsuarioLogado.Id);
            return response.IsSuccessStatusCode
                ? Ok("Compartilhamento excluído com sucesso!")
                : Error(response);
        }
    }
}
