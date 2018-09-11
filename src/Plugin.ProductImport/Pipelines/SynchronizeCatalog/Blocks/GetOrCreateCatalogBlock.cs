using System.Threading.Tasks;
using Plugin.ProductImport.Pipelines.SynchronizeCatalog.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Plugin.ProductImport.Pipelines.SynchronizeCatalog.Blocks
{
    public class GetOrCreateCatalogBlock : PipelineBlock<SynchronizeCatalogArgument, SynchronizeCatalogArgument, CommercePipelineExecutionContext>
    {
        private readonly IGetCatalogPipeline _getCatalogPipeline;
        private readonly ICreateCatalogPipeline _createCatalogPipeline;
        private readonly IDoesEntityExistPipeline _doesEntityExistPipeline;

        public GetOrCreateCatalogBlock(IGetCatalogPipeline getCatalogPipeline, ICreateCatalogPipeline createCatalogPipeline, IDoesEntityExistPipeline doesEntityExistPipeline)
        {
            _getCatalogPipeline = getCatalogPipeline;
            _createCatalogPipeline = createCatalogPipeline;
            _doesEntityExistPipeline = doesEntityExistPipeline;
        }

        public override async Task<SynchronizeCatalogArgument> Run(SynchronizeCatalogArgument arg, CommercePipelineExecutionContext context)
        {
            Sitecore.Commerce.Plugin.Catalog.Catalog catalog = null;
            var catalogId = arg.ImportCatalog.CatalogName.ProposeValidId()
                .EnsurePrefix(CommerceEntity.IdPrefix<Sitecore.Commerce.Plugin.Catalog.Catalog>());
            if (await _doesEntityExistPipeline.Run(
                new FindEntityArgument(typeof(Sitecore.Commerce.Plugin.Catalog.Catalog), catalogId), context.CommerceContext.GetPipelineContextOptions()))
            {
                catalog = await _getCatalogPipeline.Run(new GetCatalogArgument(arg.ImportCatalog.CatalogName.ProposeValidId()), context.CommerceContext.GetPipelineContextOptions());
            }
            else
            {
                var createResult = await _createCatalogPipeline.Run(new CreateCatalogArgument(arg.ImportCatalog.CatalogName.ProposeValidId(), arg.ImportCatalog.CatalogName), context.CommerceContext.GetPipelineContextOptions());
                catalog = createResult?.Catalog;
            }

            Condition.Requires<Sitecore.Commerce.Plugin.Catalog.Catalog>(catalog).IsNotNull($"{this.Name}: The Catalog could not be created.");
            
            arg.Catalog = catalog;

            return arg;
        }
    }
}
