using System.Collections.Generic;

namespace Plugin.ProductImport.Models
{
    public class Category
    {
        public string CategoryName { get; set; }
        public string CategoryId { get; set; }
        public List<Category> SubCategories { get; set; }
    }

    public class ProductName
    {
        public string Language { get; set; }
        public string Name { get; set; }
    }

    public class ValueItem
    {
        public string Language { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class ProductProperty
    {
        public string PropertyId { get; set; }
        public List<ValueItem> Values { get; set; }
    }

    public class Product
    {
        public string ProductId { get; set; }
        public List<string> Categories { get; set; }
        public List<ProductName> ProductName { get; set; }
        public string Brand { get; set; }
        public List<ProductProperty> ProductProperties { get; set; }
        public List<Product> Variants { get; set; }
        public List<string> Images { get; set; }
    }

    public class Catalog
    {
        public string CatalogName { get; set; }
        public string CatalogId { get; set; }
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
    }
}
