using ConFin.Common.Domain;
using System.Text;
using System.Web.Mvc;

namespace ConFin.Common.Web.Extension
{
    public static class UrlExtension
    {
        public static string AbsoluteAction(this UrlHelper h, string controller, string action)
        {
            var sb = new StringBuilder();

            sb.Append(new Parameters().UriWeb);
            sb.Append(controller);
            sb.Append("/");
            sb.Append(action);

            return sb.ToString();
        }

    }
}
