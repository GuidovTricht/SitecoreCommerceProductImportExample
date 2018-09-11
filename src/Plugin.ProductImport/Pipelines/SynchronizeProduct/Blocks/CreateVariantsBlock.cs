using System.Linq;
using System.Threading.Tasks;
using Plugin.ProductImport.Pipelines.SynchronizeProduct.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;

namespace Plugin.ProductImport.Pipelines.SynchronizeProduct.Blocks
{
    public class CreateVariantsBlock : PipelineBlock<SynchronizeProductArgument, SynchronizeProductArgument, CommercePipelineExecutionContext>
    {
        private readonly IPersistEntityPipeline _persistEntityPipeline;

        public CreateVariantsBlock(IPersistEntityPipeline persistEntityPipeline)
        {
            _persistEntityPipeline = persistEntityPipeline;
        }

        public override async Task<SynchronizeProductArgument> Run(SynchronizeProductArgument arg, CommercePipelineExecutionContext context)
        {
            var sellableItem = arg.SellableItem;
            var variantsComponent = sellableItem.GetComponent<ItemVariationsComponent>();
            variantsComponent.ChildComponents.Clear();

            foreach (var variant in arg.ImportProduct.Variants)
            {
                var colorProperty = variant.ProductProperties.FirstOrDefault(pp => pp.PropertyId.Equals("11"));
                var defaultColorValue = colorProperty?.Values?.FirstOrDefault(v => v.Language.Equals("en"))?.Value;
                var sizeProperty = variant.ProductProperties.FirstOrDefault(pp => pp.PropertyId.Equals("12"));
                var defaultSizeValue = sizeProperty?.Values?.FirstOrDefault(v => v.Language.Equals("en"))?.Value;
                var variantId = $"{arg.SellableItem.Name}{defaultColorValue}{defaultSizeValue}";

                var variationItem = new ItemVariationComponent()
                {
                    Id = variantId,
                    Name = $"{arg.SellableItem.DisplayName} {defaultColorValue} {defaultSizeValue}"
                };

                //Add identifiers
                var identifiersComponent = variationItem.GetComponent<IdentifiersComponent>();
                identifiersComponent.SKU = variantId;
                variationItem.SetComponent(identifiersComponent);

                variantsComponent.ChildComponents.Add(variationItem);
            }

            arg.SellableItem = (await _persistEntityPipeline.Run(new PersistEntityArgument(sellableItem),
                                   context.CommerceContext.GetPipelineContextOptions()))?.Entity as SellableItem ?? sellableItem;

            return arg;
        }
    }
}
