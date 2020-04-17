using ConFin.Common.Repository;
using ConFin.Common.Repository.Infra;
using ConFin.Domain.AcessoOpcaoMenu;

namespace ConFin.Repository
{
    public class AcessoOpcaoMenuRepository: BaseRepository, IAcessoOpcaoMenuRepository
    {
        public AcessoOpcaoMenuRepository(IDatabaseConnection connection) : base(connection)
        {
        }

        private enum Procedures
        {
            SP_InsAcessoOpcaoMenu,
            FNC_PegaQuantidadeAcessoOpcaoMenu
        }

        public void Post(int idUsuario, int codigoOpcaoMenu)
        {
            ExecuteProcedure(Procedures.SP_InsAcessoOpcaoMenu);
            AddParameter("IdUsuario", idUsuario);
            AddParameter("CodigoOpcao", codigoOpcaoMenu);
            ExecuteNonQuery();
        }

        public int GetTotalAcessos(int codigoOpcaoMenu, int? idUsuario = null)
        {
            ExecuteProcedure(Procedures.FNC_PegaQuantidadeAcessoOpcaoMenu);
            AddParameter("CodigoOpcao", codigoOpcaoMenu);
            AddParameter("IdUsuario", idUsuario);
            return ExecuteNonQueryWithReturn();
        }
    }
}
