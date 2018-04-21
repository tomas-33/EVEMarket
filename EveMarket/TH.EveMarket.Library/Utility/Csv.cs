namespace TH.EveMarket.Library.Utility
{
    using System.Collections.Generic;
    using System.IO;

    public static class Csv
    {
        public static List<string[]> GetCsv(string path, char separator = ';')
        {
            var result = new List<string[]>();
            using (StreamReader reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    result.Add(reader.ReadLine().Split(separator));
                }
            }

            return result;
        }

        public static void SaveCsv(List<string[]> items, string path, char separator = ';')
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                foreach (var item in items)
                {
                    if (item.Length == 1)
                    {
                        sw.WriteLine(item[0]);
                    }
                    else
                    {
                        for (int i = 0; i < item.Length; i++)
                        {
                            if (i == item.Length - 1)
                            {
                                sw.Write($"{item[i]}");
                            }
                            else
                            {
                                sw.Write($"{item[i]};");
                            }
                        }

                        sw.WriteLine();
                    }
                }
            }
        }
    }
}
