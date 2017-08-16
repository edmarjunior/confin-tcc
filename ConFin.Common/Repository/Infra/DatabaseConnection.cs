using System;
using System.Data;
using System.Data.SqlClient;

namespace ConFin.Common.Repository.Infra
{
    public class DatabaseConnection : IDatabaseConnection, IDisposable
    {
        public DatabaseConnection()
        {
            SqlConnection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=ConFin;Integrated Security=True"); /*Casa*/
            //SqlConnection = new SqlConnection("Data Source=EDMAR-PC;Initial Catalog=ConFin;Integrated Security=True"); /*Empresa*/
        }

        public SqlConnection SqlConnection { get; }
        public SqlTransaction SqlTransaction { get; set; }

        public void OpenTransaction()
        {
            if (SqlConnection.State == ConnectionState.Broken)
            {
                SqlConnection.Close();
                SqlConnection.Open();
            }

            if (SqlConnection.State == ConnectionState.Closed)
                SqlConnection.Open();

            SqlTransaction = SqlConnection.BeginTransaction();
        }

        public void Commit()
        {
            SqlTransaction.Commit();
            SqlTransaction.Dispose();
        }

        public void Rollback()
        {
            SqlTransaction.Rollback();
            SqlTransaction.Dispose();
        }

        public void Dispose()
        {
            SqlConnection.Close();
        }
    }
}
