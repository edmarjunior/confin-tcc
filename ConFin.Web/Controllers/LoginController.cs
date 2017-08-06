using ConFin.Application.Interfaces;
using ConFin.Common.Domain;
using ConFin.Common.Web;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace ConFin.Web.Controllers
{
    public class LoginController : BaseHomeController
    {
        private readonly ILoginAppService _loginAppService;

        public LoginController(ILoginAppService loginAppService)
        {
            _loginAppService = loginAppService;
        }

        public ActionResult Login()
        {
            return View("Login");
        }

        public ActionResult Get(string email, string senha)
        {
            var response = _loginAppService.Get(email, senha);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("Erro", response.Content.ReadAsStringAsync().Result.Replace('[', ' ').Replace(']', ' ').Replace('"', ' '));
                return View("Login");
            }

            UsuarioLogado = JsonConvert.DeserializeObject<Usuario>(response.Content.ReadAsStringAsync().Result);
            return RedirectToAction("Home", "Home");
        }

        public ActionResult GetConfirmacao(int idUsuario)
        {

            var response = _loginAppService.PutConfirmacaoCadastro(idUsuario);
            if (!response.IsSuccessStatusCode)
                return View("Error", response.Content.ReadAsStringAsync().Result);

            return RedirectToAction("Home", "Home");
        }

        public ActionResult Post(Usuario usuario)
        {
            var response = _loginAppService.Post(usuario);
            return response.IsSuccessStatusCode ? Ok() : Error(response);
        }

        public ActionResult Logout()
        {
            UsuarioLogado = null;
            return RedirectToAction("Home", "Home");
        }
    }
}
