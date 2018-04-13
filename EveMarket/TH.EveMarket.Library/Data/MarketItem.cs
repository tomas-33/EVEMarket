namespace TH.EveMarket.Library.Data
{
    using System.Collections.Generic;

    public class MarketItem
    {
        public Route Route { get; set; }
        public Product Product { get; set; }
        public MarketData From { get; set; }
        public MarketData To { get; set; }
        public decimal TaxPlusFee { get; set; } = default(decimal);
        public decimal Profit { get; set; } = default(decimal);
        public decimal ProfitPercent { get; set; } = default(decimal);
        public int Margin { get; set; } = default(int);
        public decimal MedianSellDay { get; set; } = default(decimal);
        public decimal AverageSellDay { get; set; } = default(decimal);
        public string Note { get; set; } = string.Empty;


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
                    newResult.From = new MarketData();
                    newResult.To = new MarketData();
                    marketItems.Add(newResult);
                }
            }

            return marketItems;
        }
    }
}
