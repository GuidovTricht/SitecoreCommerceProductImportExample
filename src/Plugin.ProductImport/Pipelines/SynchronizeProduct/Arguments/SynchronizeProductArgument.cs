using Sitecore.Commerce.Plugin.Catalog;

namespace Plugin.ProductImport.Pipelines.SynchronizeProduct.Arguments
{
    public class SynchronizeProductArgument : CatalogContentArgument
    {
        public ProductImport.Models.Product ImportProduct { get; set; }
        public SellableItem SellableItem { get; set; }
    }
}
