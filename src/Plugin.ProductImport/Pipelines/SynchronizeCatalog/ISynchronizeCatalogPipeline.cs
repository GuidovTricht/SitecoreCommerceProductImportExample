using Plugin.ProductImport.Pipelines.SynchronizeCatalog.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;

namespace Plugin.ProductImport.Pipelines.SynchronizeCatalog
{
    public interface ISynchronizeCatalogPipeline : IPipeline<SynchronizeCatalogArgument, CatalogContentArgument, CommercePipelineExecutionContext>
    {
    }
}
