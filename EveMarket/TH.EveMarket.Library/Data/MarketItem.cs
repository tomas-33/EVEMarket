namespace TH.EveMarket.Library.Data
{
    public class MarketItem
    {
        public Route Route { get; set; }
        public Product Product { get; set; }
        public MarketData From { get; set; }
        public MarketData To { get; set; }
        public decimal TaxPlusFee { get; set; }
        public decimal Profit { get; set; }
        public decimal ProfitPercent { get; set; }
        public int Margin { get; set; }
        public decimal MedianSellDay { get; set; }
        public decimal AverageSellDay { get; set; }
        public string Note { get; set; }
    }
}
