namespace TH.EveMarket.Library.Data
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Serializable]
    public class MarketItems : ObservableCollection<MarketItem>
    {
        public DateTime LastUpdated { get; set; }
    }
}
