namespace TH.EveMarket.Library.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using TH.EveMarket.Library.Data;

    class EveMarketerApi
    {
        private string _apiUri;
        private System.Globalization.CultureInfo _enCultureInfo = new System.Globalization.CultureInfo("en-US");

        public EveMarketerApi(string apiUri = null)
        {
            this._apiUri = !string.IsNullOrEmpty(apiUri) ? apiUri : "https://api.evemarketer.com/ec/marketstat";
        }

        public List<MarketData> LoadMarketData(List<Route> routes, List<Product> products)
        {
            var result = new List<MarketData>();
            products = products.Distinct().ToList();

            foreach (var systemId in this.GetDistinctSystemIds(routes))
            {
                result.AddRange(this.GetMarketData(products.Select(p => p.Id.ToString()).ToList(), systemId));
            }

            return result;
        }

        private List<MarketData> GetMarketData(List<string> typeIds, string systemId)
        {
            var system = long.Parse(systemId);
            var data = this.Parse(this.GetApiData(typeIds, systemId));
            data.ForEach(d => d.SystemId = system);
            return data;
        }

        private XmlDocument GetApiData(List<string> typeIds, string system = null, string regionLimit = null)
        {
            // Example
            // https://api.evemarketer.com/ec/marketstat?typeid=34&typeid=35&regionlimit=10000002&usesystem=30002659

            StringBuilder parameters = new StringBuilder();

            bool first = true;
            foreach (var item in typeIds)
            {
                if (first)
                {
                    first = false;
                    parameters.Append("typeid=" + item);
                }
                else
                {
                    parameters.Append("&typeid=" + item);
                }
            }

            if (!string.IsNullOrEmpty(regionLimit))
            {
                parameters.Append("&regionlimit=" + regionLimit);
            }

            if (!string.IsNullOrEmpty(system))
            {
                parameters.Append("&usesystem=" + system);
            }

            using (WebClient wc = new WebClient())
            {
                if (Configuration.UseDefaultProxy)
                {
                    IWebProxy wp = WebRequest.DefaultWebProxy;
                    wp.Credentials = CredentialCache.DefaultCredentials;
                    wc.Proxy = wp;
                }

                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string htmlResult = wc.UploadString(this._apiUri, parameters.ToString());
                XmlDocument result = new XmlDocument();
                result.LoadXml(htmlResult);
                return result;
            }
        }

        private List<MarketData> Parse(XmlDocument data)
        {
            var marketData = new List<MarketData>();

            var types = data.SelectNodes("//exec_api/marketstat/type");

            foreach (XmlNode type in types)
            {
                var item = new MarketData();
                item.TypeId = long.Parse(type.Attributes["id"].Value);
                item.Buy.Volume = ParseXmlLongValue(type, "//buy/volume");
                item.Buy.Avg = ParseXmlDecimalValue(type, "//buy/avg");
                item.Buy.Max = ParseXmlDecimalValue(type, "//buy/max");
                item.Buy.Min = ParseXmlDecimalValue(type, "//buy/min");
                item.Buy.StdDev = ParseXmlDecimalValue(type, "//buy/stddev");
                item.Buy.Median = ParseXmlDecimalValue(type, "//buy/median");
                item.Sell.Percentile = ParseXmlDecimalValue(type, "//sell/percentile");
                item.Sell.Volume = ParseXmlLongValue(type, "//sell/volume");
                item.Sell.Avg = ParseXmlDecimalValue(type, "//sell/avg");
                item.Sell.Max = ParseXmlDecimalValue(type, "//sell/max");
                item.Sell.Min = ParseXmlDecimalValue(type, "//sell/min");
                item.Sell.StdDev = ParseXmlDecimalValue(type, "//sell/stddev");
                item.Sell.Median = ParseXmlDecimalValue(type, "//sell/median");
                marketData.Add(item);
            }

            return marketData;
        }

        private decimal ParseXmlDecimalValue(XmlNode type, string node)
        {
            return decimal.Parse(string.IsNullOrEmpty(type.SelectSingleNode(node).InnerText) ? "0.0" : type.SelectSingleNode(node).InnerText, this._enCultureInfo);
        }

        private long ParseXmlLongValue(XmlNode type, string node)
        {
            return long.Parse(string.IsNullOrEmpty(type.SelectSingleNode(node).InnerText) ? "0.0" : type.SelectSingleNode(node).InnerText);
        }

        private List<string> GetDistinctSystemIds(List<Route> routes)
        {
            var distinctSystems = new List<string>();
            foreach (var item in routes)
            {
                if (!distinctSystems.Contains(item.FromSystem.Id.ToString()))
                {
                    distinctSystems.Add(item.FromSystem.Id.ToString());
                }

                if (!distinctSystems.Contains(item.ToSystem.Id.ToString()))
                {
                    distinctSystems.Add(item.ToSystem.Id.ToString());
                }
            }

            return distinctSystems;
        }
    }
}
