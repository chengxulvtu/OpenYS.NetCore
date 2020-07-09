using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace OpenYS.NetCore
{
    public class AccessTokenManager
    {
        private readonly ConcurrentDictionary<string, AccessToken> maps = new ConcurrentDictionary<string, AccessToken>();

        public void AddOrUpdateAccessToken(string appKey, AccessToken accessToken)
        {
            maps.AddOrUpdate(appKey, accessToken, (key, oldAccessToken) => accessToken);
        }

        public AccessToken GetAccessToken(string appKey)
        {
            maps.TryGetValue(appKey, out AccessToken accessToken);
            return accessToken;
        }
    }
}
