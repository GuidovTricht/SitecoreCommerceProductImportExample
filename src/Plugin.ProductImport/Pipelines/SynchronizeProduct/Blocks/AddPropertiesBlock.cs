using System.Linq;
using System.Threading.Tasks;
using Plugin.ProductImport.Pipelines.SynchronizeProduct.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;

namespace Plugin.ProductImport.Pipelines.SynchronizeProduct.Blocks
{
    public class AddPropertiesBlock : PipelineBlock<SynchronizeProductArgument, SynchronizeProductArgument, CommercePipelineExecutionContext>
    {
        private readonly IPersistEntityPipeline _persistEntityPipeline;
        private readonly IGetSellableItemPipeline _getSellableItemPipeline;
        private readonly IFindEntityPipeline _findEntityPipeline;

        public AddPropertiesBlock(IPersistEntityPipeline persistEntityPipeline, IGetSellableItemPipeline getSellableItemPipeline, IFindEntityPipeline findEntityPipeline)
        {
            _persistEntityPipeline = persistEntityPipeline;
            _getSellableItemPipeline = getSellableItemPipeline;
            _findEntityPipeline = findEntityPipeline;
        }

        public override async Task<SynchronizeProductArgument> Run(SynchronizeProductArgument arg, CommercePipelineExecutionContext context)
        {
            var sellableItem = arg.SellableItem;

            sellableItem.Brand = arg.ImportProduct.Brand;

            //Localize display name
            var localizedEntityComponent = sellableItem.GetComponent<LocalizedEntityComponent>();
            var localizedEntity = (LocalizationEntity) await _findEntityPipeline.Run(
                new FindEntityArgument(typeof(LocalizationEntity), localizedEntityComponent.Entity.EntityTarget, true),
                context.CommerceContext.GetPipelineContextOptions());

            var displayNames = arg.ImportProduct.ProductName.Select(p => new Parameter(p.Language, p.Name)).ToList();
            localizedEntity.AddOrUpdatePropertyValue("DisplayName", displayNames);
            await _persistEntityPipeline.Run(new PersistEntityArgument(localizedEntity),
                context.CommerceContext.GetPipelineContextOptions());

            //Add identifiers
            var identifiersComponent = sellableItem.GetComponent<IdentifiersComponent>();
            identifiersComponent.SKU = arg.ImportProduct.ProductId;
            sellableItem.SetComponent(identifiersComponent);

            sellableItem = (await _persistEntityPipeline.Run(new PersistEntityArgument(sellableItem),
                               context.CommerceContext.GetPipelineContextOptions()))?.Entity as SellableItem ?? sellableItem;

            arg.SellableItem = sellableItem;
            return arg;
        }
    }
}
