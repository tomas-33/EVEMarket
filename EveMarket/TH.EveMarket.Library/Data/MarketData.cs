namespace TH.EveMarket.Library.Data
{
    public class MarketData
    {
        public long TypeId { get; set; }
        public long SystemId { get; set; }
        public Data Buy { get; private set; } = new Data();
        public Data Sell { get; private set; } = new Data();
        public Data All { get; private set; } = new Data();

        public class Data
        {
            public long Volume { get; set; }
            public decimal Avg { get; set; }
            public decimal Max { get; set; }
            public decimal Min { get; set; }
            public decimal StdDev { get; set; }
            public decimal Median { get; set; }
            public decimal Percentile { get; set; }
        }
    }
}
