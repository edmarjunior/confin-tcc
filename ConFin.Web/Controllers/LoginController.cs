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

            if (response.IsSuccessStatusCode)
            {
                var usuario = JsonConvert.DeserializeObject<Usuario>(response.Content.ReadAsStringAsync().Result);
                //return View("Home/Home", usuario);
                return RedirectToAction("Home", "Home", usuario);
            }

            return View("Login");
        }
    }
}