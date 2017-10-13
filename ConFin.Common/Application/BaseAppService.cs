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
        private readonly string _baseUri;
        private string _uri;
        private HttpClient _client;
        private readonly JsonMediaTypeFormatter _jsonMediaTypeFormatter;

        protected BaseAppService(string controllerApi)
        {
            var parameters = new Parameters();
            _baseUri = _uri  = $"{parameters.UriApi}{controllerApi}";
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _jsonMediaTypeFormatter = new JsonMediaTypeFormatter
            {
                SerializerSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Include,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ContractResolver = new DefaultContractResolver { IgnoreSerializableAttribute = false }
                }
            };
        }

        protected HttpResponseMessage GetRequest(string action, object parametros = null)
        {
            ConfigureRequest(action, parametros);
            return _client.GetAsync(_client.BaseAddress).Result;
        }

        protected HttpResponseMessage PostRequest(string action, object objeto = null, object parametros = null)
        {
            ConfigureRequest(action, parametros);
            return _client.PostAsync(_uri, objeto, _jsonMediaTypeFormatter).Result;
        }

        protected HttpResponseMessage PutRequest(string action, object objeto = null, object parametros = null)
        {
            ConfigureRequest(action, parametros);
            return _client.PutAsync(_uri, objeto, _jsonMediaTypeFormatter).Result;
        }

        protected HttpResponseMessage DeleteRequest(string action, object parametros = null)
        {
            ConfigureRequest(action, parametros);
            return _client.DeleteAsync(_client.BaseAddress).Result;
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

        private void SetInstanceClient()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private void ConfigureRequest(string action, object parametros = null)
        {
            SetInstanceClient();
            _uri = $"{_baseUri}/{action}";
            AddParametersUri(parametros);
            _client.BaseAddress = new Uri($"{_uri}");
        }

    }
}
