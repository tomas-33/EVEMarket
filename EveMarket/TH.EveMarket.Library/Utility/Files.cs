namespace TH.EveMarket.Library.Utility
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    public static class Files<T>
    {
        public static T LoadXml(string path)
        {
            var xs = new XmlSerializer(typeof(T));
            T result;
            using (Stream sr = File.Open(path, FileMode.Open))
            {
                result = (T)xs.Deserialize(sr);
            }

            return result;
        }

        public static void SaveXml(string path, T instanceToSave)
        {
            var xs = new XmlSerializer(typeof(T));
            using (Stream s = File.Open(path, FileMode.Create))
            {
                xs.Serialize(s, instanceToSave);
            }
        }
    }
}
