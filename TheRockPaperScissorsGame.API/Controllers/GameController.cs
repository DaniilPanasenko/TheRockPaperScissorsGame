using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.API.Enums;
using TheRockPaperScissorsGame.API.Exceptions;
using TheRockPaperScissorsGame.API.Models;
using TheRockPaperScissorsGame.API.Services;
using TheRockPaperScissorsGame.API.Storages;

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

        private readonly ITokenStorage _tokenStorage;

        public GameController(ISessionService sessionService, IRoundService roundService, ITokenStorage tokenstorage)
        {
            _sessionService = sessionService;
            _roundService = roundService;
            _tokenStorage = tokenstorage;
        }

        [FromHeader(Name = "Token")]
        public string Token { get; set; }

        [NonAction]
        private string GetLogin()
        {
            if (Token == null)
            {
                return null;
            }
            var login = _tokenStorage.GetLogin(Token);
            if (login == null)
            {
                return null;
            }
            return login;
        }

        [HttpPost]
        [Route("start_session")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<string>> StartSessionAsync([FromBody] GameOptions options)
        {
            var login = GetLogin();
            if (login == null) return Unauthorized();
            try
            {
                var roomNumber = await _sessionService.StartSessionAsync(login, options);
                return Ok(roomNumber);
            }
            catch(RoomConnectionException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("check_session/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> CheckSessionAsync(string id)
        {
            var login = GetLogin();
            if (login == null) return Unauthorized();
            try
            {
                var playerName = await _sessionService.CheckSessionAsync(id, login);
                if (playerName == null)
                {
                    return NotFound();
                }
                return Ok(playerName);
            }
            catch (RoomConnectionException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(GameFinishedException ex)
            {
                await _sessionService.FinishSessionAsync(id);
                return Conflict(ex.Status.ToString());
            }
        }

        [HttpPost]
        [Route("do_move/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> DoMoveAsync([FromRoute]int id, [FromBody] string move)
        {
            var login = GetLogin();
            if (login == null) return Unauthorized();
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("check_move/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<string>> CheckMoveAsync(int id)
        {
            var login = GetLogin();
            if (login == null) return Unauthorized();
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("finish_session/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<string>> FinishSessionAsync(string id)
        {
            var login = GetLogin();
            if (login == null) return Unauthorized();
            try
            {
                await _sessionService.FinishSessionAsync(id);
                return Conflict(GameEndReason.RivalLeftGame.ToString());
            }
            catch (RoomConnectionException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}