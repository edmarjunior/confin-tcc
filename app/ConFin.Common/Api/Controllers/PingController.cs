using System;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace ConFin.Common.Api.Controllers
{
    public class PingController : ApiController
    {
        public string Get() => $"API {Regex.Replace(Request.RequestUri.AbsolutePath.ToUpper().TrimStart('/'), @"\/?API\/?", string.Empty)} | " +
                               $"Everything is fine! :) - {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
    }
}
