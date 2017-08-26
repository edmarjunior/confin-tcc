using ConFin.Common.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Routing;

namespace ConFin.Common.Application
{
    public class BaseAppService
    {
        private string _uri;
        private readonly HttpClient _client;

        protected BaseAppService(string controllerApi)
        {
            var parameters = new Parameters();
            _uri = $"{parameters.UriApi}{controllerApi}";
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        protected HttpResponseMessage GetRequest(object parametros = null)
        {
            AddParametersUri(parametros);
            _client.BaseAddress = new Uri($"{_uri}");
            return _client.GetAsync(_client.BaseAddress).Result;
        }

        protected HttpResponseMessage GetRequest(string action, object parametros = null)
        {
            _uri += $"/{action}";
            return GetRequest(parametros);
        }

        protected HttpResponseMessage PostRequest(object objeto = null, object parametros = null)
        {
            AddParametersUri(parametros);
            _client.BaseAddress = new Uri($"{_uri}");

            return _client.PostAsync(_uri, objeto, new JsonMediaTypeFormatter
            {
                SerializerSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Include,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ContractResolver = new DefaultContractResolver { IgnoreSerializableAttribute = false }
                }
            }).Result;
        }

        protected HttpResponseMessage PostRequest(string action, object objeto = null, object parametros = null)
        {
            _uri += $"/{action}";
            return PostRequest(objeto, parametros);
        }

        protected HttpResponseMessage PutRequest(object objeto = null, object parametros = null)
        {
            AddParametersUri(parametros);
            _client.BaseAddress = new Uri($"{_uri}");

            return _client.PutAsync(_uri, objeto, new JsonMediaTypeFormatter
            {
                SerializerSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Include,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ContractResolver = new DefaultContractResolver { IgnoreSerializableAttribute = false }
                }
            }).Result;
        }

        protected HttpResponseMessage PutRequest(string action, object objeto = null, object parametros = null)
        {
            _uri += $"/{action}";
            return PutRequest(objeto, parametros);
        }

        protected HttpResponseMessage DeleteRequest(object parametros = null)
        {
            AddParametersUri(parametros);
            _client.BaseAddress = new Uri($"{_uri}");
            return _client.DeleteAsync(_client.BaseAddress).Result;
        }

        protected HttpResponseMessage DeleteRequest(string action, object parametros = null)
        {
            _uri += $"/{action}";
            return DeleteRequest(parametros);
        }

        private void AddParametersUri(object parametros)
        {
            if (parametros == null)
                return;

            var param = new StringBuilder();
            param.Append(_uri);
            param.Append("?");

            foreach (var parametro in new RouteValueDictionary(parametros))
            {
                param.Append(parametro.Key);
                param.Append("=");
                if (parametro.Value != null)
                {
                    var d = parametro.Value as decimal?;
                    var dt = parametro.Value as DateTime?;
                    if (d != null)
                        param.Append(Regex.Replace(d.Value.ToString(CultureInfo.InvariantCulture), @"(?<=\d)\,(?=\d)", ".", RegexOptions.Compiled));
                    else if (dt != null)
                        param.Append(string.Format("{0:yyyy-MM-dd}%20{0:HH}%3A{0:mm}%3A{0:ss}", dt));
                    else
                        param.Append(parametro.Value);
                }
                param.Append("&");
            }

            _uri = param.ToString();
        }
    }
}
