using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;

namespace DAT.SqlHelper
{
    public static class SqlHelper
    {

        public static DataTable ExecuteScalar(String procedureName, params object[] parameters)
        {
            if (parameters == null || parameters.Length == 0 || parameters.Length % 2 != 0)
                return null;

            var keys = parameters.Where((p, i) => i % 2 == 0).ToArray();
            var values = parameters.Where((p, i) => i % 2 != 0).ToArray();

            if (keys.Length != values.Length) return null;

            var dt = new DataTable();
            SqlHelperBase.Fill(dt, procedureName, keys.Select((t, i) => new SqlParameter(t.ToString(), values[i])).ToArray());
            return dt;
        }

        public static int ExecuteNonQueryNoParam(string procedureName)
        {
            return SqlHelperBase.ExecuteNonQuery(procedureName);
        }

        public static int ExecuteNonQuery(string procedureName, params object[] parameters)
        {
            if (parameters == null || parameters.Length == 0 || parameters.Length % 2 != 0)
                return -1;

            var keys = parameters.Where((p, i) => i % 2 == 0).ToArray();
            var values = parameters.Where((p, i) => i % 2 != 0).ToArray();

            if (keys.Length != values.Length) return -1;

            return SqlHelperBase.ExecuteNonQuery(procedureName, keys.Select((t, i) => new SqlParameter(t.ToString(), values[i])).ToArray());
        }
    }
}
