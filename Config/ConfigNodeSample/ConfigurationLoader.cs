using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMAgentService.IO.Entity;
using System.IO;
using System.Xml;

namespace IMAgentService.IO.Utility
{
    public static class ConfigurationLoader
    {
        /// <summary>
        /// 读取自定义的XML配置节点(省份OMTAG配置)
        /// </summary>
        private static System.Configuration.ConfigXmlDocument configXml;

        /// <summary>
        /// 配置文件路径(确保CONFIG文件与可执行文件是同级目录！！)
        /// </summary>
        private const string ConfigFilePath = @"..\..\..\IMAgentService.IO\Config\App.config";

        #region 构造器
        static ConfigurationLoader()
        {
            try
            {
                configXml = new System.Configuration.ConfigXmlDocument();
                configXml.Load(ConfigFilePath);
            }
            catch (FileNotFoundException ex)
            {
                ////加载文件出错，记录日志
            }
        }
        #endregion

        #region 读文本文件配置

        /// <summary>
        /// 根据omTag获得对应的文件路径
        /// </summary>
        /// <param name="omTag"></param>
        /// <returns></returns>
        public static string GetPathByOmTag(string omTag)
        {
            ProvinceConfig config = LoadProvinceConfigByOmtag(omTag);
            if (config != null)
            {
                return config.Path;
            }
            return null;
        }

        /// <summary>
        /// 获得omTag对应的配置节实体
        /// </summary>
        /// <param name="omTag"></param>
        /// <returns></returns>
        public static ProvinceConfig LoadProvinceConfigByOmtag(string omTag)
        {
            ProvinceConfig configEntity;
            XmlNodeList nodeList = configXml.GetElementsByTagName("provinceConfig");
            if (nodeList != null)
            {
                foreach (XmlNode node in nodeList)
                {
                    if (node.Attributes["domain"] != null && node.Attributes["domain"].Value == omTag)
                    {
                        configEntity = new ProvinceConfig();
                        configEntity.Domain = node.Attributes["domain"].Value;
                        configEntity.Name = node.Attributes["name"].Value;
                        configEntity.Path = node.Attributes["path"].Value;

                        return configEntity;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 获得所有省份的配置节实体
        /// </summary>
        /// <returns></returns>
        public static List<ProvinceConfig> GetAllProvinceConfig()
        {
            List<ProvinceConfig> configList = new List<ProvinceConfig>();
            XmlNodeList nodeList = configXml.GetElementsByTagName("province");
            if (nodeList != null)
            {
                foreach (XmlNode node in nodeList)
                {
                    configList.Add(new ProvinceConfig()
                    {
                        Name = node.Attributes["name"].Value,
                        Domain = node.Attributes["domain"].Value,
                        Path = node.Attributes["path"].Value
                    });
                }
            }
            return configList;
        }

        #endregion

        #region 获得timer休眠时间

        public static string GetTimerSleepTime()
        {
            XmlNodeList nodeList = configXml.GetElementsByTagName("timerConfig");
            if (nodeList == null || nodeList.Count == 0 || nodeList[0].Attributes["sleepTime"] == null)
            {
                return null;
            }
            return nodeList[0].Attributes["sleepTime"].Value;
        }

        #endregion
    }
}
