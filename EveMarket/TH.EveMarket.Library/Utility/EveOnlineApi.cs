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

    public class EveOnlineApi
    {
        private string _apiUri;
        private System.Globalization.CultureInfo _enCultureInfo = new System.Globalization.CultureInfo("en-US");

        public EveOnlineApi(string apiUri = null)
        {
            this._apiUri = !string.IsNullOrEmpty(apiUri) ? apiUri : "https://api.eveonline.com/eve/CharacterID.xml.aspx";
        }

        public long LoadSystemId(string systemName)
        {
            if (string.IsNullOrEmpty(systemName))
            {
                throw new Exception("SystemName is empty.");
            }

            using (WebClient wc = new WebClient())
            {
                if (Configuration.AppConfig.UseDefaultProxy)
                {
                    IWebProxy wp = WebRequest.DefaultWebProxy;
                    wp.Credentials = CredentialCache.DefaultCredentials;
                    wc.Proxy = wp;
                }

                string result = wc.DownloadString($"{this._apiUri}?names={systemName.ToLower()}");
                XDocument xml = new XDocument();
                xml = XDocument.Parse(result);
                var name = xml.Root.Element("result").Element("rowset").Element("row").Attribute("name").Value;
                if (systemName.ToLower() != name.ToLower())
                {
                    throw new Exception("Wrong system ID was returned.");
                }

                var id = long.Parse(xml.Root.Element("result").Element("rowset").Element("row").Attribute("characterID").Value);
                return id;
            }
        }
    }
}
