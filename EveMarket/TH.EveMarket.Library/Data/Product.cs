namespace TH.EveMarket.Library.Data
{
    using System.Collections.Generic;
    using TH.EveMarket.Library.Utility;

    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public Product()
        {
        }

        public Product(string name, long id)
        {
            this.Name = name;
            this.Id = id;
        }

        public static Product CreateProduct(string name)
        {
            long id;
            if (Configuration.MarketConfiguration.TypeIds.TryGetValue(name, out id))
            {
                return new Product(name, id);
            }
            else
            {
                throw new System.Exception($"Product \"{name}\" not found in TypeIds.");
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Product) || (obj as Product).Name == null)
            {
                return false;
            }

            return (obj as Product).Id == this.Id && (obj as Product).Name == this.Name;
        }

        public static bool operator ==(Product obj1, Product obj2)
        {
            if (obj1 == null || obj2 == null)
            {
                return false;
            }

            return obj1.Id == obj2.Id && obj1.Name == obj2.Name;
        }

        public static bool operator !=(Product obj1, Product obj2)
        {
            if (obj1 == null || obj2 == null)
            {
                return false;
            }

            return !(obj1.Id == obj2.Id && obj1.Name == obj2.Name);
        }

        public override int GetHashCode()
        {
            return (Id.ToString() + Name).GetHashCode();
        }

        internal static List<Product> LoadFromCsv(string path, Dictionary<string, long> typeIds)
        {
            var productsCsv = Csv.GetCsv(path);
            var products = new List<Product>();
            foreach (var item in productsCsv)
            {
                products.Add(Product.CreateProduct(item[0]));
            }

            return products;
        }
    }
}
