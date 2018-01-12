using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;

namespace SpiderDemo.SearchUtil
{
    public static class RecordUtility
    {


        public static void AppendLog(string log)
        {

            string path = ConfigurationManager.AppSettings["LogPath"];
            if (string.IsNullOrEmpty(path))
            {
                ////failed to find log path in config!!!
                return;
            }

            else
            {
                if (!File.Exists(path))
                {
                    File.Create(path);
                }
                File.AppendText(log);
            }

        }


        public static string ReadLog()
        {
            string path = ConfigurationManager.AppSettings["LogPath"];
            if (!File.Exists(path))
            {
                return null;
            }
            return File.ReadAllText(path);
        }


    }
}