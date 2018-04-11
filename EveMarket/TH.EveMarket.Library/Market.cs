namespace TH.EveMarket.Library
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using TH.EveMarket.Library.Data;
    using TH.EveMarket.Library.Utility;

    public class Market
    {
        private decimal _BrokersFee = 0.03M;
        private decimal _transactionTax = 0.02M;

        private Dictionary<string, long> _typeIds;
        private Dictionary<string, long> _systemIds;
        private System.Globalization.CultureInfo _enCultureInfo = new System.Globalization.CultureInfo("en-US");
        private List<Route> _routes;
        private List<Product> _products;
        private List<MarketData> _marketData;

        public List<MarketItem> MarketItems { get; private set; }

        public Market()
        {
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

        public void TestEveOnlineApi()
        {
            var api = new EveOnlineApi();
            System.Console.WriteLine(api.LoadSystemId("Dodixie"));
        }

        public void Test()
        {
            var marketerApi = new EveMarketerApi();
            this.LoadData();
            marketerApi.LoadMarketData(this._routes, this._products);
        }

        public void LoadData()
        {
            this.GetTypeIds();
            this.GetSystemIds();

            // Routes
            var systemRoutes = Csv.GetCsv(Configuration.RoutesPath);
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
            var products = Csv.GetCsv(Configuration.ProductsPath);
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
            var api = new EveMarketerApi(Configuration.EveMarketerApiUri);
            this._marketData = api.LoadMarketData(this._routes, this._products);
        }

        public void Calculate()
        {
            this.MarketItems = new List<MarketItem>();
            foreach (var route in this._routes)
            {
                foreach (var product in this._products)
                {
                    var newResult = new MarketItem();
                    newResult.Route = route;
                    newResult.Product = product;
                    newResult.From = this._marketData.Where(m => m.TypeId == product.Id && m.SystemId == route.FromSystem.Id).First();
                    newResult.To = this._marketData.Where(m => m.TypeId == product.Id && m.SystemId == route.ToSystem.Id).First();
                    newResult.TaxPlusFee = (this._BrokersFee + this._transactionTax) * newResult.From.Sell.Min;
                    newResult.Profit = newResult.From.Sell.Min + newResult.TaxPlusFee - newResult.To.Sell.Min;
                    newResult.ProfitPercent = (newResult.TaxPlusFee + newResult.To.Sell.Min) != 0 ? newResult.Profit / (newResult.TaxPlusFee + newResult.To.Sell.Min) : 0;
                    this.MarketItems.Add(newResult);
                }
            }
        }

        public string ShowItems()
        {
            StringBuilder result = new StringBuilder();
            foreach (var route in this._routes)
            {
                result.AppendLine($"{route.FromSystem.Name} -> {route.ToSystem.Name}");
                this.MarketItems.Where(r => r.Route == route).ToList().ForEach(r => result.AppendLine($"{r.Product.Name}    Origin sell: {r.From.Sell.Min} ISK / Destination Sell: {r.To.Sell.Min} ISK / Profit: {r.ProfitPercent.ToString("0.##")}%"));
            }

            return result.ToString();
        }

        private void GetTypeIds()
        {
            TypeIds typeIds = new TypeIds(Configuration.TypeIdsUri);
            this._typeIds = typeIds.Load(Configuration.TypeIdsPath);
            if (this._typeIds == null || this._typeIds.Count == 0)
            {
                this._typeIds = typeIds.DownloadTypeIds();
                typeIds.Save(this._typeIds, Configuration.TypeIdsPath);
            }
        }

        private void GetSystemIds()
        {
            this._systemIds = new Dictionary<string, long>();

            var systemIds = Csv.GetCsv(Configuration.SystemIdsPath);

            foreach (var item in systemIds)
            {
                this._systemIds.Add(item[0], long.Parse(item[1]));
            }
        }
    }
}
