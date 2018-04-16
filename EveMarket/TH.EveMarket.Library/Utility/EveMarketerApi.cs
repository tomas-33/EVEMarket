namespace TH.EveMarket.Library.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Linq;
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

        private XDocument GetApiData(List<string> typeIds, string system = null, string regionLimit = null)
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
                if (Configuration.AppConfig.UseDefaultProxy)
                {
                    IWebProxy wp = WebRequest.DefaultWebProxy;
                    wp.Credentials = CredentialCache.DefaultCredentials;
                    wc.Proxy = wp;
                }

                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string htmlResult = wc.UploadString(this._apiUri, parameters.ToString());
                return XDocument.Parse(htmlResult);
            }
        }

        private List<MarketData> Parse(XDocument data)
        {
            var marketData = new List<MarketData>();

            var types = data.Root.Element("marketstat").Elements("type");

            foreach (var type in types)
            {
                var item = new MarketData();
                item.TypeId = long.Parse(type.Attribute("id").Value);
                item.Sell.Percentile = decimal.Parse(type.Element("buy").Element("percentile").Value, this._enCultureInfo);
                item.Buy.Volume = long.Parse(type.Element("buy").Element("volume").Value, this._enCultureInfo);
                item.Buy.Avg = decimal.Parse(type.Element("buy").Element("avg").Value, this._enCultureInfo);
                item.Buy.Max = decimal.Parse(type.Element("buy").Element("max").Value, this._enCultureInfo);
                item.Buy.Min = decimal.Parse(type.Element("buy").Element("min").Value, this._enCultureInfo);
                item.Buy.StdDev = decimal.Parse(type.Element("buy").Element("stddev").Value, this._enCultureInfo);
                item.Buy.Median = decimal.Parse(type.Element("buy").Element("median").Value, this._enCultureInfo);
                item.Sell.Percentile = decimal.Parse(type.Element("sell").Element("percentile").Value, this._enCultureInfo);
                item.Sell.Volume = long.Parse(type.Element("sell").Element("volume").Value, this._enCultureInfo);
                item.Sell.Avg = decimal.Parse(type.Element("sell").Element("avg").Value, this._enCultureInfo);
                item.Sell.Max = decimal.Parse(type.Element("sell").Element("max").Value, this._enCultureInfo);
                item.Sell.Min = decimal.Parse(type.Element("sell").Element("min").Value, this._enCultureInfo);
                item.Sell.StdDev = decimal.Parse(type.Element("sell").Element("stddev").Value, this._enCultureInfo);
                item.Sell.Median = decimal.Parse(type.Element("sell").Element("median").Value, this._enCultureInfo);
                marketData.Add(item);
            }

            return marketData;
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
