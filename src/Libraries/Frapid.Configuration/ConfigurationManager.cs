using System.Configuration;
using System.Web.Hosting;

namespace Frapid.Configuration
{
    public static class ConfigurationManager
    {
        /// <summary>
        /// Gets the configuration value of the requested key.
        /// </summary>
        /// <param name="configFileName">The name of the configuration file.</param>
        /// <param name="key">The configuration key to find.</param>
        /// <returns>Returns the configuration value of the requested key.</returns>
        /// <exception cref="ConfigurationErrorsException">Thrown when the supplied configuration file is invalid or could not be loaded.</exception>
        public static string GetConfigurationValue(string configFileName, string key)
        {
            if (configFileName == null)
            {
                return string.Empty;
            }

            var fileName = System.Configuration.ConfigurationManager.AppSettings[configFileName];

            string path = HostingEnvironment.MapPath(fileName);

            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap {ExeConfigFilename = path};

            System.Configuration.Configuration config =
                System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(configFileMap,
                    ConfigurationUserLevel.None);
            AppSettingsSection section = config.GetSection("appSettings") as AppSettingsSection;

            return section?.Settings[key] != null ? section.Settings[key].Value : string.Empty;
        }
    }
}