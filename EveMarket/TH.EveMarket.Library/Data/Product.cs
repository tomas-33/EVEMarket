namespace TH.EveMarket.Library.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TH.EveMarket.Library.Utility;

    [Serializable]
    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Sequence { get; set; }

        public Product()
        {
        }

        public Product(string name, long id, int sequence)
        {
            this.Name = name;
            this.Id = id;
            this.Sequence = sequence;
        }

        public static Product CreateProduct(string name, int sequence)
        {
            long id;
            if (Configuration.TypeIds.TryGetValue(name, out id))
            {
                return new Product(name, id, sequence);
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

        public static List<Product> LoadFromCsv(string path, Dictionary<string, long> typeIds)
        {
            var productsCsv = Csv.GetCsv(path);
            var products = new List<Product>();
            int i = 0;
            foreach (var item in productsCsv)
            {
                products.Add(Product.CreateProduct(item[0], i++));
            }

            return products;
        }

        public static void SaveToCsv(string path, List<Product> products)
        {
            var productsToSave = new List<string[]>();
            foreach (var item in products.OrderBy(p => p.Sequence))
            {
                var product = new string[] { item.Name };
                productsToSave.Add(product);
            }

            Csv.SaveCsv(productsToSave, path);
        }
    }
}
