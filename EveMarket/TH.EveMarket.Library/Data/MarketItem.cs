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
        private Product _product = new Product();
        private MarketData _from = new MarketData();
        private MarketData _to = new MarketData();
        private decimal _tax;
        private decimal _fee;
        private decimal _profit;
        private decimal _profitPercent;
        private int _margin;
        private decimal _medianSellDay;
        private decimal _averageSellDay;
        private string _note = string.Empty;

        #region Properties
        public Route Route
        {
            get { return this._route; }
            set
            {
                this._route = value;
                OnPropertyChanged(nameof(this.Route));
            }
        }

        public Product Product
        {
            get { return this._product; }
            set
            {
                this._product = value;
                OnPropertyChanged(nameof(this.Product));
            }
        }

        public MarketData From
        {
            get { return this._from; }
            set
            {
                this._from = value;
                OnPropertyChanged(nameof(this.From));
            }
        }

        public MarketData To
        {
            get { return this._to; }
            set
            {
                this._to = value;
                OnPropertyChanged(nameof(this.To));
            }
        }

        public decimal Tax
        {
            get { return _tax; }
            set
            {
                _tax = value;
                OnPropertyChanged(nameof(this.Tax));
                OnPropertyChanged(nameof(this.TaxPlusFee));
            }
        }

        public decimal Fee
        {
            get { return _fee; }
            set
            {
                _fee = value;
                OnPropertyChanged(nameof(this.Fee));
                OnPropertyChanged(nameof(this.TaxPlusFee));
            }
        }

        public decimal TaxPlusFee => this._tax + this._fee;

        public decimal Profit
        {
            get { return this._profit; }
            set
            {
                this._profit = value;
                OnPropertyChanged(nameof(this.Profit));
            }
        }

        public decimal ProfitPercent
        {
            get { return this._profitPercent; }
            set
            {
                this._profitPercent = value;
                OnPropertyChanged(nameof(this.ProfitPercent));
            }
        }

        public int Margin
        {
            get { return this._margin; }
            set
            {
                this._margin = value;
                OnPropertyChanged(nameof(this.Margin));
            }
        }

        public decimal MedianSellDay
        {
            get { return this._medianSellDay; }
            set
            {
                this._medianSellDay = value;
                OnPropertyChanged(nameof(this.MedianSellDay));
            }
        }

        public decimal AverageSellDay
        {
            get { return this._averageSellDay; }
            set
            {
                this._averageSellDay = value;
                OnPropertyChanged(nameof(AverageSellDay));
            }
        }

        public string Note
        {
            get { return this._note; }
            set
            {
                this._note = value;
                OnPropertyChanged(nameof(this.Note));
            }
        }

        public bool IsWorthIt => this._profitPercent > 0.05M;

        #endregion // Properties

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
                item.Profit = item.From.Sell.Min + item.TaxPlusFee - item.To.Sell.Min;
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
