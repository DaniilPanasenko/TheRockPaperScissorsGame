using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.API.Enums;
using TheRockPaperScissorsGame.API.Services;
using TheRockPaperScissorsGame.API.Storages;

namespace TheRockPaperScissorsGame.API.Controllers
{
    [ApiController]
    [Route("api/statistics")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        private readonly ITokenStorage _tokenStorage;

        public StatisticsController(IStatisticsService statisticService, ITokenStorage tokenStorage)
        {
            _statisticsService = statisticService;
            _tokenStorage = tokenStorage;
        }

        [FromHeader(Name = "Token")]
        public string Token { get; set; }


        [HttpGet]
        [Route("leaderboard")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetLeaderboardAsync(int? amount, StatisticsType type)
        {
            if (amount == null)
            {
                amount = int.MaxValue;
            }

            if (amount < 0)
            {
                return BadRequest();
            }

            if (type == StatisticsType.WinStatistics)
            {
                var result = await _statisticsService.GetWinsLeaderboardAsync((int)amount);
                return Ok(result);
            }
            else
            if (type == StatisticsType.TimeStatistics)
            {
                var result = await _statisticsService.GetTimeLeaderboardAsync((int)amount);
                return Ok(result);
            }
            else
            if (type == StatisticsType.WinPercentStatistics)
            {
                var result = await _statisticsService.GetWinsPercentLeaderboardAsync((int)amount);
                return Ok(result);
            }
            else
            { 
                return BadRequest(); 
            }
        }

        [HttpGet]
        [Route("user_results")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult> GetUserResultsAsync()
        {
            var login = GetLogin();

            if (login == null)
            {
                return Unauthorized();
            }

            var result = await _statisticsService.GetUserResultsCountAsync(login);

            return Ok(result);
        }

        [HttpGet]
        [Route("user_time")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult> GetUserGameTime()
        {
            var login = GetLogin();

            if (login == null)
            {
                return Unauthorized();
            }

            var result = await _statisticsService.GetUserGameTimeAsync(login);

            return Ok(result);
        }

        [HttpGet]
        [Route("user_moves")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult> GetUserMovesAsync()
        {
            var login = GetLogin();

            if (login == null)
            {
                return Unauthorized();
            }

            var result = await _statisticsService.GetUserMovesStatisticsAsync(login);

            return Ok(result);
        }

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
    }
}
