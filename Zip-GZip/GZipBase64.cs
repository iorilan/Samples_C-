using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEC.ESBU.Ticketing.Client.ValidatorBase.Helpers
{
    public static class ZipUtil
    {
        public static string CompressFromBase64(string param)
        {
            byte[] data = Convert.FromBase64String(param);
            var ms = new MemoryStream();
            var stream = new GZipStream(ms, CompressionMode.Compress);
            try
            {
                stream.Write(data, 0, data.Length);
            }
            finally
            {
                stream.Close();
                ms.Close();
            }
            return Convert.ToBase64String(ms.ToArray());
        }


        public static string Decompress(string param)
        {
            string commonString = "";
            byte[] buffer = Convert.FromBase64String(param);
            MemoryStream ms = new MemoryStream(buffer);
            Stream sm = new GZipStream(ms, CompressionMode.Decompress);
            StreamReader reader = new StreamReader(sm, Encoding.UTF8);
            try
            {
                commonString = reader.ReadToEnd();
            }
            finally
            {
                sm.Close();
                ms.Close();
            }
            return commonString;
        }
    }

}
