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
    public class CharactersController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly ICharactersRepository CharacterRepository;
        private readonly ILogger Logger;

        public CharactersController(IConfiguration configuration,
            ICharactersRepository characterRepository, ILogger<CharactersController> logger)
        {
            Configuration = configuration;
            CharacterRepository = characterRepository;
            Logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<Dictionary<string, string>>> GetCharacters([FromQuery] string name, int age, int movies)
        {
            Dictionary<string, string> returnCharacters = null;

            if (string.IsNullOrEmpty(name) && age <= 0 && movies <= 0)
            {
                try
                {
                    returnCharacters = await CharacterRepository.GetCharacters();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Ha ocurrido un error al intentar obtener los personajes.");
                    return StatusCode(500, returnCharacters);
                }
            }
            else
            {
                try
                {
                    returnCharacters = await CharacterRepository.GetCharacters(name, age, movies);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Ha ocurrido un error al intentar obtener los personajes.");
                    return StatusCode(500, returnCharacters);
                }
            }

            if (returnCharacters == null)
            {
                Logger.LogError("Se han intentado obtener los personajes sin exito");
                return StatusCode(401, returnCharacters);
            }

            return returnCharacters;
        }

        [HttpGet("{identifier}")]
        public async Task<ActionResult<Character>> GetCharacter(int identifier)
        {
            Character returnCharacter = null;

            try
            {
                returnCharacter = await CharacterRepository.GetCharacter(identifier);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Ha ocurrido un error al intentar obtener un personaje.");
                return StatusCode(500, returnCharacter);
            }

            if (returnCharacter == null)
            {
                Logger.LogError("Se ha intentado obtener un producto sin exito.");
                return StatusCode(404, returnCharacter);
            }

            return StatusCode(200, returnCharacter);
        }

        [HttpPost]
        public async Task<ActionResult<Character>> CreateCharacter([FromBody] Character character)
        {
            Character returnCharacter = null;

            try
            {
                returnCharacter = await CharacterRepository.CreateCharacter(character);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Ha ocurrido un error al intentar crear un personaje.");
                return StatusCode(500, returnCharacter);
            }

            if (returnCharacter == null)
            {
                Logger.LogError("Se ha intentado crear un personaje sin exito.");
                return StatusCode(500, returnCharacter);
            }

            return returnCharacter;
        }

        [HttpPut]
        public async Task<ActionResult<Character>> UpdateCharacter([FromBody] Character character)
        {
            Character returnCharacter = null;

            try
            {
                returnCharacter = await CharacterRepository.UpdateCharacter(character);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Ha ocurrido un error al intentar modificar un personaje.");
                return StatusCode(500, returnCharacter);
            }

            if (returnCharacter == null)
            {
                Logger.LogError("Se ha intentado modificar un personaje sin exito.");
                return StatusCode(500, returnCharacter);
            }

            return returnCharacter;
        }

        [HttpDelete]
        public async Task DeleteCharacter([FromBody] int identifier)
        {
            await CharacterRepository.DeleteCharacter(identifier);
        }
    }
}
