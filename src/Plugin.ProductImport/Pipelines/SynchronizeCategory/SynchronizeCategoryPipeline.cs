using Microsoft.Extensions.Logging;
using Plugin.ProductImport.Pipelines.SynchronizeCategory.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.ProductImport.Pipelines.SynchronizeCategory
{
    public class SynchronizeCategoryPipeline : CommercePipeline<SynchronizeCategoryArgument, SynchronizeCategoryArgument>, ISynchronizeCategoryPipeline
    {
        public SynchronizeCategoryPipeline(IPipelineConfiguration<ISynchronizeCategoryPipeline> configuration, ILoggerFactory loggerFactory) : base(configuration, loggerFactory)
        {
        }
    }
}
