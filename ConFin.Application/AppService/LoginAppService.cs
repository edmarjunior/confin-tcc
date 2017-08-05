using ConFin.Application.Interfaces;
using ConFin.Common.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace ConFin.Application.AppService
{
    public class LoginAppService : ILoginAppService
    {
        public HttpResponseMessage Get(string email, string senha)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri($"http://localhost:5002/api/Login?email={email}&senha={senha}")
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client.GetAsync(client.BaseAddress).Result;

        }

        public HttpResponseMessage Post(Usuario usuario)
        {
            var client = new HttpClient();

            return client.PostAsync("http://localhost:5002/api/Login", usuario, new JsonMediaTypeFormatter
            {
                SerializerSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Include,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ContractResolver = new DefaultContractResolver { IgnoreSerializableAttribute = false }
                }
            }).Result;
        }

        public HttpResponseMessage PutConfirmacaoCadastro(int idUsuario)
        {
            var client = new HttpClient();

            return client.PutAsync($"http://localhost:5002/api/Login/PutConfirmacaoCadastro?idUsuario={idUsuario}", "", new JsonMediaTypeFormatter
            {
                SerializerSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Include,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ContractResolver = new DefaultContractResolver { IgnoreSerializableAttribute = false }
                }
            }).Result;

        }
    }
}
