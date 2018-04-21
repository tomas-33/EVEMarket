namespace TH.EveMarket.Library.Data
{
    using System;

    [Serializable]
    public class MarketData
    {
        public long TypeId { get; set; }
        public long SystemId { get; set; }
        public Data Buy { get; set; } = new Data();
        public Data Sell { get; set; } = new Data();
        public Data All { get; set; } = new Data();

        [Serializable]
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
