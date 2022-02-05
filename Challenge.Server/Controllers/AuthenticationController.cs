using Challenge.Model;
using Challenge.Server.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Challenge.Server.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly IAuthenticationRepository AuthenticationRepository;
        private readonly ILogger Logger;

        public AuthenticationController(IConfiguration configuration,
            IAuthenticationRepository authenticationRepository, ILogger<AuthenticationController> logger)
        {
            Configuration = configuration;
            AuthenticationRepository = authenticationRepository;
            Logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login([FromBody] Authentication credentials)
        {
            User returnUser = null;

            try
            {
                returnUser = await AuthenticationRepository.Login(credentials);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Ha ocurrido un error al intentar autenticar un usuario.");
                return StatusCode(500, returnUser);
            }

            if (returnUser == null)
            {
                Logger.LogError("Un usuario ha intentado autenticarse sin exito.");
                return StatusCode(401, returnUser);
            }

            try
            {
                return StatusCode(200, GenerateToken(returnUser));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Ha ocurrido un error al intentar generar un token.");
                return StatusCode(500, returnUser);
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] Authentication credentials)
        {
            User returnUser = null;

            try
            {
                returnUser = await AuthenticationRepository.Register(credentials);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Ha ocurrido un error al intentar registrar un usuario");
                return StatusCode(500, returnUser);
            }

            if (returnUser == null)
            {
                Logger.LogError("Un usuario ha intentado registrarse sin exito");
                return StatusCode(401, returnUser);
            }

            return returnUser;
        }

        private User GenerateToken(User user)
        {
            var _symmetricSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Configuration["JWT:SecretKey"])
                );

            var _signinCredentials = new SigningCredentials(
                _symmetricSecurityKey, SecurityAlgorithms.HmacSha256
                );

            var _Header = new JwtHeader(_signinCredentials);

            List<Claim> _preClaims = new();
            var _Claims = _preClaims.ToArray();

            var _payLoad = new JwtPayload(
                issuer: Configuration["JWT:Issuer"],
                audience: Configuration["JWT:Audience"],
                claims: _Claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(10)
                );

            var _Token = new JwtSecurityToken(
                _Header,
                _payLoad
                );

            user.Token = new JwtSecurityTokenHandler().WriteToken(_Token);
            return user;
        }
    }
}
