using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace DAT.SqlHelper
{
    public static class SqlHelperBase
    {

        #region property

        private static string ConnectionStr
        {
            get { return ConfigurationManager.AppSettings["ConnStr"]; }
        }

        #endregion

        #region "FILL DATA TABLE"

        public static void Fill(DataTable dataTable, String procedureName)
        {
            var oConnection = new SqlConnection(ConnectionStr);
            var oCommand = new SqlCommand(procedureName, oConnection) { CommandType = CommandType.StoredProcedure };

            var oAdapter = new SqlDataAdapter { SelectCommand = oCommand };

            oConnection.Open();
            using (var oTransaction = oConnection.BeginTransaction())
            {
                try
                {
                    oAdapter.SelectCommand.Transaction = oTransaction;
                    oAdapter.Fill(dataTable);
                    oTransaction.Commit();
                }
                catch
                {
                    oTransaction.Rollback();
                    throw;
                }
                finally
                {
                    if (oConnection.State == ConnectionState.Open)
                        oConnection.Close();
                    oConnection.Dispose();
                    oAdapter.Dispose();
                }
            }
        }

        public static void Fill(DataTable dataTable, String procedureName, SqlParameter[] parameters)
        {
            var oConnection = new SqlConnection(ConnectionStr);
            var oCommand = new SqlCommand(procedureName, oConnection) { CommandType = CommandType.StoredProcedure };

            if (parameters != null)
                oCommand.Parameters.AddRange(parameters);

            var oAdapter = new SqlDataAdapter { SelectCommand = oCommand };

            oConnection.Open();
            using (var oTransaction = oConnection.BeginTransaction())
            {
                try
                {
                    oAdapter.SelectCommand.Transaction = oTransaction;
                    oAdapter.Fill(dataTable);
                    oTransaction.Commit();
                }
                catch
                {
                    oTransaction.Rollback();
                    throw;
                }
                finally
                {
                    if (oConnection.State == ConnectionState.Open)
                        oConnection.Close();
                    oConnection.Dispose();
                    oAdapter.Dispose();
                }
            }
        }

        #endregion

        #region "FILL DATASET"

        public static void Fill(DataSet dataSet, String procedureName)
        {
            var oConnection = new SqlConnection(ConnectionStr);
            var oCommand = new SqlCommand(procedureName, oConnection) { CommandType = CommandType.StoredProcedure };

            var oAdapter = new SqlDataAdapter { SelectCommand = oCommand };

            oConnection.Open();
            using (var oTransaction = oConnection.BeginTransaction())
            {
                try
                {
                    oAdapter.SelectCommand.Transaction = oTransaction;
                    oAdapter.Fill(dataSet);
                    oTransaction.Commit();
                }
                catch
                {
                    oTransaction.Rollback();
                    throw;
                }
                finally
                {
                    if (oConnection.State == ConnectionState.Open)
                        oConnection.Close();
                    oConnection.Dispose();
                    oAdapter.Dispose();
                }
            }
        }

        public static void Fill(DataSet dataSet, String procedureName, SqlParameter[] parameters)
        {
            var oConnection = new SqlConnection(ConnectionStr);
            var oCommand = new SqlCommand(procedureName, oConnection) { CommandType = CommandType.StoredProcedure };

            if (parameters != null)
                oCommand.Parameters.AddRange(parameters);

            var oAdapter = new SqlDataAdapter { SelectCommand = oCommand };

            oConnection.Open();
            using (var oTransaction = oConnection.BeginTransaction())
            {
                try
                {
                    oAdapter.SelectCommand.Transaction = oTransaction;
                    oAdapter.Fill(dataSet);
                    oTransaction.Commit();
                }
                catch
                {
                    oTransaction.Rollback();
                    throw;
                }
                finally
                {
                    if (oConnection.State == ConnectionState.Open)
                        oConnection.Close();
                    oConnection.Dispose();
                    oAdapter.Dispose();
                }
            }
        }

        #endregion

        #region "EXECUTE SCALAR"

        public static object ExecuteScalar(String procedureName)
        {
            var oConnection = new SqlConnection(ConnectionStr);
            var oCommand = new SqlCommand(procedureName, oConnection) { CommandType = CommandType.StoredProcedure };

            object oReturnValue;
            oConnection.Open();
            using (SqlTransaction oTransaction = oConnection.BeginTransaction())
            {
                try
                {
                    oCommand.Transaction = oTransaction;
                    oReturnValue = oCommand.ExecuteScalar();
                    oTransaction.Commit();
                }
                catch
                {
                    oTransaction.Rollback();
                    throw;
                }
                finally
                {
                    if (oConnection.State == ConnectionState.Open)
                        oConnection.Close();
                    oConnection.Dispose();
                    oCommand.Dispose();
                }
            }
            return oReturnValue;
        }

        public static object ExecuteScalar(String procedureName, SqlParameter[] parameters)
        {
            var oConnection = new SqlConnection(ConnectionStr);
            var oCommand = new SqlCommand(procedureName, oConnection) { CommandType = CommandType.StoredProcedure };

            object oReturnValue;
            oConnection.Open();
            using (var oTransaction = oConnection.BeginTransaction())
            {
                try
                {
                    if (parameters != null)
                        oCommand.Parameters.AddRange(parameters);

                    oCommand.Transaction = oTransaction;
                    oReturnValue = oCommand.ExecuteScalar();
                    oTransaction.Commit();
                }
                catch
                {
                    oTransaction.Rollback();
                    throw;
                }
                finally
                {
                    if (oConnection.State == ConnectionState.Open)
                        oConnection.Close();
                    oConnection.Dispose();
                    oCommand.Dispose();
                }
            }
            return oReturnValue;
        }

        #endregion

        #region "EXECUTE NON QUERY"

        public static int ExecuteNonQuery(string procedureName)
        {
            var oConnection = new SqlConnection(ConnectionStr);
            var oCommand = new SqlCommand(procedureName, oConnection) { CommandType = CommandType.StoredProcedure };

            int iReturnValue;
            oConnection.Open();
            using (var oTransaction = oConnection.BeginTransaction())
            {
                try
                {
                    oCommand.Transaction = oTransaction;
                    iReturnValue = oCommand.ExecuteNonQuery();
                    oTransaction.Commit();
                }
                catch
                {
                    oTransaction.Rollback();
                    throw;
                }
                finally
                {
                    if (oConnection.State == ConnectionState.Open)
                        oConnection.Close();
                    oConnection.Dispose();
                    oCommand.Dispose();
                }
            }
            return iReturnValue;
        }

        public static int ExecuteNonQuery(string procedureName, SqlParameter[] parameters)
        {
            var oConnection = new SqlConnection(ConnectionStr);
            var oCommand = new SqlCommand(procedureName, oConnection) { CommandType = CommandType.StoredProcedure };

            int iReturnValue;
            oConnection.Open();
            using (var oTransaction = oConnection.BeginTransaction())
            {
                try
                {
                    if (parameters != null)
                        oCommand.Parameters.AddRange(parameters);

                    oCommand.Transaction = oTransaction;
                    iReturnValue = oCommand.ExecuteNonQuery();
                    oTransaction.Commit();
                }
                catch
                {
                    oTransaction.Rollback();
                    throw;
                }
                finally
                {
                    if (oConnection.State == ConnectionState.Open)
                        oConnection.Close();
                    oConnection.Dispose();
                    oCommand.Dispose();
                }
            }
            return iReturnValue;
        }

        #endregion

    }
}
