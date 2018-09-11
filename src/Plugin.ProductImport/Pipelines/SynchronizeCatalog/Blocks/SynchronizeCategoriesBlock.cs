using System.Threading.Tasks;
using Plugin.ProductImport.Pipelines.SynchronizeCatalog.Arguments;
using Plugin.ProductImport.Pipelines.SynchronizeCategory;
using Plugin.ProductImport.Pipelines.SynchronizeCategory.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.ProductImport.Pipelines.SynchronizeCatalog.Blocks
{
    public class SynchronizeCategoriesBlock : PipelineBlock<SynchronizeCatalogArgument, SynchronizeCatalogArgument, CommercePipelineExecutionContext>
    {
        private readonly ISynchronizeCategoryPipeline _synchronizeCategoryPipeline;

        public SynchronizeCategoriesBlock(ISynchronizeCategoryPipeline synchronizeCategoryPipeline)
        {
            _synchronizeCategoryPipeline = synchronizeCategoryPipeline;
        }

        public override async Task<SynchronizeCatalogArgument> Run(SynchronizeCatalogArgument arg, CommercePipelineExecutionContext context)
        {
            foreach (var category in arg.ImportCatalog.Categories)
            {
                var syncResult = await _synchronizeCategoryPipeline.Run(
                    new SynchronizeCategoryArgument()
                    {
                        ImportCategory = category,
                        ParentEntityId = arg.Catalog.Id,
                        Catalog = arg.Catalog
                    }, context.CommerceContext.GetPipelineContextOptions());
                
                arg.Catalog = syncResult.Catalog;
            }

            return arg;
        }
    }
}
