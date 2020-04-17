using ConFin.Common.Domain.Dto;

namespace ConFin.Domain.Transferencia
{
    public interface ITransferenciaService
    {
        void Post(TransferenciaDto transferencia);
        void Put(TransferenciaDto transferencia);
        void Delete(int idTransferencia);
    }
}
