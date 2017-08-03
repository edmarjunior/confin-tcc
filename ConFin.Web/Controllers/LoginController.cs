using ConFin.Web.Model;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;

namespace ConFin.Web.Controllers
{
    public class LoginController: Controller
    {
        public ActionResult Login()
        {
            return View("Login");
        }

        public ActionResult PostLogin(string email, string senha)
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri($"http://localhost:5002/api/Usuario?email={email}&senha={senha}");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.GetAsync(client.BaseAddress).Result;

            var result = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = result;
                return View("Login");
            }

            var usuario = JsonConvert.DeserializeObject<Usuario>(result);

            if(usuario != null)
                return RedirectToAction("Home", "Home", usuario);

            ViewBag.Error = "E-mail ou Senha inserido não corresponde a nenhuma conta";
            return View("Login");

        }
    }
}