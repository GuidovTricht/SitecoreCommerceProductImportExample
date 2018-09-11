using System.Linq;
using System.Threading.Tasks;
using Plugin.ProductImport.Pipelines.SynchronizeCategory.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Plugin.ProductImport.Pipelines.SynchronizeCategory.Blocks
{
    public class GetOrCreateCategoryBlock : PipelineBlock<SynchronizeCategoryArgument, SynchronizeCategoryArgument, CommercePipelineExecutionContext>
    {
        private readonly IDoesEntityExistPipeline _doesEntityExistPipeline;
        private readonly IGetCategoryPipeline _getCategoryPipeline;
        private readonly ICreateCategoryPipeline _createCategoryPipeline;

        public GetOrCreateCategoryBlock(IDoesEntityExistPipeline doesEntityExistPipeline, IGetCategoryPipeline getCategoryPipeline, ICreateCategoryPipeline createCategoryPipeline)
        {
            _doesEntityExistPipeline = doesEntityExistPipeline;
            _getCategoryPipeline = getCategoryPipeline;
            _createCategoryPipeline = createCategoryPipeline;
        }

        public override async Task<SynchronizeCategoryArgument> Run(SynchronizeCategoryArgument arg, CommercePipelineExecutionContext context)
        {
            Sitecore.Commerce.Plugin.Catalog.Category category = null;
            var categoryId = $"{CommerceEntity.IdPrefix<Category>()}{arg.Catalog.Name}-{arg.ImportCategory.CategoryId.ProposeValidId()}";

            if (await _doesEntityExistPipeline.Run(
                new FindEntityArgument(typeof(Sitecore.Commerce.Plugin.Catalog.Category), categoryId),
                context.CommerceContext.GetPipelineContextOptions()))
            {
                category = await _getCategoryPipeline.Run(new GetCategoryArgument(categoryId),
                    context.CommerceContext.GetPipelineContextOptions());
            }
            else
            {
                var createResult = await _createCategoryPipeline.Run(
                    new CreateCategoryArgument(arg.Catalog.Id, arg.ImportCategory.CategoryId.ProposeValidId(),
                        arg.ImportCategory.CategoryName, ""), context.CommerceContext.GetPipelineContextOptions());
                category = createResult?.Categories?.FirstOrDefault(c => c.Id.Equals(categoryId));
            }

            Condition.Requires<Sitecore.Commerce.Plugin.Catalog.Category>(category)
                .IsNotNull($"{this.Name}: The Category could not be created.");

            arg.Category = category;

            return arg;
        }
    }
}
