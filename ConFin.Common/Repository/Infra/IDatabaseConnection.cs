using System.Data.SqlClient;

namespace ConFin.Common.Repository.Infra
{
    public interface IDatabaseConnection
    {
        SqlConnection SqlConnection { get; }
        SqlTransaction SqlTransaction { get; }
        void OpenTransaction();
        void Commit();
        void Rollback();
        void Dispose();
    }
}
