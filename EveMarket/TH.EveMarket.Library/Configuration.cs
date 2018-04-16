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

        //public static string AssemblyPath { get; } = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        //public static string EveCentralApiUri { get; set; } = string.Empty;
        //public static string EveMarketerApiUri { get; set; } = string.Empty;
        //public static string EveOnlineApiUri { get; set; } = string.Empty;
        //public static string TypeIdsUri { get; set; } = string.Empty;
        //public static string RoutesPath { get; set; } = string.Empty;
        //public static string ProductsPath { get; set; } = string.Empty;
        //public static string TypeIdsPath { get; set; } = string.Empty;
        //public static string SystemIdsPath { get; set; } = string.Empty;
        //public static bool UseDefaultProxy { get; set; } = false;
        //public static System.Globalization.CultureInfo EnCultureInfo { get; }  = new System.Globalization.CultureInfo("en-US");

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

            //XDocument xml = new XDocument();

            //try
            //{
            //    xml = XDocument.Load(Configuration.AppConfigAssemblyPath + "\\config.xml");
            //}
            //catch (FileNotFoundException)
            //{
            //    xml = Configuration.AppConfigCreateDefaultConfig();
            //    xml.Save(Configuration.AppConfigAssemblyPath + "\\config.xml");
            //}

            //Configuration.AppConfigRoutesPath = xml.Root.Element("routes").Attribute("path").Value;
            //Configuration.AppConfigProductsPath = xml.Root.Element("products").Attribute("path").Value;
            //Configuration.AppConfigSystemIdsPath = xml.Root.Element("systemIds").Attribute("path").Value;
            //Configuration.AppConfigTypeIdsPath = xml.Root.Element("typeIds").Attribute("path").Value;
            //Configuration.AppConfigTypeIdsUri = xml.Root.Element("typeIds").Attribute("uri").Value;
            //Configuration.AppConfigEveCentralApiUri = xml.Root.Element("eveCentralApi").Attribute("uri").Value;
            //Configuration.AppConfigEveMarketerApiUri = xml.Root.Element("eveMarketerApi").Attribute("uri").Value;
            //Configuration.AppConfigEveOnlineApiUri = xml.Root.Element("eveOnlineApi").Attribute("uri").Value;
            //Configuration.AppConfigUseDefaultProxy = bool.Parse(xml.Root.Element("useDefaultProxy").Attribute("value").Value);

            //Save();
        }

        //public static XDocument CreateDefaultConfig()
        //{
        //    XDocument xml = new XDocument
        //        (
        //        new XElement
        //            (
        //            "config",
        //                new XElement("routes", new XAttribute("path", Configuration.AppConfigAssemblyPath + "\\Routes.txt")),
        //                new XElement("products", new XAttribute("path", Configuration.AppConfigAssemblyPath + "\\Products.txt")),
        //                new XElement("systemIds", new XAttribute("path", Configuration.AppConfigAssemblyPath + "\\SystemIds.txt")),
        //                new XElement("typeIds", new XAttribute[] { new XAttribute("path", Configuration.AppConfigAssemblyPath + "\\typeids.txt"), new XAttribute("uri", "http://eve-files.com/chribba/typeid.txt") }),
        //                new XElement("eveCentralApi", new XAttribute("uri", "http://api.eve-central.com/api/marketstat")),
        //                new XElement("eveMarketerApi", new XAttribute("uri", "https://api.evemarketer.com/ec/marketstat")),
        //                new XElement("eveOnlineApi", new XAttribute("uri", "https://api.eveonline.com/eve/CharacterID.xml.aspx")),
        //                new XElement("useDefaultProxy", new XAttribute("value", false.ToString()))
        //            )
        //        );

        //    return xml;
        //}

        

        public static void CreateAppDataFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }
    }
}
