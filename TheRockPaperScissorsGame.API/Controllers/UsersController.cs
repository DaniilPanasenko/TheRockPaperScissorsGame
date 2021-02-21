using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.API.Enums;
using TheRockPaperScissorsGame.API.Exceptions;
using TheRockPaperScissorsGame.API.Models;
using TheRockPaperScissorsGame.API.Services;

namespace TheRockPaperScissorsGame.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class UsersController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UsersController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<string>> RegisterAsync([FromBody] Account account)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            bool isRegistered = await _authService.Register(account);

            if (isRegistered)
            {
                string token = await _authService.Login(account.Login, account.Password);
                return Ok(token);
            }
            else
            {
                return Unauthorized(AuthorizationStatus.LoginAlreadyExist.ToString("g"));
            }
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<string>> LoginAsync([FromBody] Account account)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                string token = await _authService.Login(account.Login, account.Password);
                return Ok(token);
            }
            catch (AuthorizationException ex)
            {
                return Unauthorized(ex.Status.ToString("g"));
            }
        }
    }
}
