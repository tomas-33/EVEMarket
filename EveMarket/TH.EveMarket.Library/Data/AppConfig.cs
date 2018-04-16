namespace TH.EveMarket.Library.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Serializable]
    public class AppConfig
    {
        public string TypeIdsUri { get; set; } = "http://eve-files.com/chribba/typeid.txt";
        public string EveMarketerApiUri { get; set; } = "https://api.evemarketer.com/ec/marketstat";
        public string EveOnlineApiUri { get; set; } = "https://api.eveonline.com/eve/CharacterID.xml.aspx";
        public bool UseDefaultProxy { get; set; } = false;
        public string RoutesFileName { get; set; } = "Routes.csv";
        public string ProductsFileName { get; set; } = "Products.csv";
        public string TypeIdsFileName { get; set; } = "TypeIds.csv";
        public string SystemIdsFileName { get; set; } = "SystemIds.csv";

        public System.Globalization.CultureInfo EnCultureInfo { get; } = new System.Globalization.CultureInfo("en-US");

        [NonSerialized]
        private string _actualConfigFolder;

        public string ActualConfigFolder
        {
            get { return _actualConfigFolder; }
            set { _actualConfigFolder = value; }
        }

    }
}
