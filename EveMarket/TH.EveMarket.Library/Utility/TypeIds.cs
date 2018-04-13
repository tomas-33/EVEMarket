namespace TH.EveMarket.Library.Utility
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;

    public class TypeIds
    {
        private string _uri;

        public TypeIds(string uri)
        {
            this._uri = !string.IsNullOrEmpty(uri) ? uri : "http://eve-files.com/chribba/typeid.txt";
        }

        public Dictionary<string, long> DownloadTypeIds()
        {
            Dictionary<string, long> result = new Dictionary<string, long>();
            string getpage;

            using (WebClient wc = new WebClient())
            {
                if (Configuration.UseDefaultProxy)
                {
                    IWebProxy wp = WebRequest.DefaultWebProxy;
                    wp.Credentials = CredentialCache.DefaultCredentials;
                    wc.Proxy = wp;
                }
                
                getpage = wc.DownloadString(_uri);
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

        public void Save(Dictionary<string, long> typeIds, string path)
        {
            List<string[]> list = new List<string[]>();
            foreach (var item in typeIds)
            {
                list.Add(new string[] { item.Key, item.Value.ToString() });
            }

            Csv.SaveCsv(list, path);
        }

        public Dictionary<string, long> Load(string path)
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
    }
}
