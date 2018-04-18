namespace TH.EveMarket.Library.Utility
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;

    public static class TypeIds
    {
        public static Dictionary<string, long> DownloadTypeIds(string uri)
        {
            Dictionary<string, long> result = new Dictionary<string, long>();
            string getpage;

            using (WebClient wc = new WebClient())
            {
                if (Configuration.AppConfig.UseDefaultProxy)
                {
                    IWebProxy wp = WebRequest.DefaultWebProxy;
                    wp.Credentials = CredentialCache.DefaultCredentials;
                    wc.Proxy = wp;
                }

                getpage = wc.DownloadString(uri);
            }

            string[] list = getpage.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in list)
            {
                long value;
                if (item.Length < 13)
                {
                    continue;
                }

                string key = item.Substring(12).TrimEnd();

                if (long.TryParse(item.Substring(0, 12).TrimEnd(), out value) && !result.ContainsKey(key))
                {
                    result.Add(key, value);
                }
            }

            return result;
        }

        public static void Save(Dictionary<string, long> typeIds, string path)
        {
            List<string[]> list = new List<string[]>();
            foreach (var item in typeIds)
            {
                list.Add(new string[] { item.Key, item.Value.ToString() });
            }

            Csv.SaveCsv(list, path);
        }

        public static Dictionary<string, long> Load(string path)
        {
            Dictionary<string, long> typeIds = new Dictionary<string, long>();

            if (!File.Exists(path))
            {
                return typeIds;
            }

            foreach (var item in Csv.GetCsv(path))
            {
                typeIds.Add(item[0], long.Parse(item[1]));
            }

            return typeIds;
        }

        public static Dictionary<string, long> GetTypeIds()
        {
            var typeIds = new Dictionary<string, long>();
            typeIds = Load(Path.Combine(Configuration.AppConfig.ActualConfigFolder, Configuration.AppConfig.TypeIdsFileName));
            if (typeIds == null || typeIds.Count == 0)
            {
                typeIds = DownloadTypeIds(Configuration.AppConfig.TypeIdsUri);
                Save(typeIds, Configuration.AppConfig.TypeIdsFileName);
            }

            return typeIds;
        }
    }
}
