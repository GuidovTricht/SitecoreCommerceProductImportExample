using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.ProductImport.Pipelines.SynchronizeProduct.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;

namespace Plugin.ProductImport.Pipelines.SynchronizeProduct.Blocks
{
    public class AssociateWithCategoriesBlock : PipelineBlock<SynchronizeProductArgument, SynchronizeProductArgument, CommercePipelineExecutionContext>
    {
        private readonly IAssociateSellableItemToParentPipeline _associateSellableItemToParentPipeline;
        private readonly IDoesEntityExistPipeline _doesEntityExistPipeline;
        private readonly IFindEntityPipeline _findEntityPipeline;

        public AssociateWithCategoriesBlock(IAssociateSellableItemToParentPipeline associateSellableItemToParentPipeline, IDoesEntityExistPipeline doesEntityExistPipeline, IFindEntityPipeline findEntityPipeline)
        {
            _associateSellableItemToParentPipeline = associateSellableItemToParentPipeline;
            _doesEntityExistPipeline = doesEntityExistPipeline;
            _findEntityPipeline = findEntityPipeline;
        }

        public override async Task<SynchronizeProductArgument> Run(SynchronizeProductArgument arg, CommercePipelineExecutionContext context)
        {
            if (arg.SellableItem == null || arg.ImportProduct.Categories == null || !arg.ImportProduct.Categories.Any())
                return arg;

            foreach (var category in arg.ImportProduct.Categories)
            {
                var categoryId = $"{CommerceEntity.IdPrefix<Category>()}{arg.Catalog.Name}-{category.ProposeValidId()}";
                if (await _doesEntityExistPipeline.Run(
                    new FindEntityArgument(typeof(Sitecore.Commerce.Plugin.Catalog.Category), categoryId),
                    context.CommerceContext.GetPipelineContextOptions()))
                {
                    var associateResult = await _associateSellableItemToParentPipeline.Run(
                        new CatalogReferenceArgument(arg.Catalog.Id, categoryId, arg.SellableItem.Id),
                        context.CommerceContext.GetPipelineContextOptions());

                    arg.SellableItem = await _findEntityPipeline.Run(
                        new FindEntityArgument(typeof(Sitecore.Commerce.Plugin.Catalog.SellableItem),
                            arg.SellableItem.Id), context.CommerceContext.GetPipelineContextOptions()) as SellableItem ?? arg.SellableItem;
                }
            }

            return arg;
        }
    }
}
