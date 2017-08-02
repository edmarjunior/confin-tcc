namespace ConFin.Common.Repository.Infra
{
    public interface IBaseRepository
    {
        void OpenTransaction();

        void RollbackTransaction();

        void CommitTransaction();
    }
}
