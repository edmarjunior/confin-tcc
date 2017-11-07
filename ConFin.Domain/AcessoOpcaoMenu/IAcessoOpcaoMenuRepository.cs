namespace ConFin.Domain.AcessoOpcaoMenu
{
    public interface IAcessoOpcaoMenuRepository
    {
        void Post(int idUsuario, int codigoOpcaoMenu);
        int GetTotalAcessos(int codigoOpcaoMenu, int? idUsuario = null);
    }
}
