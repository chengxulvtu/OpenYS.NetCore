using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace OpenYS.NetCore
{
    public class AccessToken
    {
        public AccessToken(string token, long expireTime)
        {
            Token = token;
            ExpireTimeStamp = expireTime;
            ExpireTime = DateTimeOffset.FromUnixTimeMilliseconds(ExpireTimeStamp).LocalDateTime;

            CreateTime = DateTime.Now;
        }

        /// <summary>
        /// Access Token
        /// </summary>
        public string Token { get; private set; }

        /// <summary>
        /// 过期时间戳
        /// </summary>
        public long ExpireTimeStamp { get; private set; }

        /// <summary>
        /// 生成时间
        /// </summary>
        public DateTime CreateTime { get; private set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireTime { get; private set; }


        /// <summary>
        /// 生成后过去了多长时间 （7天过期）
        /// </summary>
        public TimeSpan PassTimeSinceCreate => DateTime.Now - CreateTime;

        /// <summary>
        /// 即将过期 (距离过期1分钟判定为即将过期)
        /// </summary>
        public bool NearlyExpired => Expired == false && PassTimeSinceCreate.TotalMinutes > 10079;

        /// <summary>
        /// 是否过期
        /// </summary>
        public bool Expired => DateTime.Now > ExpireTime;
    }
}
