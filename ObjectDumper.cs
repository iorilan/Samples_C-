using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace ServerToolServer.Util
{
    public class ObjectDumper
    {
        /// <summary>
        /// 本次记录最大的字节数
        /// </summary>
        private static int MAXLENGTH = 16384;

        /// <summary>
        /// 当前要记录的对象
        /// </summary>
        private static object lastObj = null;

        /// <summary>
        /// 反射出obj的字段名和字段值
        /// </summary>
        /// <typeparam name="T">要反射的类型</typeparam>
        /// <param name="obj">实体</param>
        /// <returns>字段名：字段值</returns>
        public static string GetObjStr(object obj)
        {
            if (obj == null)
            {
                return "";
            }

            StringBuilder ret = new StringBuilder(30);
            FieldInfo[] fields = obj.GetType().GetFields();
            if (fields == null || fields.Length == 0)
            {
                return ret.ToString();
            }
            else
            {
                foreach (FieldInfo eachField in fields)
                {
                    GetObjStr(obj, eachField, ret);
                }
            }
        
            return ret.ToString();

        }

        /// <summary>
        /// 反射出对象的字段名：值
        /// </summary>
        /// <param name="entity">要反射的对象</param>
        /// <param name="obj">字段</param>
        /// <param name="str">记录的stringbuilder</param>
        public static void GetObjStr(object entity, object obj, StringBuilder str)
        {
            try
            {

                ////避免无限递归，确保一个对象只会被记录一次
                if (Object.ReferenceEquals(obj, lastObj))
                {
                    return;
                }
                lastObj = obj;


                if (entity == null || obj == null)
                {
                    return;
                }

                if (str.Length > MAXLENGTH)
                {
                    str.Append("...to long...");
                    return;
                }

                FieldInfo f = obj as FieldInfo;
                string typeName = f == null ? obj.GetType().Name : f.Name;
                Type type = f == null ? obj.GetType() : f.FieldType;
                object value = f == null ? obj : f.GetValue(entity);

                if (type.IsValueType || type == typeof(string))
                {
                    if (str.Length > MAXLENGTH)
                    {
                        str.Append("...to long...");
                        return;
                    }
                    str.Append(typeName);
                    str.Append(" : ");
                    str.Append(value);
                    str.Append("\r\n");
                    return;
                }


                ////如果成员是个集合，递归遍历
                if (typeof(IEnumerable).IsAssignableFrom(type))
                {
                    IEnumerable ie = value as IEnumerable;
                    if (ie != null)
                    {
                        IEnumerator list = ie.GetEnumerator();
                        while (list.MoveNext())
                        {
                            ////基本数据类型或者string
                            if (list.Current.GetType().IsValueType || list.Current.GetType() == typeof(string))
                            {
                                if (str.Length > MAXLENGTH)
                                {
                                    str.Append("...to long...");
                                    return;
                                }
                                else
                                {
                                    str.Append(type);
                                    str.Append(" : ");
                                    str.Append(list.Current);
                                    str.Append("\r\n");
                                }
                            }

                            ////自定义类型
                            else
                            {
                                str.Append(list.Current.GetType());
                                str.Append(".");
                                FieldInfo[] fields = list.Current.GetType().GetFields();
                                foreach (FieldInfo subField in fields)
                                {

                                    if (str.Length > MAXLENGTH)
                                    {
                                        str.Append("...to long...");
                                        return;
                                    }
                                    GetObjStr(list.Current, subField, str);
                                }

                            }
                        }
                    }
                }

                else
                {
                    str.Append(type);
                    str.Append(".");
                    FieldInfo[] fields = type.GetFields();
                    if (fields.Length == 0)
                    {
                        return;
                    }
                    foreach (FieldInfo subField in fields)
                    {

                        if (str.Length > MAXLENGTH)
                        {
                            str.Append("...to long...");
                            return;
                        }
                        GetObjStr(value, subField, str);
                    }

                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}
