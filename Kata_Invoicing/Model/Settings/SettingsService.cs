using System;
using System.IO;
using System.Security;
using System.Xml.Serialization;
using log4net;

namespace Kata_Invoicing.Model.Settings
{
    /// <summary>
    /// Class for accessing settings
    /// </summary>
    public static class SettingsService
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(SettingsService));

        private static readonly string SettingsFileDirectory;
        private static readonly string SettingsFileFullName;

        static SettingsService()
        {
            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            SettingsFileDirectory = Path.Combine(appPath, Constants.SettingsDirectory);
            SettingsFileFullName = Path.Combine(SettingsFileDirectory, Constants.SettingsFileName);
        }

        public static Settings GetSettings()
        {
            Settings settings = new Settings();

            if (File.Exists(SettingsFileFullName))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                    using (TextReader reader = new StreamReader(SettingsFileFullName))
                    {
                        settings = (Settings)serializer.Deserialize(reader);
                    }

                }
                catch (Exception e)
                {
                    _log.Error(e + " GetSettings()");
                }
            }

            return settings;
        }

        public static string SaveSettings(Settings settings)
        {
            string msg = string.Empty;

            if (!Directory.Exists(SettingsFileDirectory))
            {
                Directory.CreateDirectory(SettingsFileDirectory);
            }

            XmlSerializer serializer = new XmlSerializer(typeof(Settings));

            try
            {
                using (TextWriter writer = new StreamWriter(SettingsFileFullName))
                {
                    serializer.Serialize(writer, settings);
                }
            }
            catch (SecurityException se)
            {
                _log.Error(se.ToString() + ". SecurityException");
                msg = "Security Exception! Make sure you have rights to the application directory.";
            }
            catch (Exception e)
            {
                _log.Error(e.ToString());
                msg = "Error!";
            }

            return msg;
        }

    }
}
