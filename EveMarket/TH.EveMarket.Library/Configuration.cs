using System.IO;
using System.Xml.Linq;

namespace TH.EveMarket.Library
{
    public static class Configuration
    {
        public static string AssemblyPath { get; } = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public static string EveCentralApiUri { get; set; } = string.Empty;
        public static string EveMarketerApiUri { get; set; } = string.Empty;
        public static string TypeIdsUri { get; set; } = string.Empty;
        public static string RoutesPath { get; set; } = string.Empty;
        public static string ProductsPath { get; set; } = string.Empty;
        public static string TypeIdsPath { get; set; } = string.Empty;
        public static string SystemIdsPath { get; set; } = string.Empty;
        public static bool UseDefaultProxy { get; set; } = false;

        static Configuration()
        {
            Configuration.LoadConfig();
        }

        public static void LoadConfig()
        {
            XDocument xml = new XDocument();

            try
            {
                xml = XDocument.Load(Configuration.AssemblyPath + "\\config.xml");
            }
            catch (FileNotFoundException)
            {
                xml = Configuration.CreateDefaultConfig();
                xml.Save(Configuration.AssemblyPath + "\\config.xml");
            }

            Configuration.RoutesPath = xml.Root.Element("routes").Attribute("path").Value;
            Configuration.ProductsPath = xml.Root.Element("products").Attribute("path").Value;
            Configuration.SystemIdsPath = xml.Root.Element("systemIds").Attribute("path").Value;
            Configuration.TypeIdsPath = xml.Root.Element("typeIds").Attribute("path").Value;
            Configuration.TypeIdsUri = xml.Root.Element("typeIds").Attribute("uri").Value;
            Configuration.EveCentralApiUri = xml.Root.Element("eveCentralApi").Attribute("uri").Value;
            Configuration.EveMarketerApiUri = xml.Root.Element("eveMarketerApi").Attribute("uri").Value;
            Configuration.UseDefaultProxy = bool.Parse(xml.Root.Element("useDefaultProxy").Attribute("value").Value);
        }

        public static XDocument CreateDefaultConfig()
        {
            XDocument xml = new XDocument
                (
                new XElement
                    (
                    "config",
                        new XElement("routes", new XAttribute("path", Configuration.AssemblyPath + "\\Routes.txt")),
                        new XElement("products", new XAttribute("path", Configuration.AssemblyPath + "\\Products.txt")),
                        new XElement("systemIds", new XAttribute("path", Configuration.AssemblyPath + "\\SystemIds.txt")),
                        new XElement("typeIds", new XAttribute[] { new XAttribute("path", Configuration.AssemblyPath + "\\typeids.txt"), new XAttribute("uri", "http://eve-files.com/chribba/typeid.txt") }),
                        new XElement("eveCentralApi", new XAttribute("uri", "http://api.eve-central.com/api/marketstat")),
                        new XElement("eveMarketerApi", new XAttribute("uri", "https://api.evemarketer.com/ec/marketstat")),
                        new XElement("useDefaultProxy", new XAttribute("value", false.ToString()))
                    )
                );

            return xml;
        }
    }
}
