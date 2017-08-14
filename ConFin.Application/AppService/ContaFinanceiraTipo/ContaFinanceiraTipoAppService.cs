using ConFin.Common.Application;
using System.Net.Http;

namespace ConFin.Application.AppService.ContaFinanceiraTipo
{
    public class ContaFinanceiraTipoAppService : BaseAppService, IContaFinanceiraTipoAppService
    {
        public ContaFinanceiraTipoAppService() : base("ContaFinanceiraTipo")
        {
        }

        public HttpResponseMessage Get()
        {
            return GetRequest();
        }
    }
}
