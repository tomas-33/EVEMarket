namespace TH.EveMarket.Library.Data
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    [Serializable]
    public class MarketItem : BaseINotifyPropertyChanged
    {
        private Route _route = new Route();
        public Route Route
        {
            get { return this._route; }
            set
            {
                this._route = value;
                OnPropertyChanged(GetPropertyName(() => this.Route));
            }
        }

        private Product _product = new Product();
        public Product Product
        {
            get { return this._product; }
            set
            {
                this._product = value;
                OnPropertyChanged(GetPropertyName(() => this.Product));
            }
        }

        private MarketData _from = new MarketData();
        public MarketData From
        {
            get { return this._from; }
            set
            {
                this._from = value;
                OnPropertyChanged(GetPropertyName(() => this.From));
            }
        }

        private MarketData _to = new MarketData();
        public MarketData To
        {
            get { return this._to; }
            set
            {
                this._to = value;
                OnPropertyChanged(GetPropertyName(() => this.To));
            }
        }

        private decimal _tax;
        public decimal Tax
        {
            get { return _tax; }
            set
            {
                _tax = value;
                OnPropertyChanged(GetPropertyName(() => this.Tax));
                OnPropertyChanged(GetPropertyName(() => this.TaxPlusFee));
            }
        }

        private decimal _fee;
        public decimal Fee
        {
            get { return _fee; }
            set
            {
                _fee = value;
                OnPropertyChanged(GetPropertyName(() => this.Fee));
                OnPropertyChanged(GetPropertyName(() => this.TaxPlusFee));
            }
        }


        public decimal TaxPlusFee { get { return this._tax + this._fee; } }

        private decimal _profit;
        public decimal Profit
        {
            get { return this._profit; }
            set
            {
                this._profit = value;
                OnPropertyChanged(GetPropertyName(() => this.Profit));
            }
        }

        private decimal _profitPercent;
        public decimal ProfitPercent
        {
            get { return this._profitPercent; }
            set
            {
                this._profitPercent = value;
                OnPropertyChanged(GetPropertyName(() => this.ProfitPercent));
            }
        }

        private int _margin;
        public int Margin
        {
            get { return this._margin; }
            set
            {
                this._margin = value;
                OnPropertyChanged(GetPropertyName(() => this.Margin));
            }
        }

        private decimal _medianSellDay;
        public decimal MedianSellDay
        {
            get { return this._medianSellDay; }
            set
            {
                this._medianSellDay = value;
                OnPropertyChanged(GetPropertyName(() => this.MedianSellDay));
            }
        }

        private decimal _averageSellDay;
        public decimal AverageSellDay
        {
            get { return this._averageSellDay; }
            set
            {
                this._averageSellDay = value;
                OnPropertyChanged(GetPropertyName(() => this.AverageSellDay));
            }
        }

        private string _note = string.Empty;
        public string Note
        {
            get { return this._note; }
            set
            {
                this._note = value;
                OnPropertyChanged(GetPropertyName(() => this.Note));
            }
        }

        public static List<MarketItem> Load(List<Route> routes, List<Product> products)
        {
            var marketItems = new List<MarketItem>();
            foreach (var route in routes)
            {
                foreach (var product in products)
                {
                    var newResult = new MarketItem();
                    newResult.Route = route;
                    newResult.Product = product;
                    marketItems.Add(newResult);
                }
            }

            return marketItems;
        }

        public static void LoadMarketData(ObservableCollection<MarketItem> marketItems, List<MarketData> marketData)
        {
            foreach (MarketItem item in marketItems)
            {
                item.From = marketData.Where(m => m.TypeId == item.Product.Id && m.SystemId == item.Route.FromSystem.Id).First();
                item.To = marketData.Where(m => m.TypeId == item.Product.Id && m.SystemId == item.Route.ToSystem.Id).First();
                item.Tax = Configuration.MarketConfiguration.TransactionTaxPercent * item.To.Sell.Min;
                item.Fee = Configuration.MarketConfiguration.BrokersFeePercent * item.To.Sell.Min;
                item.Profit = item.To.Sell.Min + item.TaxPlusFee - item.To.Sell.Min;
                item.ProfitPercent = (item.TaxPlusFee + item.To.Sell.Min) != 0 ? item.Profit / (item.TaxPlusFee + item.To.Sell.Min) : 0;
            }
        }

        public static void AddProducts(ObservableCollection<MarketItem> marketItems, List<Route> routes, List<Product> newProducts)
        {
            foreach (var newProduct in newProducts)
            {
                if (!marketItems.Where(i => i.Product == newProduct).Any())
                {
                    foreach (var route in routes)
                    {
                        var newItem = new MarketItem();
                        newItem.Route = route;
                        newItem.Product = newProduct;
                        marketItems.Add(newItem);
                    }
                }
            }
        }

        public static void RemoveProducts(ObservableCollection<MarketItem> marketItems, List<Route> routes, List<Product> oldProducts)
        {
            var itemsToRemove = new List<MarketItem>();
            foreach (var oldProduct in oldProducts)
            {
                itemsToRemove.AddRange(marketItems.Where(i => i.Product == oldProduct).Select(i => i));
            }

            foreach (var item in itemsToRemove)
            {
                marketItems.Remove(item);
            }
        }
    }
}
