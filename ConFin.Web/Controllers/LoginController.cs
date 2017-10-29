using ConFin.Application.AppService.Login;
using ConFin.Application.AppService.Usuario;
using ConFin.Common.Domain.Dto;
using ConFin.Common.Web;
using ConFin.Web.ViewModel;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace ConFin.Web.Controllers
{
    public class LoginController : BaseHomeController
    {
        private readonly ILoginAppService _loginAppService;
        private readonly IUsuarioAppService _usuarioAppService;

        public LoginController(ILoginAppService loginAppService, IUsuarioAppService usuarioAppService)
        {
            _loginAppService = loginAppService;
            _usuarioAppService = usuarioAppService;
        }

        public ActionResult Login()
        {
            return View("Login");
        }

        public ActionResult Get(UsuarioViewModel usuario)
        {
            var response = _loginAppService.Get(usuario.Email, usuario.Senha);
            if (!response.IsSuccessStatusCode)
                return Error(response);

            UsuarioLogado = JsonConvert.DeserializeObject<UsuarioDto>(response.Content.ReadAsStringAsync().Result);
            return Ok("Login realizado com sucesso.");
        }

        public ActionResult RedirectToHome()
        {
            return RedirectToAction("Home", "Home");
        }

        public ActionResult Post(UsuarioDto usuario)
        {
            var response = _loginAppService.Post(usuario);
            return !response.IsSuccessStatusCode 
                ? Error(response) 
                : Ok("Solicitação realizada com sucesso, foi enviado um e-mail para a confirmação do cadastro.");
        }

        public ActionResult GetConfirmacao(int idUsuario)
        {
            var response = _loginAppService.PutConfirmacaoCadastro(idUsuario);
            return !response.IsSuccessStatusCode 
                ? (ActionResult) View("Error", model: response.Content.ReadAsStringAsync().Result)
                : RedirectToAction("Home", "Home");
        }

        public ActionResult PostReenviarSenha(string email)
        {
            var response = _loginAppService.PostReenviarSenha(email);
            return response.IsSuccessStatusCode ? Ok("") : Error(response);
        }

        [HttpGet]
        public ActionResult RedefinirSenha(int idUsuario, string token)
        {
            var response = _loginAppService.GetVerificaTokenValidoRedefinirSenha(idUsuario, token);
            if(!response.IsSuccessStatusCode)
                return View("Error", model: response.Content.ReadAsStringAsync().Result);

            response = _usuarioAppService.Get(idUsuario);
            if (!response.IsSuccessStatusCode)
                return View("Error", model: response.Content.ReadAsStringAsync().Result);


            var usuario = JsonConvert.DeserializeObject<UsuarioDto>(response.Content.ReadAsStringAsync().Result);
            ViewBag.Token = token;
            return View("RedefinirSenha", usuario);
        }

        public ActionResult PostRedefinirSenha(UsuarioDto usuario, string token)
        {
            var response = _usuarioAppService.PutSenha(usuario.Id, token, usuario.Senha);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("Erro", response.Content.ReadAsStringAsync().Result.Replace('[', ' ').Replace(']', ' ').Replace('"', ' '));
                ViewBag.Token = token;
                return View("RedefinirSenha", usuario);
            }

            UsuarioLogado = usuario;
            return RedirectToAction("Home", "Home");
        }

    }
}
