using System;
using System.Linq;
using System.Threading.Tasks;
using Plugin.ProductImport.Pipelines.SynchronizeCategory.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;

namespace Plugin.ProductImport.Pipelines.SynchronizeCategory.Blocks
{
    public class AssociateWithParentBlock : PipelineBlock<SynchronizeCategoryArgument, SynchronizeCategoryArgument, CommercePipelineExecutionContext>
    {
        private readonly IAssociateCategoryToParentPipeline _associateCategoryToParentPipeline;

        public AssociateWithParentBlock(IAssociateCategoryToParentPipeline associateCategoryToParentPipeline)
        {
            _associateCategoryToParentPipeline = associateCategoryToParentPipeline;
        }

        public override async Task<SynchronizeCategoryArgument> Run(SynchronizeCategoryArgument arg, CommercePipelineExecutionContext context)
        {
            var associateResult = await _associateCategoryToParentPipeline.Run(
                new CatalogReferenceArgument(arg.Catalog.Id, arg.ParentEntityId, arg.Category.Id),
                context.CommerceContext.GetPipelineContextOptions());

            //arg.Category = associateResult?.Categories?.FirstOrDefault(c => c.Id.Equals(arg.Category.Id));
            arg.Catalog = associateResult?.Catalog;

            return arg;
        }
    }
}
