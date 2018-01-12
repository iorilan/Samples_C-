using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Common
{
    public class ConfigCommon
    {
        public static string WhichAppFileIsUsing()
        {
            return AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
        }

        /// <summary>
        /// Get Config by key
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static string GetConfig(string key)
        {
            var val = string.Empty;
            if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
                val = ConfigurationManager.AppSettings[key];
            return val;
        }

        /// <summary>
        /// Get All Keys
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetConfig()
        {
            return ConfigurationManager.AppSettings.AllKeys.ToDictionary(key => key, key => ConfigurationManager.AppSettings[key]);
        }

        /// <summary>
        /// Get Config By Key if no return default value
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="defaultValue">default value</param>
        /// <returns></returns>
        public static string GetConfig(string key, string defaultValue)
        {
            var val = defaultValue;
            if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
                val = ConfigurationManager.AppSettings[key];
            return val ?? (defaultValue);
        }

        /// <summary>
        /// Add Key/Value to config
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">value</param>
        /// <returns></returns>
        public static bool SetConfig(string key, string value)
        {
            try
            {
                var conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (!conf.AppSettings.Settings.AllKeys.Contains(key))
                    conf.AppSettings.Settings.Add(key, value);
                else
                    conf.AppSettings.Settings[key].Value = value;
                conf.Save();
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Set A list of config
        /// </summary>
        /// <param name="dict">Key Values pairs</param>
        /// <returns></returns>
        public static bool SetConfig(Dictionary<string, string> dict)
        {
            try
            {
                if (dict == null || dict.Count == 0)
                    return false;
                var conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                foreach (var key in dict.Keys)
                {
                    if (!conf.AppSettings.Settings.AllKeys.Contains(key))
                        conf.AppSettings.Settings.Add(key, dict[key]);
                    else
                        conf.AppSettings.Settings[key].Value = dict[key];
                }
                conf.Save();
                return true;
            }
            catch { return false; }
        }
    }

}
