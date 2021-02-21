using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
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
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult> RegisterAsync([FromBody] Account account)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            bool isRegistered;

            try
            {
                isRegistered = await _authService.Register(account);
            }
            //do we need this catch?
            catch (Exception e) // added e to see the exception during debug
            {
                // log error
                return Unauthorized();
            }

            if (isRegistered)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        public async Task<ActionResult<string>> LoginAsync([FromBody] Account account)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                string token = await _authService.Login(account.Login, account.Password);
                if (token == null)
                {
                    return NotFound();
                }

                return Ok();
            }
            catch (InvalidCastException)
            {
                return Conflict();
            }
        }
    }
}
