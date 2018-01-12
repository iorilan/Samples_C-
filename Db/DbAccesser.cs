using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;

namespace ORM
{
    public class DbAccesser
    {
        private const string SqlGetAllTables = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ";
        private const string SqlGetAllColumns = "SELECT COLUMN_NAME,DATA_TYPE,CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME ='{0}' ORDER BY ORDINAL_POSITION";
        private readonly List<string> _lstAllCharType = new List<string> { "nvarchar", "varchar", "char", "nchar","binary","varbinary" };

        public void ExcSqlScript(string sql)
        {
            var conn = new SqlConnection(ConfigLoader.ConnStr);
            var server = new Server(new ServerConnection(conn));
            server.ConnectionContext.ExecuteNonQuery(sql);
        }

        public void ExcSql(string sql)
        {
            var oConnection = new SqlConnection(ConfigLoader.ConnStr);
            var oCommand = new SqlCommand(sql, oConnection) { CommandType = CommandType.Text };

            oConnection.Open();

            using (var oTransaction = oConnection.BeginTransaction())
            {
                try
                {
                    oCommand.Transaction = oTransaction;
                    oCommand.ExecuteNonQuery();
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
                }
            }
        }

        public string[] GetAllTables()
        {
            var oConnection = new SqlConnection(ConfigLoader.ConnStr);
            var oCommand = new SqlCommand(SqlGetAllTables, oConnection) { CommandType = CommandType.Text };

            var oAdapter = new SqlDataAdapter { SelectCommand = oCommand };
            var dataTable = new DataTable();
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
            if (dataTable.Rows == null || dataTable.Rows.Count <= 0)
            {
                return null;
            }
            var names = new List<string>();
            for (var i = 0; i < dataTable.Rows.Count; i++)
            {
                names.Add(dataTable.Rows[i][0].ToString());
            }
            return names.ToArray();
        }

        public Dictionary<string, string> GetColumnsByTblName(string name)
        {
            var oConnection = new SqlConnection(ConfigLoader.ConnStr);
            var oCommand = new SqlCommand(string.Format(SqlGetAllColumns, name), oConnection) { CommandType = CommandType.Text };

            var oAdapter = new SqlDataAdapter { SelectCommand = oCommand };
            var dataTable = new DataTable();
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
            if (dataTable.Rows == null || dataTable.Rows.Count <= 0)
            {
                return null;
            }

            var columns = new Dictionary<string, string>();

            for (var i = 0; i < dataTable.Rows.Count; i++)
            {
                var dbType = dataTable.Rows[i][1].ToString();
                if (_lstAllCharType.Contains(dbType))
                    dbType += "(" + dataTable.Rows[i][2] + ")";

                columns.Add(dataTable.Rows[i][0].ToString(), dbType);
            }
            return columns;
        }

    }
}
