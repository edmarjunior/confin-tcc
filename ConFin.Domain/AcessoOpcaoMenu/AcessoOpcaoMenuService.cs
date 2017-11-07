namespace ConFin.Domain.AcessoOpcaoMenu
{
    public class AcessoOpcaoMenuService: IAcessoOpcaoMenuService
    {
        private readonly IAcessoOpcaoMenuRepository _acessoOpcaoMenuRepository;

        public AcessoOpcaoMenuService(IAcessoOpcaoMenuRepository acessoOpcaoMenuRepository)
        {
            _acessoOpcaoMenuRepository = acessoOpcaoMenuRepository;
        }

        public int Post(int idUsuario, int codigoOpcaoMenu)
        {
            var totAcessos = _acessoOpcaoMenuRepository.GetTotalAcessos(codigoOpcaoMenu, idUsuario);
            _acessoOpcaoMenuRepository.Post(idUsuario, codigoOpcaoMenu);
            return totAcessos;
        }
    }
}
