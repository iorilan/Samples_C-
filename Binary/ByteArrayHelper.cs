﻿using System;
using System.Collections;
using System.Globalization;
using System.Diagnostics;
using System.Linq;

namespace SmppStack.Helpers
{
    /// <summary>
    ///     字节数组帮助器，提供了相关的基本相关操作
    /// </summary>
    public class ByteArrayHelper
    {
        /// <summary>
        ///     比对2个字节数组是否每个字节数值都相同
        /// </summary>
        /// <param name="orgByte">第一个字节数组</param>
        /// <param name="newByte">第二个字节数组</param>
        /// <returns>返回false,则表示不相同</returns>
        public static bool Equals(byte[] orgByte, byte[] newByte)
        {
            if (orgByte == null && newByte == null)
            {
                return false;
            }
            if (orgByte.Length != newByte.Length)
            {
                return false;
            }
            for (int i = 0; i < orgByte.Length; i++)
            {
                if (orgByte[i] != newByte[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        ///     2个字节数组相互与操作。
        /// </summary>
        /// <param name="array1">数组1</param>
        /// <param name="array2">数组2</param>
        /// <returns>返回与操作的结果</returns>
        public static byte[] EachOperation(byte[] array1, byte[] array2)
        {
            if (array2.Length != array2.Length)
            {
                return null;
            }
            byte[] newData = new byte[array1.Length];
            for (int i = 0; i < newData.Length; i++)
            {
                byte data = (byte)(array1[i] & array2[i]);
                newData.SetValue(data, i);
            }
            return newData;
        }

        /// <summary>
        ///     从指定位置开始，比对2个字节数组是否每个字节数值都相同
        ///          * 注意： compareSize不能小于0
        /// </summary>
        /// <param name="orgByte">第一个字节数组</param>
        /// <param name="newByte">第二个字节数组</param>
        /// <param name="compareSize">比对位置</param>
        /// <returns>返回false,则表示不相同</returns>
        public static bool Equals(byte[] orgByte, byte[] newByte, int compareSize)
        {
            if (orgByte == null && newByte == null)
            {
                return false;
            }
            if (compareSize < 0)
            {
                return false;
            }
            if (orgByte.Length < compareSize && newByte.Length < compareSize)
            {
                return false;
            }
            for (int i = 0; i < compareSize; i++)
            {
                if (orgByte[i] != newByte[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        ///     移除指定字节数组中的末尾字节，并返回剩余的真实数据
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <returns>返回剩余的真实数据</returns>
        public static byte[] Pickup(byte[] data)
        {
            if (data == null  || data.Length == 0)
            {
                return null;
            }
            if (data.Length == 1)
            {
                return data[0] == 0x00 ? null : data;
            }
            return GetNextData(data, 0, data.Length - 1);
        }

        /// <summary>
        ///     从一个字节数组中，按照指定的填充顺序获取真实数据
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <param name="isLeftPickup">填充顺序标示</param>
        /// <returns>返回真是数据</returns>
        public static byte[] PickupContent(byte[] data, bool isLeftPickup)
        {
            if (data == null || data.Length == 0)
            {
                return data;
            }
            if (JudgeNull(data))
            {
                return null;
            }
            int offset;
            byte[] newData;
            int lastIndex;
            if (isLeftPickup)
            {
                int innerOffset = IndexOfFlagForArray(data, new byte[] {0, 0}, FLAG.End);
                lastIndex = innerOffset == -1 ? 0 : innerOffset + 2;
                offset = data.Length - lastIndex;
                newData = new byte[data.Length - lastIndex];
                Array.ConstrainedCopy(GetReallyData(data, lastIndex, offset), 0, newData, 0, offset);
            }
            else
            {
                lastIndex = IndexOfFlagForArray(data, new byte[] { 0, 0 }, FLAG.Begin);
                offset = data.Length - (lastIndex == -1 ? 0 : (data.Length - lastIndex));
                newData = new byte[offset];
                Array.ConstrainedCopy(GetReallyData(data, 0, offset), 0, newData, 0, offset);
            }
            return newData;
        }

        /// <summary>
        ///     补齐数据
        /// </summary>
        /// <param name="data">准备补齐的数据</param>
        /// <param name="maxLength">最大长度</param>
        /// <param name="isLeftFill">左填充标示位</param>
        /// <returns>返回填充后的结果</returns>
        public static byte[] Fill(byte[] data, int maxLength, bool isLeftFill)
        {
            int offset = 0;
            int size;
            byte[] realData;
            if ((data == null || data.Length == 0) && maxLength > 0)
            {
                return CreateNullByteArray(maxLength);
            }
            //与原有规格长度不匹配
            if (data.Length < maxLength)
            {
                realData = new byte[maxLength];
                size = maxLength - data.Length;
                if (isLeftFill)
                {
                    Array.ConstrainedCopy(CreateNullByteArray(size), 0, realData, offset, size);
                    offset += size;
                    Array.ConstrainedCopy(data, 0, realData, offset, data.Length);
                }
                else
                {
                    Array.ConstrainedCopy(data, 0, realData, offset, data.Length);
                    offset += data.Length;
                    Array.ConstrainedCopy(CreateNullByteArray(size), 0, realData, offset, size);
                }
                return realData;
            }
            //与原有规格匹配，但是内部数据的填充规则需要变化
            //查找最后一次双字节都为空的索引
            return data;
            //int lastIndex = IndexOfFlagForArray(data, new byte[] {0, 0}, FLAG.End);
            //if (lastIndex == -1)
            //{
            //    return data;
            //}
            ////获取真实数据
            //realData = GetReallyData(data, lastIndex, data.Length - lastIndex);
            //byte[] newData = new byte[data.Length];
            //size = data.Length - realData.Length;
            //if (isLeftFill)
            //{
            //    Array.ConstrainedCopy(CreateNullByteArray(size), 0, newData, offset, size);
            //    offset += size;
            //    Array.ConstrainedCopy(realData, 0, newData, offset, realData.Length);
            //}
            //else
            //{
            //    Array.ConstrainedCopy(realData, 0, newData, offset, realData.Length);
            //    offset += realData.Length;
            //    Array.ConstrainedCopy(CreateNullByteArray(size), 0, newData, offset, size);
            //}
            //return newData;
        }

        /// <summary>
        ///     创建一个定长的空字节数组
        /// </summary>
        /// <param name="length">长度</param>
        /// <returns>返回空字节数组</returns>
        public static byte[] CreateNullByteArray(int length)
        {
            if (length <= 0)
            {
                throw new Exception("不能创建一个小于或等于0长度的空字节数组。");
            }
            return new byte[length];
        }


        /// <summary>
        ///     判断指定字节数组所有元素是否都为空
        /// </summary>
        /// <param name="totalArry" type="byte[]">
        ///     <para>
        ///         字节数组
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回false, 标示不都为空
        /// </returns>
        public static bool JudgeNull(byte[] totalArry)
        {
            int num = 0;
            for (int i = 0; i < totalArry.Length; i++)
            {
                if (totalArry[i] == 0)
                {
                    num++;
                }
            }
            return (num == totalArry.Length);
        }

        /// <summary>
        ///     获取一个字节数组中的真是数据
        /// </summary>
        /// <param name="data">源字节数组</param>
        /// <param name="offset">偏移</param>
        /// <param name="count">长度</param>
        /// <returns></returns>
        public static byte[] GetReallyData(byte[] data, int offset, int count)
        {
            byte[] NewData;
            if (data == null)
            {
                return null;
            }
            if (count > data.Length)
            {
                return null;
            }
            //if (offset < 0 || offset >= count)
            //{
            //    return null;
            //}
            if (data.Length == count && offset == 0)
            {
                return data;
            }
            if (data.Length > count)
            {
                NewData = new byte[count];
                Array.Copy(data, offset, NewData, 0, count);
                return NewData;
            }
            return null;
        }

        /// <summary>
        ///     获取指定标记内容在字节数组中的位置
        /// </summary>
        /// <param name="totalArray" type="byte[]">
        ///     <para>
        ///         字节数组
        ///     </para>
        /// </param>
        /// <param name="Flag" type="byte[]">
        ///     <para>
        ///         标记内容 : byte[]
        ///     </para>
        /// </param>
        /// <param name="flag" type="KJFramework.Helper.ByteArrayHelper.FLAG">
        ///     <para>
        ///         判断条件 : 开始 / 结尾
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回根据判断条件的位置结果 
        ///        * 如果是从判断第一次相同的情况，则返回第一次开始出现处索引，不存在则返回 -1
        ///        * 如果是从判断最后一次相同的情况，则返回最后一次开始出现处索引，不存在则返回 -1
        /// </returns>
        public static int IndexOfFlagForArray(byte[] totalArray, byte[] Flag, FLAG flag)
        {
            ArrayList list = new ArrayList();
            try
            {
                for (int i = 0; i < totalArray.Length; i++)
                {
                    byte[] destinationArray = new byte[Flag.Length];
                    Array.ConstrainedCopy(totalArray, i, destinationArray, 0, destinationArray.Length);
                    int num2 = 0;
                    for (int j = 0; j < destinationArray.Length; j++)
                    {
                        if (destinationArray[j] != Flag[j])
                        {
                            num2++;
                            break;
                        }
                    }
                    if (num2 == 0)
                    {
                        if (flag == FLAG.Begin)
                        {
                            return (i);
                        }
                        list.Add(i);
                        if ((totalArray.Length - i) < Flag.Length)
                        {
                            return (int)list[list.Count - 1];
                        }
                    }
                }
                return ((list.Count > 0) ? ((int)list[list.Count - 1]) : -1);
            }
            catch (Exception ex)
            {
                if (list.Count > 0)
                {
                    return ((int) list[0]) + Flag.Length;
                }
                return -1;
            }
        }

        /// <summary>
        ///     从一个字节数组中根据开始字节以及结束字节按照指定的分割长度来获取第一次或者最后一次出现的位置
        ///         * 分割长度是指： 开始字节与结尾字节之间的字节长度（距离）
        ///           如： start --中间距离3个位置--  end
        ///           [注] 必须指定一个大于0的分割长度。
        /// </summary>
        /// <param name="totalArray">字节数组</param>
        /// <param name="startFlag">开始字节</param>
        /// <param name="endFlag">结尾字节</param>
        /// <param name="startIndex">开始查找的位置</param>
        /// <param name="splitSize">分割（距离）长度</param>
        /// <param name="flag">判断位置标示</param>
        /// <returns>返回根据判断标示判断到的位置，如果不存在则返回 -1</returns>
        public static int IndexOfFlagForArray(byte[] totalArray, byte startFlag, byte endFlag, int startIndex, int splitSize, FLAG flag)
        {
            if (totalArray == null || totalArray.Length < splitSize + 2)
            {
                return -1;
            }
            if (splitSize <= 0)
            {
                return -1;
            }
            if (startIndex >= totalArray.Length || startIndex + splitSize + 2 > totalArray.Length)
            {
                return -1;
            }
            //计算要查找对象的总长度
            int totalsize = splitSize + 2;
            //设置查找偏移
            int offset = startIndex;
            //计算查找次数
            int searchCount = (totalArray.Length - startIndex)/totalsize;
            if (searchCount <= 0)
            {
                return -1;
            }
            //用来记录最后的记过
            int result = -1;
            //用来保存当前截取到的数据
            byte[] current;
            while (offset != -1)
            {
                try
                {
                    //截取数据
                    current = GetNextData(totalArray, offset, totalsize);
                }
                catch { break; }
                //如果截取的数据为空或者小于指定长度，则直接退出
                if (current == null || current.Length < totalsize)
                {
                    break;
                }
                //如果截取到数据，但是不符合标准
                if (current[0] != startFlag || current[current.Length - 1] != endFlag)
                {
                    offset++;
                    continue;
                }
                //找到标记
                result = offset;
                if (flag == FLAG.Begin)
                {
                    break;
                }
                offset++;
            }
            return result;
        }

        /// <summary>
        ///     获取指定数据中第一次出现"0x00"的位置
        /// </summary>
        /// <param name="data">要检查的数据</param>
        /// <returns>
        ///     返回一个位置
        ///             *　如果返回的位置为－１，则表示不存在该位置，或者给定的内容非法。
        /// </returns>
        public static int GetFirstNullOffset(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                return -1;
            }
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == 0x00)
                {
                    return i;
                }
            }
            return -1;
        }


        /// <summary>
        ///     从指定的位置开始截取指定字节数组中指定长度的数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="startOffset">开始偏移</param>
        /// <param name="catchLength">截取长度</param>
        /// <returns>返回截取后的数据</returns>
        public static byte[] GetNextData(byte[] data, int startOffset, int catchLength)
        {
            if (catchLength <= 0 || startOffset < 0 ||  (data.Length - startOffset) < catchLength)
            {
                return null;
            }
            byte[] catchData = new byte[catchLength];
            Array.ConstrainedCopy(data, startOffset, catchData, 0, catchLength);
            return catchData;
        }

        /// <summary>
        ///     将一个字符串按照指定的数字规则转换成2进制字符串
        /// </summary>
        /// <param name="binary">要转换的字符串</param>
        /// <param name="styles">转换样式</param>
        /// <returns>返回null，则表示不能转换，或者转换错误</returns>
        public static String GetBinary(String binary,NumberStyles styles)
        {
            try
            {
                byte current = Byte.Parse(binary, styles);
                String bin = Convert.ToString(current, 2);
                int len = bin.Length;
                if (bin.Length != 8)
                {
                    for (int i = 0; i < 8 - len; i++)
                    {
                        bin = "0" + bin;
                    }
                }
                return bin;
            }
            catch (Exception e)
            {
                Debug.WriteLine("转换成2进制时出现错误 : " + e.Message);
            }
            return null;
        }

        /// <summary>
        ///     字符串转16进制字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        private static byte[] StrToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// <summary>
        ///     字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ByteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        /// <summary>
        ///     从汉字转换到16进制
        /// </summary>
        /// <param name="s"></param>
        /// <param name="charset">编码,如"utf-8","gb2312"</param>
        /// <param name="fenge">是否每字符用逗号分隔</param>
        /// <returns></returns>
        public static string ToHex(string s, string charset, bool fenge)
        {
            if ((s.Length % 2) != 0)
            {
                s += " ";//空格
                //throw new ArgumentException("s is not valid chinese string!");
            }
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding(charset);
            byte[] bytes = chs.GetBytes(s);
            string str = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                str += string.Format("{0:X}", bytes[i]);
                if (fenge && (i != bytes.Length - 1))
                {
                    str += string.Format("{0}", ",");
                }
            }
            return str.ToLower();
        }

        ///<summary>
        ///     从16进制转换成汉字
        /// </summary>
        /// <param name="hex"></param>
        /// <param name="charset">编码,如"utf-8","gb2312"</param>
        /// <returns></returns>
        public static string UnHex(string hex, string charset)
        {
            if (hex == null)
                throw new ArgumentNullException("hex");
            hex = hex.Replace(",", "");
            hex = hex.Replace("\n", "");
            hex = hex.Replace("\\", "");
            hex = hex.Replace(" ", "");
            if (hex.Length%2 != 0)
            {
                hex += "20"; //空格
            }
            // 需要将 hex 转换成 byte 数组。 
            byte[] bytes = new byte[hex.Length/2];

            for (int i = 0; i < bytes.Length; i++)
            {
                try
                {
                    // 每两个字符是一个 byte。 
                    bytes[i] = byte.Parse(hex.Substring(i*2, 2), NumberStyles.HexNumber);
                }
                catch
                {
                    // Rethrow an exception with custom message. 
                    throw new ArgumentException("hex is not a valid hex number!", "hex");
                }
            }
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding(charset);
            return chs.GetString(bytes);
        }

        /// <summary>
        ///     使用一个目标值替换现有数据中的指定标示。
        ///     <para>* 请确保orgData和findData不为null。</para>
        /// </summary>
        /// <param name="orgData">现有数据</param>
        /// <param name="findData">寻找标示</param>
        /// <param name="targetData">替换的值</param>
        /// <returns>返回替换后的数据</returns>
        public static byte[] Replace(byte[] orgData, byte[] findData, byte[] targetData)
        {
            if (orgData == null || findData == null)
            {
                return null;
            }
            int offset;
            byte[] left;
            byte[] right;
            byte[] temp;
            try
            {
                while ((offset = IndexOfFlagForArray(orgData, findData, FLAG.Begin)) != -1)
                {
                    //找到可替换标示
                    left = new byte[offset];
                    right = new byte[orgData.Length - offset - findData.Length];
                    Array.ConstrainedCopy(orgData, 0, left, 0, left.Length);
                    Array.ConstrainedCopy(orgData, offset + findData.Length, right, 0, right.Length);
                    temp = new byte[left.Length + right.Length + (targetData == null ? 0 : targetData.Length)];
                    Array.ConstrainedCopy(left, 0, temp, 0, left.Length);
                    if (targetData != null)
                    {
                        Array.ConstrainedCopy(targetData, 0, temp, left.Length, targetData.Length);
                    }
                    Array.ConstrainedCopy(right, 0, temp, left.Length + (targetData == null ? 0 : targetData.Length), right.Length);
                    orgData = temp;
                }
                return orgData;
            }
            catch (Exception ex)
            {
                Global.Tracing.Error(ex, "ByteArrayHelper.Replace error.");
                return null;
            }
        }

		
		
		 public static string ConvertBytesToHex(byte[] arrByte)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in arrByte)
                sb.AppendFormat("{0:x2}", b);
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
		
		
		
        /// <summary>
        ///     判断条件枚举
        /// </summary>
        public enum FLAG
        {
            /// <summary>
            ///     开始
            /// </summary>
            Begin,
            /// <summary>
            ///     结束
            /// </summary>
            End
        }
    }
}