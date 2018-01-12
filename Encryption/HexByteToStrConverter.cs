using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Md5StrGenerator
{
    class HexByteToStrConverter
    {


        public static string ConvertBytesToHex(byte[] arrByte)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in arrByte)
                sb.AppendFormat("{0:X2}", b);
            return sb.ToString();
        }


        public static byte[] ConvertHexToBytes(string value)
        {
            int len = value.Length / 2;
            byte[] ret = new byte[len];
            for (int i = 0; i < len; i++)
                ret[i] = Convert.ToByte(value.Substring(i * 2, 2), 16);
            return ret;
        }


    }
}
