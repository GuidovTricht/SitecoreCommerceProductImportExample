using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Plugin.ProductImport.Pipelines.SynchronizeCatalog;
using Plugin.ProductImport.Pipelines.SynchronizeCatalog.Blocks;
using Plugin.ProductImport.Pipelines.SynchronizeCategory;
using Plugin.ProductImport.Pipelines.SynchronizeCategory.Blocks;
using Plugin.ProductImport.Pipelines.SynchronizeProduct;
using Plugin.ProductImport.Pipelines.SynchronizeProduct.Blocks;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines.Definitions.Extensions;

namespace Plugin.ProductImport
{
    /// <summary>
    /// </summary>
    /// <seealso cref="Sitecore.Framework.Configuration.IConfigureSitecore" />
    public class ConfigureSitecore : IConfigureSitecore
    {
        /// <summary>
        /// The configure services.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);
            services.RegisterAllCommands(assembly);

            services.Sitecore().Pipelines(config => config
                //Register Controller API Methods
                .ConfigurePipeline<IConfigureServiceApiPipeline>(configure => configure
                    .Add<ConfigureServiceApiBlock>()
                )
                //Register SynchronizeCatalogPipeline
                .AddPipeline<ISynchronizeCatalogPipeline, SynchronizeCatalogPipeline>(configure => configure
                    .Add<GetOrCreateCatalogBlock>()
                    .Add<SynchronizeCategoriesBlock>()
                    .Add<SynchronizeProductsBlock>()
                )
                //Register SynchronizeCategoryPipeline
                .AddPipeline<ISynchronizeCategoryPipeline, SynchronizeCategoryPipeline>(configure => configure
                    .Add<GetOrCreateCategoryBlock>()
                    .Add<AssociateWithParentBlock>()
                    .Add<SynchronizeSubCategoriesBlock>()
                )
                //Register SynchronizeProductPipeline
                .AddPipeline<ISynchronizeProductPipeline, SynchronizeProductPipeline>(configure => configure
                    .Add<GetOrCreateProductBlock>()
                    .Add<AddPropertiesBlock>()
                    .Add<CreateVariantsBlock>()
                    .Add<AssociateWithCategoriesBlock>()
                )
            );
        }
    }
}