using System;
using System.Collections.Concurrent;
using System.Linq;

namespace TheRockPaperScissorsGame.API.Storages.Impl
{
    internal class TokenStorage : ITokenStorage
    {
        private readonly ConcurrentDictionary<string, string> _tokens = new ConcurrentDictionary<string, string>();

        public TokenStorage()
        {
        }

        public string GenerateToken(string login)
        {
            while (true)
            {
                var token = Guid.NewGuid().ToString();
                if (!_tokens.Values.Contains(token))
                {

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

#region Method when key is token
/*
   public string GenerateToken(string login)
        {
            while (true)
            {
                var token = Guid.NewGuid().ToString();

                if (_tokens.TryAdd(token, login))
                {
                    return token;
                }
            }
        }

        public string GetLogin(string token)
        {
            if (token == null || !_tokens.ContainsKey(token))
            {
                return null;
            }

            return _tokens[token];
        }
 */
#endregion