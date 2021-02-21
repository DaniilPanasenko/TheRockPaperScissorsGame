using System;
using System.Collections.Concurrent;

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

                if (_tokens.TryAdd(login, token))
                {
                    return token;
                }
                else
                { 
                // change the value (token) where the key == login
                }
            }
        }

        public string GetLogin(string token)
        {
            // we need to check ContainsValue(token) and retrun the key..
            if (token == null || !_tokens.ContainsKey(token))
            {
                return null;
            }

            return _tokens[token];
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