namespace TH.EveMarket.Library.Utility
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
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

        public static T LoadBinary(string path)
        {
            var bf = new BinaryFormatter();
            T result;
            using (Stream sr = File.Open(path, FileMode.Open))
            {
                result = (T)bf.Deserialize(sr);
            }

            return result;
        }

        public static void SaveBinary(string path, T instanceToSave)
        {
            var bf = new BinaryFormatter();
            using (Stream s = File.Open(path, FileMode.Create))
            {
                bf.Serialize(s, instanceToSave);
            }
        }
    }
}
