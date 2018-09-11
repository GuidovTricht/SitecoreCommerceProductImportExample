using System.Threading.Tasks;
using Microsoft.AspNetCore.OData.Builder;
using Plugin.ProductImport.Commands;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Plugin.ProductImport
{
    public class ConfigureServiceApiBlock : PipelineBlock<ODataConventionModelBuilder, ODataConventionModelBuilder, CommercePipelineExecutionContext>
    {
        public override Task<ODataConventionModelBuilder> Run(ODataConventionModelBuilder modelBuilder, CommercePipelineExecutionContext context)
        {
            Condition.Requires(modelBuilder).IsNotNull("The argument can not be null");

            modelBuilder.AddEntityType(typeof(SynchronizeCatalogCommand));

            var synchronizeCatalog = modelBuilder.Action("SynchronizeCatalog");
            synchronizeCatalog.Returns<string>();
            synchronizeCatalog.ReturnsFromEntitySet<CommerceCommand>("Commands");

            return Task.FromResult(modelBuilder);
        }
    }
}
