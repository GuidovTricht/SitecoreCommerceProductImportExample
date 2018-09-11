using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.ProductImport.Pipelines.SynchronizeCatalog.Arguments;
using Plugin.ProductImport.Pipelines.SynchronizeProduct;
using Plugin.ProductImport.Pipelines.SynchronizeProduct.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;

namespace Plugin.ProductImport.Pipelines.SynchronizeCatalog.Blocks
{
    public class SynchronizeProductsBlock : PipelineBlock<SynchronizeCatalogArgument, SynchronizeCatalogArgument, CommercePipelineExecutionContext>
    {
        private readonly ISynchronizeProductPipeline _synchronizeProductPipeline;

        public SynchronizeProductsBlock(ISynchronizeProductPipeline synchronizeProductPipeline)
        {
            _synchronizeProductPipeline = synchronizeProductPipeline;
        }

        public override async Task<SynchronizeCatalogArgument> Run(SynchronizeCatalogArgument arg, CommercePipelineExecutionContext context)
        {
            foreach (var product in arg.ImportCatalog.Products)
            {
                var syncResult = await _synchronizeProductPipeline.Run(new SynchronizeProductArgument()
                {
                    ImportProduct = product,
                    Catalog = arg.Catalog,
                    SellableItems = new List<SellableItem>()
                }, context.CommerceContext.GetPipelineContextOptions());

                arg.Catalog = syncResult?.Catalog;
            }

            return arg;
        }
    }
}
