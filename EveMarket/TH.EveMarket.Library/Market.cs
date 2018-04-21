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

        public MarketItems MarketItems { get; private set; } = new MarketItems();
        public ObservableCollection<Product> Products { get; private set; } = new ObservableCollection<Product>();
        public ObservableCollection<Route> Routes { get; private set; } = new ObservableCollection<Route>();

        public Dictionary<string, List<string>> ProductsFilterList { get; set; }

        #endregion // Properties

        public Market()
        {
            this.Products.CollectionChanged += ProductsChanged;
        }

        public void LoadData()
        {
            this.Routes = Convert<Route>.ListToObservableCollection(Route.LoadFromCsv(Configuration.AppConfig.RoutesFileName, Configuration.SystemIds));
            this.Products = Convert<Product>.ListToObservableCollection(Product.LoadFromCsv(Configuration.AppConfig.ProductsFileName, Configuration.TypeIds));
            this.ProductsFilterList = this.CreateFilterList(this.Products.Select(p => p.Name).ToList());

            this.MarketItems = new MarketItems();
            var marketItems = MarketItem.Load(this.Routes.ToList(), this.Products.ToList());
            foreach (var item in marketItems)
            {
                this.MarketItems.Add(item);
            }
        }

        public void SaveData()
        {
            Route.SaveToCsv(Path.Combine(Configuration.AppConfig.ActualConfigFolder, "Routes.csv"), this.Routes.ToList());
            Product.SaveToCsv(Path.Combine(Configuration.AppConfig.ActualConfigFolder, "Products.csv"), this.Products.ToList());
        }

        public void DownloadMarketData()
        {
            var marketData = EveMarketerApi.LoadMarketData(this.Routes.ToList(), this.Products.ToList(), Configuration.AppConfig.EveMarketerApiUri);
            MarketItem.LoadMarketData(this.MarketItems, marketData);
            MarketItems.LastUpdated = DateTime.Now;
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

        private Dictionary<string, List<string>> CreateFilterList(List<string> list)
        {
            var filterList = new Dictionary<string, List<string>>();
            foreach (var item in list)
            {
                AddOneWord(item);
            }

            return filterList;

            void AddOneWord(string word)
            {
                for (int i = 0; i < word.Length; i++)
                {
                    for (int j = 1; j <= word.Substring(i).Length; j++)
                    {
                        CheckOneWord(word, i, j);
                    }
                }
            }

            void CheckOneWord(string word, int startIndex, int length)
            {
                if (string.IsNullOrEmpty(word))
                {
                    return;
                }

                var checkKey = word.Substring(startIndex, length).ToLower();
                List<string> seznam;
                if (filterList.TryGetValue(checkKey, out seznam))
                {
                    if (seznam != null && !seznam.Contains(word))
                    {
                        seznam.Add(word);
                    }
                }
                else
                {
                    seznam = new List<string>() { word };
                    filterList.Add(checkKey, seznam);
                }
            }
        }  
    }
}
