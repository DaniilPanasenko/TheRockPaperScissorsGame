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
    [Route("api/game")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class GameController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        private readonly IRoundService _roundService;

        public GameController(ISessionService sessionService, IRoundService roundService)
        {
            _sessionService = sessionService;
            _roundService = roundService;
        }

        [HttpPost]
        [Route("start_session")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<string>> StartSessionAsync([FromBody] GameOptions options)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("check_session/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> CheckSessionAsync(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("do_move/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> DoMoveAsync([FromRoute]int id, [FromBody] string move)
        {
            //mapping from string to move
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("check_move/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<string>> CheckMoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("finish_session/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<string>> FinishSessionAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}