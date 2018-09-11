using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Plugin.ProductImport.Commands;
using Plugin.ProductImport.Models;
using Sitecore.Commerce.Core;

namespace Plugin.ProductImport.Controllers
{
    public class CommandsController : CommerceController
    {
        public CommandsController(IServiceProvider serviceProvider, CommerceEnvironment globalEnvironment) : base(serviceProvider, globalEnvironment)
        {
        }

        [HttpPost]
        [Route("SynchronizeCatalog()")]
        public async Task<IActionResult> SynchronizeCatalog([FromBody] Catalog value)
        {
            if(!this.ModelState.IsValid || value == null)
                return new BadRequestObjectResult(this.ModelState);

            var command = Command<SynchronizeCatalogCommand>();
            await command.Process(CurrentContext, value);
            return new ObjectResult(command);
        }
    }
}
