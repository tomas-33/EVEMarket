namespace TH.EveMarket.Library.Data
{
    using System.Collections.Generic;
    using TH.EveMarket.Library.Utility;
    public class SolarSystem
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public SolarSystem()
        {
        }

        public SolarSystem(long id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public static SolarSystem CreateSolarSystem(string name, ref Dictionary<string, long> systemIds)
        {
            long id;
            if (!systemIds.TryGetValue(name, out id))
            {
                id = DownloadSystemId(name);
                systemIds.Add(name, id);
            }

            return new SolarSystem(id, name);
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

        private static long DownloadSystemId(string systemName)
        {
            var eveOnlineApi = new EveOnlineApi(Configuration.AppConfig.EveOnlineApiUri);
            return eveOnlineApi.LoadSystemId(systemName);
        }
    }
}
