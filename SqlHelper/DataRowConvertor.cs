using System;
using System.Data;
using System.Reflection;

namespace Common
{
    public class DataRowConvertor
    {
        public static void DataRowToObj(DataColumnCollection pDcc, DataRow pDr, Object pObject)
        {
            
            var t = pObject.GetType();
            for (var i = 0; i <= pDcc.Count - 1; i++)
            {
                try
                {
                    t.InvokeMember(pDcc[i].ColumnName,
                                  BindingFlags.SetProperty, null,
                                  pObject,
                                  new[] { pDr[pDcc[i].ColumnName] });
                }
                catch (Exception ex)
                {
                    //Usually you are getting here because a column 
                    //doesn't exist or it is null
                    throw ex;
                }
            }
        }
    }
}
