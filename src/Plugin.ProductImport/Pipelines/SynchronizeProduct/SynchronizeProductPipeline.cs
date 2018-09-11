using Microsoft.Extensions.Logging;
using Plugin.ProductImport.Pipelines.SynchronizeProduct.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.ProductImport.Pipelines.SynchronizeProduct
{
    public class SynchronizeProductPipeline : CommercePipeline<SynchronizeProductArgument, SynchronizeProductArgument>, ISynchronizeProductPipeline
    {
        public SynchronizeProductPipeline(IPipelineConfiguration<ISynchronizeProductPipeline> configuration, ILoggerFactory loggerFactory) : base(configuration, loggerFactory)
        {
        }
    }
}
