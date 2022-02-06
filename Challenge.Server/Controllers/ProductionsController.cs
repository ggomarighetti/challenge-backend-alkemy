using Challenge.Model;
using Challenge.Server.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Challenge.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionsController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly IProductionsRepository ProductionsRepository;
        private readonly ILogger Logger;

        public ProductionsController(IConfiguration configuration,
            IProductionsRepository productionsRepository, ILogger<ProductionsController> logger)
        {
            Configuration = configuration;
            ProductionsRepository = productionsRepository;
            Logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<Tuple<string, string, DateTime>>>> GetProductions([FromQuery] string title, int genre, string order)
        {
            List<Tuple<string, string, DateTime>> returnProductions = null;

            if (string.IsNullOrEmpty(title) && genre <= 0 && string.IsNullOrEmpty(order))
            {
                try
                {
                    returnProductions = await ProductionsRepository.GetProductions();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Ha ocurrido un error al intentar obtener las producciones.");
                    return StatusCode(500, returnProductions);
                }
            }
            else
            {
                try
                {
                    returnProductions = await ProductionsRepository.GetProductions(title, genre, order);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Ha ocurrido un error al intentar obtener las producciones.");
                    return StatusCode(500, returnProductions);
                }
            }

            if (returnProductions == null)
            {
                Logger.LogError("Se han intentado obtener las producciones sin exito");
                return StatusCode(401, returnProductions);
            }

            return returnProductions;
        }

        [HttpGet("{identifier}")]
        public async Task<ActionResult<Production>> GetProduction(int identifier)
        {
            Production returnProduction = null;

            try
            {
                returnProduction = await ProductionsRepository.GetProduction(identifier);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Ha ocurrido un error al intentar obtener una produccion.");
                return StatusCode(500, returnProduction);
            }

            if (returnProduction == null)
            {
                Logger.LogError("Se ha intentado obtener una produccion sin exito.");
                return StatusCode(404, returnProduction);
            }

            return StatusCode(200, returnProduction);
        }

        [HttpPost]
        public async Task<ActionResult<Production>> CreateProduction([FromBody] Production production)
        {
            Production returnProduction = null;

            try
            {
                returnProduction = await ProductionsRepository.CreateProduction(production);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Ha ocurrido un error al intentar crear una produccion.");
                return StatusCode(500, returnProduction);
            }

            if (returnProduction == null)
            {
                Logger.LogError("Se ha intentado crear una produccion sin exito.");
                return StatusCode(500, returnProduction);
            }

            return returnProduction;
        }

        [HttpPut]
        public async Task<ActionResult<Production>> UpdateProduction([FromBody] Production production)
        {
            Production returnProduction = null;

            try
            {
                returnProduction = await ProductionsRepository.UpdateProduction(production);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Ha ocurrido un error al intentar modificar una produccion.");
                return StatusCode(500, returnProduction);
            }

            if (returnProduction == null)
            {
                Logger.LogError("Se ha intentado modificar una produccion sin exito.");
                return StatusCode(500, returnProduction);
            }

            return returnProduction;
        }

        [HttpDelete]
        public async Task DeleteProduction([FromBody] int identifier)
        {
            await ProductionsRepository.DeleteProduction(identifier);
        }
    }
}
