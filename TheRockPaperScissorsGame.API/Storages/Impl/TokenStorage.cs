using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace TheRockPaperScissorsGame.API.Storages.Impl
{
    public class TokenStorage : ITokenStorage
    {
        private readonly ConcurrentDictionary<string, string> _tokens = new ConcurrentDictionary<string, string>();

        private readonly ILogger<TokenStorage> _logger;

        public TokenStorage(ILogger<TokenStorage> logger)
        {
            _logger = logger;
        }

        public string GenerateToken(string login)
        {
            _logger.LogDebug("");

            var token = Guid.NewGuid().ToString();
            
            if (_tokens.TryAdd(login, token))
            {
                return token;
            }
            else
            {
                _tokens[login] = token;
                return token;
            }
        }
    
        public string GetLogin(string token)
        {
            if (token == null || !_tokens.Values.Contains(token))
            {
                return null;
            }

            return _tokens.FirstOrDefault(x => x.Value == token).Key;
        }
    }
}