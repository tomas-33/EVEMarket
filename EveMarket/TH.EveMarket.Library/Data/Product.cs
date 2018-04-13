namespace TH.EveMarket.Library.Data
{
    using System.Collections.Generic;
    using TH.EveMarket.Library.Utility;

    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public static List<Product> LoadFromCsv(string path, Dictionary<string, long> typeIds)
        {
            var productsCsv = Csv.GetCsv(path);
            var products = new List<Product>();
            foreach (var item in productsCsv)
            {
                var newProduct = new Product();
                newProduct.Name = item[0];
                newProduct.Id = typeIds[item[0]];
                products.Add(newProduct);
            }

            return products;
        }
    }
}
