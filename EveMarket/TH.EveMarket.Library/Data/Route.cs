namespace TH.EveMarket.Library.Data
{
    using System.Collections.Generic;
    using TH.EveMarket.Library.Utility;

    public class Route
    {
        public SolarSystem FromSystem { get; set; } = new SolarSystem();
        public SolarSystem ToSystem { get; set; } = new SolarSystem();

        public string Description { get { return this.ToString(); } }

        public override string ToString()
        {
            return $"{this.FromSystem.Name} -> {this.ToSystem.Name}";
        }

        public static List<Route> LoadFromCsv(string path, Dictionary<string, long> systemIds)
        {
            var systemRoutes = Csv.GetCsv(path);
            var routes = new List<Route>();
            foreach (var item in systemRoutes)
            {
                var newRoute = new Route();
                newRoute.FromSystem.Name = item[0];
                newRoute.FromSystem.Id = GetSystemId(item[0], systemIds);
                newRoute.ToSystem.Name = item[1];
                newRoute.ToSystem.Id = GetSystemId(item[1], systemIds);
                routes.Add(newRoute);
            }

            return routes;
        }

        private static long GetSystemId(string systemName, Dictionary<string, long> systemIds)
        {
            long systemId;
            var isSystemInList = systemIds.TryGetValue(systemName, out systemId);
            if (isSystemInList)
            {
                return systemId;
            }
            else
            {
                return SolarSystem.DownloadSystemId(systemName);
            }
        }
    }
}
