namespace TH.EveMarket.Library
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using TH.EveMarket.Library.Data;
    using TH.EveMarket.Library.Utility;

    public class Market
    {
        #region Properties

        public MarketItems MarketItems { get; private set; }
        public ObservableCollection<Product> Products { get; private set; }
        public ObservableCollection<Route> Routes { get; private set; }

        #endregion // Properties

        public Market()
        {
            this.LoadData();
            
            Products.CollectionChanged += ProductsChanged;
        }

        public void LoadData()
        {
            this.Routes = Convert<Route>.ListToObservableCollection(Route.LoadFromCsv(Configuration.AppConfig.RoutesFileName, Configuration.MarketConfiguration.SystemIds));
            this.Products = Convert<Product>.ListToObservableCollection(Product.LoadFromCsv(Configuration.AppConfig.ProductsFileName, Configuration.MarketConfiguration.TypeIds));

            this.MarketItems = new MarketItems();
            var marketItems = MarketItem.Load(this.Routes.ToList(), this.Products.ToList());
            foreach (var item in marketItems)
            {
                this.MarketItems.Add(item);
            }
        }

        public void DownloadMarketData()
        {
            var api = new EveMarketerApi(Configuration.AppConfig.EveMarketerApiUri);
            var marketData = api.LoadMarketData(this.Routes.ToList(), this.Products.ToList());
            MarketItem.LoadMarketData(this.MarketItems, marketData);
        }

        private void ProductsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                var newItems = new List<Product>();
                foreach (var item in e.NewItems)
                {
                    newItems.Add(item as Product);
                }

                MarketItem.AddProducts(this.MarketItems, this.Routes.ToList(), newItems);
            }

            if (e.OldItems != null)
            {
                var oldItems = new List<Product>();
                foreach (var item in e.OldItems)
                {
                    oldItems.Add(item as Product);
                }

                MarketItem.RemoveProducts(this.MarketItems, this.Routes.ToList(), oldItems);
            }
        }
    }
}
