namespace TH.EveMarket.Library
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using TH.EveMarket.Library.Data;
    using TH.EveMarket.Library.Utility;

    public class Market
    {
        private decimal _BrokersFee = 0.03M;
        private decimal _transactionTax = 0.02M;
        private Configuration _config;

        private Dictionary<string, long> _typeIds;
        private Dictionary<string, long> _systemIds;
        private System.Globalization.CultureInfo _enCultureInfo = new System.Globalization.CultureInfo("en-US");
        private List<Route> _routes;
        private List<Product> _products;
        private List<MarketData> _marketData;
        private List<MarketItem> _marketItems;

        public Market()
        {
            this.LoadConfig();
            this.LoadData();
        }


        #region Properties

        public Dictionary<string, long> TypeIds
        {
            get
            {
                if (this._typeIds == null)
                {
                    this.GetTypeIds();
                }

                return this._typeIds;
            }
        }

        public Dictionary<string, long> SystemIds
        {
            get
            {
                if (this._systemIds == null)
                {
                    this.GetSystemIds();
                }

                return this._systemIds;
            }
        }

        #endregion // Properties

        public void LoadConfig()
        {
            this._config = new Configuration();
            this._config.AssemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            this._config.RoutesPath = this._config.AssemblyPath + "\\Routes.txt";
            this._config.ProductsPath = this._config.AssemblyPath + "\\Products.txt";
            this._config.SystemIdsPath = this._config.AssemblyPath + "\\SystemIds.txt";
            this._config.TypeIdsPath = this._config.AssemblyPath + "\\typeids.txt";

            this._config.TypeIdsUri = "http://eve-files.com/chribba/typeid.txt";
            this._config.ApiUri = "http://api.eve-central.com/api/marketstat";

            this.GetTypeIds();
            this.GetSystemIds();
        }

        public void LoadData()
        {
            // Routes
            var systemRoutes = Csv.GetCsv(this._config.RoutesPath);
            this._routes = new List<Route>();
            foreach (var item in systemRoutes)
            {
                var newRoute = new Route();
                newRoute.FromSystem.Name = item[0];
                newRoute.FromSystem.Id = SystemIds[item[0]];
                newRoute.ToSystem.Name = item[1];
                newRoute.ToSystem.Id = SystemIds[item[1]];
                this._routes.Add(newRoute);
            }

            // Products
            var products = Csv.GetCsv(this._config.ProductsPath);
            this._products = new List<Product>();
            foreach (var item in products)
            {
                var newProduct = new Product();
                newProduct.Name = item[0];
                newProduct.Id = TypeIds[item[0]];
                this._products.Add(newProduct);
            }
        }

        public void DownloadMarketData()
        {
            var api = new EveCentralApi(this._config.ApiUri);
            this._marketData = api.LoadMarketData(this._routes, this._products);
        }

        public void Calculate()
        {
            this._marketItems = new List<MarketItem>();
            foreach (var route in this._routes)
            {
                foreach (var product in this._products)
                {
                    var newResult = new MarketItem();
                    newResult.Route = route;
                    newResult.Product = product;
                    newResult.From = this._marketData.Where(m => m.TypeId == product.Id && m.SystemId == route.FromSystem.Id).First();
                    newResult.To = this._marketData.Where(m => m.TypeId == product. Id && m.SystemId == route.ToSystem.Id).First();
                    newResult.TaxPlusFee = (this._BrokersFee + this._transactionTax) * newResult.From.Sell.Min;
                    newResult.Profit = newResult.From.Sell.Min + newResult.TaxPlusFee - newResult.To.Sell.Min;
                    //newResult.ProfitPercent = newResult.Profit / (newResult.TaxPlusFee + newResult.To.Sell.Min);
                    this._marketItems.Add(newResult);
                }
            }
        }

        public string ShowItems()
        {
            StringBuilder result = new StringBuilder();
            foreach (var route in this._routes)
            {
                result.AppendLine($"{route.FromSystem.Name} -> {route.ToSystem.Name}");
                this._marketItems.Where(r => r.Route == route).ToList().ForEach(r => result.AppendLine($"{r.Product.Name}    {r.From.Sell.Min / 1000000} {r.To.Sell.Min / 1000000} {r.ProfitPercent}"));
            }

            return result.ToString();
        }

        private void GetTypeIds()
        {
            TypeIds typeIds = new TypeIds(this._config.TypeIdsUri);
            this._typeIds = typeIds.Load(this._config.TypeIdsPath);
            if (this._typeIds == null || this._typeIds.Count == 0)
            {
                this._typeIds = typeIds.DownloadTypeIds();
                typeIds.Save(this._typeIds, this._config.TypeIdsPath);
            }
        }

        private void GetSystemIds()
        {
            this._systemIds = new Dictionary<string, long>();

            var systemIds = Csv.GetCsv(this._config.SystemIdsPath);

            foreach (var item in systemIds)
            {
                this._systemIds.Add(item[0], long.Parse(item[1]));
            }
        }
    }
}
