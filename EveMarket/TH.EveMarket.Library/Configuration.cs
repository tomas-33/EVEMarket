namespace TH.EveMarket.Library
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Serialization.Formatters.Soap;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using TH.EveMarket.Library.Data;
    using TH.EveMarket.Library.Utility;

    [Serializable]
    public static class Configuration
    {
        public static AppConfig AppConfig { get; set; } = new AppConfig();
        public static MarketConfiguration MarketConfiguration { get; set; } = new MarketConfiguration();

        static Configuration()
        {
            Configuration.LoadConfig();
        }

        public static void LoadConfig()
        {
            var appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EveMarket\\config.xml");
            var exePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "config.xml");

            if (File.Exists(exePath))
            {
                AppConfig = Files<AppConfig>.Load(exePath);
                AppConfig.ActualConfigFolder = Path.GetDirectoryName(exePath);
            }
            else if(File.Exists(appDataPath))
            {
                AppConfig = Files<AppConfig>.Load(appDataPath);
                AppConfig.ActualConfigFolder = Path.GetDirectoryName(appDataPath);
            }
            else
            {
                Files<AppConfig>.Save(exePath, AppConfig);
                AppConfig.ActualConfigFolder = Path.GetDirectoryName(exePath);
            }

            MarketConfiguration.SystemIds = SolarSystem.LoadFromCsv(Path.Combine(Configuration.AppConfig.ActualConfigFolder, Configuration.AppConfig.SystemIdsFileName));
            MarketConfiguration.TypeIds = TypeIds.GetTypeIds();
        }

        public static void CreateAppDataFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }
    }
}
