namespace TH.EveMarket.Library.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class Convert<T>
    {
        public static ObservableCollection<T> ListToObservableCollection(List<T> items)
        {
            var oc = new ObservableCollection<T>();
            items.ForEach(i => oc.Add(i));
            return oc;
        }
    }
}
