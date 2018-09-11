using System.Linq;
using System.Threading.Tasks;
using Plugin.ProductImport.Pipelines.SynchronizeProduct.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Plugin.ProductImport.Pipelines.SynchronizeProduct.Blocks
{
    public class GetOrCreateProductBlock : PipelineBlock<SynchronizeProductArgument, SynchronizeProductArgument, CommercePipelineExecutionContext>
    {
        private readonly IDoesEntityExistPipeline _doesEntityExistPipeline;
        private readonly IGetSellableItemPipeline _getSellableItemPipeline;
        private readonly ICreateSellableItemPipeline _createSellableItemPipeline;

        public GetOrCreateProductBlock(IDoesEntityExistPipeline doesEntityExistPipeline, IGetSellableItemPipeline getSellableItemPipeline, ICreateSellableItemPipeline createSellableItemPipeline)
        {
            _doesEntityExistPipeline = doesEntityExistPipeline;
            _getSellableItemPipeline = getSellableItemPipeline;
            _createSellableItemPipeline = createSellableItemPipeline;
        }

        public override async Task<SynchronizeProductArgument> Run(SynchronizeProductArgument arg, CommercePipelineExecutionContext context)
        {
            Sitecore.Commerce.Plugin.Catalog.SellableItem product = null;
            var productId = arg.ImportProduct.ProductId.ProposeValidId()
                .EnsurePrefix(CommerceEntity.IdPrefix<Sitecore.Commerce.Plugin.Catalog.SellableItem>());

            if (await _doesEntityExistPipeline.Run(
                new FindEntityArgument(typeof(Sitecore.Commerce.Plugin.Catalog.SellableItem), productId),
                context.CommerceContext.GetPipelineContextOptions()))
            {
                product = await _getSellableItemPipeline.Run(new ProductArgument(arg.Catalog.Id, productId),
                    context.CommerceContext.GetPipelineContextOptions());
            }
            else
            {
                var productName = arg.ImportProduct.ProductName.FirstOrDefault()?.Name;
                var createResult = await _createSellableItemPipeline.Run(
                    new CreateSellableItemArgument(arg.ImportProduct.ProductId.ProposeValidId(), arg.ImportProduct.ProductId,
                        productName, ""), context.CommerceContext.GetPipelineContextOptions());
                product = createResult?.SellableItems?.FirstOrDefault(s => s.Id.Equals(productId));
            }

            Condition.Requires<Sitecore.Commerce.Plugin.Catalog.SellableItem>(product)
                .IsNotNull($"{this.Name}: The Product could not be created.");

            product.IsPersisted = true;
            arg.SellableItem = product;

            return arg;
        }
    }
}
