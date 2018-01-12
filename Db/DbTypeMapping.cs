using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace ORM
{
    public class DbTypeMapping
    {
        private readonly Dictionary<string, string> _supportTypes = new Dictionary<string, string>();

        public string ResolveTypeStr(string typeStr)
        {
            ResetTypes();
            return _supportTypes.FirstOrDefault(eachType => typeStr.StartsWith(eachType.Key)).Value;
        }

        private void ResetTypes()
        {
            _supportTypes.Clear();
            _supportTypes.Add("int", "int");
            _supportTypes.Add("smallint", "short");
            _supportTypes.Add("bigint", "long");

            _supportTypes.Add("binary", "byte[]");
            _supportTypes.Add("image", "byte[]");
            _supportTypes.Add("timestamp", "byte[]");
            _supportTypes.Add("varbinary", "byte[]");

            _supportTypes.Add("tinyint", "byte");

            _supportTypes.Add("bit", "bool");

            _supportTypes.Add("decimal", "decimal");
            _supportTypes.Add("money", "decimal");
            _supportTypes.Add("smallmoney", "decimal");

            _supportTypes.Add("numeric", "double");
            _supportTypes.Add("float", "double");
            _supportTypes.Add("real", "double");

            _supportTypes.Add("datetime", "DateTime");
            _supportTypes.Add("smalldatetime", "DateTime");
            _supportTypes.Add("date", "DateTime");
            _supportTypes.Add("time", "DateTime");
            _supportTypes.Add("datetime2", "DateTime");

            _supportTypes.Add("datetimeoffset", "DateTimeOffset");

            _supportTypes.Add("nvarchar", "string");
            _supportTypes.Add("varchar", "string");
            _supportTypes.Add("nchar", "string");
            _supportTypes.Add("char", "string");
            _supportTypes.Add("text", "string");
            _supportTypes.Add("ntext", "string");
            _supportTypes.Add("xml", "string");


            _supportTypes.Add("uniqueidentifier", "Guid");

            _supportTypes.Add("sql_variant", "Object");
        }
    }
}
