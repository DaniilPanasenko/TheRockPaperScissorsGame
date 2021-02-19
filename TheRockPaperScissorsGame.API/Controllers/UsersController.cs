using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.API.Models;
using TheRockPaperScissorsGame.API.Services;

namespace TheRockPaperScissorsGame.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    internal class UsersController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UsersController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("register")]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<string>> RegisterAsync([FromBody] Account account)
        {
            return "";
        }

        [HttpPost]
        [Route("login")]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<string>> LoginAsync([FromBody] Account account)
        {
            return "";
        }
    }
}
