namespace TH.EveMarket.Library.Data
{
    using System.Collections.Generic;
    using TH.EveMarket.Library.Utility;
    public class SolarSystem
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public static long DownloadSystemId(string systemName)
        {
            var eveOnlineApi = new EveOnlineApi(Configuration.EveOnlineApiUri);
            return eveOnlineApi.LoadSystemId(systemName);
        }

        public static Dictionary<string, long> LoadFromCsv(string path)
        {
            var systemIds = new Dictionary<string, long>();
            var systemIdsCsv = Csv.GetCsv(path);

            foreach (var item in systemIdsCsv)
            {
                systemIds.Add(item[0], long.Parse(item[1]));
            }

            return systemIds;
        }
    }
}
