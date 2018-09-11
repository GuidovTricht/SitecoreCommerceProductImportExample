using Plugin.ProductImport.Pipelines.SynchronizeProduct.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.ProductImport.Pipelines.SynchronizeProduct
{
    public interface ISynchronizeProductPipeline : IPipeline<SynchronizeProductArgument, SynchronizeProductArgument, CommercePipelineExecutionContext>
    {
    }
}
