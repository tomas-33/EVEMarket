namespace TH.EveMarket.Library
{
    using System;
    using System.IO;
    using TH.EveMarket.Library.Data;
    using TH.EveMarket.Library.Utility;

    public static class Configuration
    {
        public static AppConfig AppConfig { get; set; } = new AppConfig();
        public static MarketConfiguration MarketConfiguration { get; set; } = new MarketConfiguration();

        static Configuration()
        {
            LoadConfig();
        }

        public static void LoadConfig()
        {
            var appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EveMarket\\config.xml");
            var exePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "config.xml");

            if (File.Exists(exePath))
            {
                AppConfig = Files<AppConfig>.LoadXml(exePath);
                AppConfig.ActualConfigFolder = Path.GetDirectoryName(exePath);
            }
            else if (File.Exists(appDataPath))
            {
                AppConfig = Files<AppConfig>.LoadXml(appDataPath);
                AppConfig.ActualConfigFolder = Path.GetDirectoryName(appDataPath);
            }
            else
            {
                Files<AppConfig>.SaveXml(exePath, AppConfig);
                AppConfig.ActualConfigFolder = Path.GetDirectoryName(exePath);
            }

            var marketConfigPath = Path.Combine(AppConfig.ActualConfigFolder, "MarketConfig.xml");
            if (File.Exists(marketConfigPath))
            {
                Files<MarketConfiguration>.LoadXml(marketConfigPath);
            }
            else
            {
                Files<MarketConfiguration>.SaveXml(marketConfigPath, MarketConfiguration);
            }

            MarketConfiguration.SystemIds = SolarSystem.LoadFromCsv(Path.Combine(AppConfig.ActualConfigFolder, AppConfig.SystemIdsFileName));
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
