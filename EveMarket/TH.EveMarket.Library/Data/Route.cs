﻿namespace TH.EveMarket.Library.Data
{
    using System;
    using System.Collections.Generic;
    using TH.EveMarket.Library.Utility;

    [Serializable]
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
                newRoute.FromSystem = SolarSystem.CreateSolarSystem(item[0], ref systemIds);
                newRoute.ToSystem = SolarSystem.CreateSolarSystem(item[1], ref systemIds);
                routes.Add(newRoute);
            }

            return routes;
        }

        public static void SaveToCsv(string path, List<Route> routes)
        {
            var routesToSave = new List<string[]>();
            
            foreach (var item in routes)
            {
                var route = new string[] { item.FromSystem.Name, item.ToSystem.Name };
                routesToSave.Add(route);
            }

            Csv.SaveCsv(routesToSave, path);
        }
    }
}
