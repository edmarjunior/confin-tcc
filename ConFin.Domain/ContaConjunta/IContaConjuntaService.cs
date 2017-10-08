namespace ConFin.Domain.ContaConjunta
{
    public interface IContaConjuntaService
    {
        void Post(int idConta, int idUsuarioEnvio, string emailUsuarioConvidado);
    }
}
