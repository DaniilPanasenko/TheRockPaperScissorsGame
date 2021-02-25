using Microsoft.AspNetCore.Mvc;
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

        public GameController(ISessionService sessionService, IRoundService roundService, ITokenStorage tokenStorage)
        {
            _sessionService = sessionService;
            _roundService = roundService;
            _tokenStorage = tokenStorage;
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
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        public async Task<ActionResult<string>> StartSessionAsync([FromBody] GameOptions options)
        {
            var login = GetLogin();
            if (login == null) return Unauthorized();
            try
            {
                var roomNumber = await _sessionService.StartSessionAsync(login, options);
                return Ok(roomNumber);
            }
            catch (RoomConnectionException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (GameFinishedException ex)
            {
                await _sessionService.FinishSessionAsync(options.RoomNumber);
                return Conflict(ex.Message);
            }

        }

        [HttpGet]
        [Route("check_session/{roomId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CheckSessionAsync(string roomId)
        {
            var login = GetLogin();
            if (login == null) return Unauthorized();
            try
            {
                var playerName = await _sessionService.CheckSessionAsync(roomId, login);

                if (playerName == null) return NotFound();
                
                return Ok(playerName);
            }
            catch (RoomConnectionException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(GameFinishedException ex)
            {
                await _sessionService.FinishSessionAsync(roomId);
                return Conflict(ex.Status.ToString());
            }
        }

        [HttpPost]
        [Route("do_move/{roomId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> DoMoveAsync([FromRoute]string roomId, [FromBody] Move move)
        {
            var login = GetLogin();
            if (login == null) return Unauthorized();

            try
            {
                await _roundService.DoMoveAsync(login, roomId, move);
                return Ok();
            }
            catch (MoveException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (GameFinishedException ex)
            {
                await _sessionService.FinishSessionAsync(roomId);
                return Conflict(ex.Status.ToString());
            }
        }

        [HttpGet]
        [Route("check_move/{roomId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CheckMoveAsync(string roomId)
        {
            var login = GetLogin();
            if (login == null) return Unauthorized();

            try
            {
                var result = await _roundService.CheckMoveAsync(login, roomId);

                if (result == null) return NotFound();

                return Ok(result);
            }
            catch (MoveException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (RoomConnectionException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (GameFinishedException ex)
            {
                await _sessionService.FinishSessionAsync(roomId);
                return Conflict(ex.Status.ToString());
            }
        }

        [HttpPost]
        [Route("finish_session/{roomId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<string>> FinishSessionAsync(string roomId)
        {
            var login = GetLogin();
            if (login == null) return Unauthorized();
            try
            {
                await _sessionService.FinishSessionAsync(roomId);
                return Ok(GameEndReason.RivalLeftGame.ToString());
            }
            catch (RoomConnectionException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}