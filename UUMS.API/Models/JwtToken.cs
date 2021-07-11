using System;

namespace UUMS.API
{
    /// <summary>
    /// token
    /// </summary>
    public class JwtToken
    {
        /// <summary>
        /// 口令
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime expiration { get; set; }
    }
}
