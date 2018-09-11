using Sitecore.Commerce.Plugin.Catalog;

namespace Plugin.ProductImport.Pipelines.SynchronizeCatalog.Arguments
{
    public class SynchronizeCatalogArgument : CatalogContentArgument
    {
        public Plugin.ProductImport.Models.Catalog ImportCatalog { get; set; }
    }
}
