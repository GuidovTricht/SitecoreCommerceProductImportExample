using Sitecore.Commerce.Plugin.Catalog;

namespace Plugin.ProductImport.Pipelines.SynchronizeCategory.Arguments
{
    public class SynchronizeCategoryArgument : CatalogContentArgument
    {
        public Models.Category ImportCategory { get; set; }
        public string ParentEntityId { get; set; }
        public Category Category { get; set; }
    }
}
