using ConFin.Common.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Mvc;

namespace ConFin.Web.Controllers
{
    public class LoginController: BaseHomeController
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
            {
                UsuarioLogado = usuario;
                return RedirectToAction("Home", "Home");

            }

            ViewBag.Error = "E-mail ou Senha inserido não corresponde a nenhuma conta";
            return View("Login");
        }

        public ActionResult CadastrarConta(Usuario usuario)
        {
            var client = new HttpClient();

            var response = client.PostAsync("http://localhost:5002/api/Usuario", usuario, new JsonMediaTypeFormatter
            {
                SerializerSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Include,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ContractResolver = new DefaultContractResolver
                    {
                        IgnoreSerializableAttribute = false
                    }
                }
            }).Result;


            if (response.IsSuccessStatusCode)
            {
                Response.StatusCode = (int)HttpStatusCode.OK;
                Response.TrySkipIisCustomErrors = true;
                return Content("cadastro realizado com sucesso !!!");
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            //Response.TrySkipIisCustomErrors = true;
            return Content(response.Content.ReadAsStringAsync().Result);



        }
    }
}