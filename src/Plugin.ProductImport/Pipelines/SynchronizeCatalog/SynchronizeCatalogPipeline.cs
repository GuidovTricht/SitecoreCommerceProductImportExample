using Microsoft.Extensions.Logging;
using Plugin.ProductImport.Pipelines.SynchronizeCatalog.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;

namespace Plugin.ProductImport.Pipelines.SynchronizeCatalog
{
    public class SynchronizeCatalogPipeline : CommercePipeline<SynchronizeCatalogArgument, CatalogContentArgument>, ISynchronizeCatalogPipeline
    {
        public SynchronizeCatalogPipeline(IPipelineConfiguration<ISynchronizeCatalogPipeline> configuration, ILoggerFactory loggerFactory) : base(configuration, loggerFactory)
        {
        }
    }
}
