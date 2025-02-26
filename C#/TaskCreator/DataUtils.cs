using Microsoft.VisualBasic.Logging;
using System.Configuration;
using Serilog;

namespace Utils
{
    public static class DataUtils
    {
        public static string GetFromConfig(string varName) 
        {
            string? var = ConfigurationManager.AppSettings[varName];
            return string.IsNullOrWhiteSpace(var)
                ? throw new KeyNotFoundException($"{varName} is missing or empty in a config file")
                : var;
        }

        public static string? GetNullableFromConfig(string varName)
        {
            try
            {
                return GetFromConfig(varName);
            } catch(KeyNotFoundException)
            {
                return null;
            }
        }

        public static string GetExecutablePath(string fileName)
        {
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            return File.Exists(fullPath) ?
                fullPath
                : throw new FileNotFoundException($"The file '{fileName}' was not found at '{fullPath}'");
        }

        public static void SaveDataToConfig(ICollection<KeyValuePair<string, string>> keyValuePairs)
        {
            Configuration conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            foreach(KeyValuePair<string, string> pair in keyValuePairs)
            {
                var settings = conf.AppSettings.Settings;
                if (settings[pair.Key] != null)
                {
                    settings[pair.Key].Value = pair.Value; 
                }
                else
                {
                    settings.Add(pair.Key, pair.Value);
                }
            }

            conf.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
