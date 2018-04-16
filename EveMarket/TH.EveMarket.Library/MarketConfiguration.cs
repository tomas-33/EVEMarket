namespace TH.EveMarket.Library
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Linq;
    using TH.EveMarket.Library.Data;
    using TH.EveMarket.Library.Utility;

    [Serializable]
    public class MarketConfiguration
    {
        public decimal BrokersFeePercent { get; set; } = 0.03M;
        public decimal TransactionTaxPercent { get; set; } = 0.02M;

        [NonSerialized]
        private Dictionary<string, long> _typeIds = new Dictionary<string, long>();
        public Dictionary<string, long> TypeIds { get { return this._typeIds; } set { this._typeIds = value; } }

        [NonSerialized]
        private Dictionary<string, long> _systemIds = new Dictionary<string, long>();
        public Dictionary<string, long> SystemIds { get { return this._systemIds; } set { this._systemIds = value; } }

        //static MarketConfiguration()
        //{
        //    LoadConfig();
        //}

        //public static void LoadConfig()
        //{
        //    this.SystemIds = SolarSystem.LoadFromCsv(Path.Combine(Configuration.AppConfigAppConfig.ActualConfigFolder, Configuration.AppConfigAppConfig.SystemIdsFileName));
        //    this.Type GetTypeIds();

        //    XDocument xml = new XDocument();

        //    try
        //    {
        //        xml = XDocument.Load(Configuration.AppConfigAssemblyPath + "\\marketConfig.xml");
        //    }
        //    catch (FileNotFoundException)
        //    {
        //        xml = MarketConfiguration.AppConfigCreateDefaultConfig();
        //        xml.Save(Configuration.AppConfigAssemblyPath + "\\marketConfig.xml");
        //    }

        //    MarketConfiguration.AppConfigBrokersFeePercent = decimal.Parse(xml.Root.Element("brokersFee").Attribute("percentValue").Value, Configuration.AppConfigEnCultureInfo);
        //    MarketConfiguration.AppConfigTransactionTaxPercent = decimal.Parse(xml.Root.Element("transactionTax").Attribute("percentValue").Value, Configuration.AppConfigEnCultureInfo);

        //}

        //public static XDocument CreateDefaultConfig()
        //{
        //    XDocument xml = new XDocument
        //        (
        //        new XElement
        //            (
        //            "config",
        //                new XElement("brokersFee", new XAttribute("percentValue", 0.03M)),
        //                new XElement("transactionTax", new XAttribute("percentValue", 0.02M))
        //            )
        //        );

        //    return xml;
        //}

        
    }
}
