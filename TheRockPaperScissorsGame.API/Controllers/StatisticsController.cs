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
        public async Task<ActionResult<string>> GetLeaderboardAsync(int amount, StatisticsType type)
        {
            return "";
        }

        [HttpGet]
        [Route("results")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<string>> GetUserStatistics()
        {
            return "";
        }

        [HttpGet]
        [Route("time")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<string>> GetGameSpendTime(string token)
        {
            return "";
        }

        [HttpGet]
        [Route("moves")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<string>> CheckSessionAsync(string roomId)
        {
            return "";
        }

    }
}
