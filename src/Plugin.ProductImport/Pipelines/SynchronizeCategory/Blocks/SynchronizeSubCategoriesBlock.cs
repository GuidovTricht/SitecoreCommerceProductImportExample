using System.Linq;
using System.Threading.Tasks;
using Plugin.ProductImport.Pipelines.SynchronizeCategory.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Plugin.ProductImport.Pipelines.SynchronizeCategory.Blocks
{
    public class SynchronizeSubCategoriesBlock : PipelineBlock<SynchronizeCategoryArgument, SynchronizeCategoryArgument, CommercePipelineExecutionContext>
    {
        private readonly ISynchronizeCategoryPipeline _synchronizeCategoryPipeline;

        public SynchronizeSubCategoriesBlock(ISynchronizeCategoryPipeline synchronizeCategoryPipeline)
        {
            _synchronizeCategoryPipeline = synchronizeCategoryPipeline;
        }

        public override async Task<SynchronizeCategoryArgument> Run(SynchronizeCategoryArgument arg, CommercePipelineExecutionContext context)
        {
            if (arg.ImportCategory.SubCategories == null || !arg.ImportCategory.SubCategories.Any())
                return arg;

            foreach (var subCategory in arg.ImportCategory.SubCategories)
            {
                var syncResult = await _synchronizeCategoryPipeline.Run(new SynchronizeCategoryArgument()
                    {
                        Catalog = arg.Catalog,
                        ImportCategory = subCategory,
                        ParentEntityId = arg.Category.Id
                    },
                    context.CommerceContext.GetPipelineContextOptions());

                arg.Catalog = syncResult?.Catalog;
            }

            return arg;
        }
    }
}
