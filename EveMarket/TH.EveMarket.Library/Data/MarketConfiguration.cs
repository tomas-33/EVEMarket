namespace TH.EveMarket.Library.Data
{
    using System;

    [Serializable]
    public class MarketConfiguration
    {
        public decimal BrokersFeePercent { get; set; } = 0.03M;
        public decimal TransactionTaxPercent { get; set; } = 0.02M;
    }
}
