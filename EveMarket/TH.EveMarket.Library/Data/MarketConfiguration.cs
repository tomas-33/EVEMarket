namespace TH.EveMarket.Library.Data
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class MarketConfiguration
    {
        public decimal BrokersFeePercent { get; set; } = 0.03M;
        public decimal TransactionTaxPercent { get; set; } = 0.02M;

        [NonSerialized]
        private Dictionary<string, long> _typeIds = new Dictionary<string, long>();
        public Dictionary<string, long> TypeIds { get { return this._typeIds; } set { this._typeIds = value; } }

        [NonSerialized]
        private Dictionary<string, long> _systemIds = new Dictionary<string, long>();
        public Dictionary<string, long> SystemIds { get { return this._systemIds; } set { this._systemIds = value; } }       
    }
}
