using ConFin.Common.Repository.Infra;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ConFin.Common.Repository
{
    public class BaseRepository
    {
        protected readonly IDatabaseConnection Connection;
        protected SqlCommand SqlCommand { get; set; }

        public BaseRepository(IDatabaseConnection connection)
        {
            Connection = connection;
        }

        public void OpenTransaction() => Connection.OpenTransaction();
        public void RollbackTransaction() => Connection.Rollback();
        public void CommitTransaction() => Connection.Commit();

        protected void OpenConnection()
        {
            try
            {
                if (SqlCommand.Connection.State == ConnectionState.Broken)
                {
                    SqlCommand.Connection.Close();
                    SqlCommand.Connection.Open();
                }

                if (SqlCommand.Connection.State == ConnectionState.Closed)
                    SqlCommand.Connection.Open();
            }
            catch (SqlException ex)
            {
                throw ex.Number == 53 ? new Exception("Falha ao efetuar conexão com o Banco de Dados") : ex;
            }
        }

        public void CloseConnection() => Connection.SqlConnection.Close();

        protected void ExecuteProcedure(object procedureName)
        {
            SqlCommand = new SqlCommand(procedureName.ToString(), Connection.SqlConnection, Connection.SqlTransaction)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 99999,
            };
        }

        protected void AddParameter(string parameterName, object parameterValue)
        {
            SqlCommand.Parameters.AddWithValue(parameterName, parameterValue);
        }

        protected void AddParameterOutput(string parameterName, object parameterValue, DbType parameterType)
        {
            SqlCommand.Parameters.Add(new SqlParameter
            {
                ParameterName = parameterName,
                Direction = ParameterDirection.Output,
                Value = parameterValue,
                DbType = parameterType
            });
        }

        protected void AddParameterReturn(string parameterName = "@RETURN_VALUE", DbType parameterType = DbType.Int16)
        {
            SqlCommand.Parameters.Add(new SqlParameter
            {
                ParameterName = parameterName,
                Direction = ParameterDirection.ReturnValue,
                DbType = parameterType
            });
        }

        protected string GetParameterOutput(string parameter) => SqlCommand.Parameters[parameter].Value.ToString();

        protected int ExecuteNonQuery()
        {
            OpenConnection();
            return SqlCommand.ExecuteNonQuery();
        }

        protected int ExecuteNonQueryWithReturn()
        {
            AddParameterReturn();
            OpenConnection();
            SqlCommand.ExecuteNonQuery();
            return int.Parse(SqlCommand.Parameters["@RETURN_VALUE"].Value.ToString());
        }

        protected T ExecuteNonQueryWithReturn<T>()
        {
            AddParameterReturn();
            OpenConnection();
            SqlCommand.ExecuteNonQuery();
            var value = SqlCommand.Parameters["@RETURN_VALUE"].Value;
            if (value == DBNull.Value)
                return default(T);
            return (T)Convert.ChangeType(value, typeof(T));
        }

        protected IDataReader ExecuteReader()
        {
            OpenConnection();
            return SqlCommand.ExecuteReader();
        }

        protected IDataReader ExecuteReader(CommandBehavior cb)
        {
            return SqlCommand.ExecuteReader(cb);
        }
    }
}
