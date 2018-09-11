using System;
using System.Threading.Tasks;
using Plugin.ProductImport.Pipelines.SynchronizeCatalog;
using Plugin.ProductImport.Pipelines.SynchronizeCatalog.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.Plugin.Catalog;
using Catalog = Plugin.ProductImport.Models.Catalog;

namespace Plugin.ProductImport.Commands
{
    public class SynchronizeCatalogCommand : CommerceCommand
    {
        private readonly ISynchronizeCatalogPipeline _synchronizeCatalogPipeline;

        public SynchronizeCatalogCommand(ISynchronizeCatalogPipeline synchronizeCatalogPipeline, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._synchronizeCatalogPipeline = synchronizeCatalogPipeline;
        }

        public virtual async Task<CatalogContentArgument> Process(CommerceContext commerceContext, Catalog catalog)
        {
            using (CommandActivity.Start(commerceContext, this))
            {
                var arg = new SynchronizeCatalogArgument(){ ImportCatalog = catalog };
                return await _synchronizeCatalogPipeline.Run(arg, new CommercePipelineExecutionContextOptions(commerceContext));
            }
        }
    }
}
