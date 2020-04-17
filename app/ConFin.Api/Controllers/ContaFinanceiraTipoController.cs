using ConFin.Common.Domain;
using ConFin.Domain.ContaFinanceiraTipo;
using System.Web.Http;

namespace ConFin.Api.Controllers
{
    public class ContaFinanceiraTipoController: ApiController
    {
        private readonly IContaFinanceiraTipoRepository _contaFinanceiraTipoRepository;
        private readonly Notification _notification;

        public ContaFinanceiraTipoController(Notification notification, IContaFinanceiraTipoRepository contaFinanceiraTipoRepository)
        {
            _notification = notification;
            _contaFinanceiraTipoRepository = contaFinanceiraTipoRepository;
        }

        public IHttpActionResult Get() => Ok(_contaFinanceiraTipoRepository.Get());
    }
}