using Plugin.ProductImport.Pipelines.SynchronizeCategory.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.ProductImport.Pipelines.SynchronizeCategory
{
    public interface ISynchronizeCategoryPipeline : IPipeline<SynchronizeCategoryArgument, SynchronizeCategoryArgument, CommercePipelineExecutionContext>
    {
    }
}
