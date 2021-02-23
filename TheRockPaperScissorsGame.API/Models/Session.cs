using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using TheRockPaperScissorsGame.API.Enums;
using TheRockPaperScissorsGame.API.Exceptions;

namespace TheRockPaperScissorsGame.API.Models
{
    public class Session
    {
        private TimeSpan _connectionTimeOut = TimeSpan.FromMinutes(5);

        private TimeSpan _roundTimeOut = TimeSpan.FromSeconds(20);

        static SemaphoreSlim _lockSlim = new SemaphoreSlim(1, 1);

        public Session()
        {
        }

        public Session(GameOptions options, string login)
        {
            SessionStart = DateTime.UtcNow;
            Player1Login = login;
            RoomNumber = Guid.NewGuid().ToString();
            Rounds = new List<Round>();
            IsBot = options.RoomType == RoomType.Train;
            if (IsBot)
            {
                LastMoveTime = DateTime.UtcNow;
            }
        }

        [JsonPropertyName("roomNumber")]
        public string RoomNumber { get; set; }

        [JsonPropertyName("rounds")]
        public List<Round> Rounds { get; set; }

        [JsonIgnore]
        public bool IsFinished { get; set; }

        [JsonPropertyName("sessionStart")]
        public DateTime SessionStart { get; set; }

        [JsonPropertyName("sessionFinished")]
        public DateTime SessionFinished { get; set; }

        [JsonPropertyName("player1Login")]
        public string Player1Login { get; set; }

        [JsonPropertyName("player2Login")]
        public string Player2Login { get; set; }

        [JsonIgnore]
        public DateTime? LastMoveTime { get; set; }

        [JsonIgnore]
        public bool IsBot { get; set; }

        [JsonIgnore]
        public bool RivalFound
        {
            get
            {
                var result =  IsBot || Player2Login != null;
                if(!result && DateTime.UtcNow - SessionStart >= _connectionTimeOut)
                {
                    throw new GameFinishedException(GameEndReason.ConnectionTimeOut);
                }
                return result;
            }
        }

        [JsonIgnore]
        public bool RoundTimeOut
        {
            get
            {
                if (LastMoveTime != null)
                {
                    return DateTime.UtcNow - LastMoveTime > _roundTimeOut;
                }
                return false;
            }
        }

        public async Task AddMoveAsync(bool isFirst, Move move)
        {
            await _lockSlim.WaitAsync();
            try
            {
                if (RoundTimeOut)
                {
                    throw new GameFinishedException(GameEndReason.RoundTimeOut);
                }
                Round round;
                if (Rounds.Count == 0)
                {
                    round = new Round();
                    Rounds.Add(round);
                }
                else
                {
                    var lastRound = Rounds[Rounds.Count - 1];
                    if (lastRound.Player1Move != null && lastRound.Player2Move != null)
                    {
                        round = new Round();
                        Rounds.Add(round);
                    }
                    else if ((isFirst && lastRound.Player1Move == null) || (!isFirst && lastRound.Player2Move == null))
                    {
                        round = lastRound;
                    }
                    else
                    {
                        throw new MoveException("User has already made a move");
                    }
                }
                if (isFirst)
                {
                    round.Player1Move = move;
                }
                else
                {
                    round.Player2Move = move;
                }
                LastMoveTime = DateTime.UtcNow;
            }
            finally
            {
                _lockSlim.Release();
            }
        }

        public async Task<Move?> GetMoveAsync(bool isFirst)
        {
            await _lockSlim.WaitAsync();
            try
            {
                if (Rounds.Count == 0)
                {
                    throw new MoveException("User has not made a move yet");
                }
                if (RoundTimeOut)
                {
                    throw new GameFinishedException(GameEndReason.RoundTimeOut);
                }
                var lastRound = Rounds[Rounds.Count - 1];

                if (isFirst)
                {
                    if(lastRound.Player1Move==null)
                    {
                        return null;
                    }
                    return lastRound.Player2Move;
                }
                else
                {
                    if (lastRound.Player2Move == null)
                    {
                        return null;
                    }
                    return lastRound.Player1Move;
                }
            }
            finally
            {
                _lockSlim.Release();
            }
        }

        public void AddSecondPlayer(string login)
        {
            if (login==null)
            {
                throw new ArgumentNullException();
            }
            if (IsBot || Player2Login != null)
            {
                throw new RoomConnectionException("Room is occupated");
            }
            if (Player1Login == login)
            {
                throw new RoomConnectionException("User is already in the room");
            }
            Player2Login = login;
            LastMoveTime = DateTime.UtcNow;
        }
    }
}
