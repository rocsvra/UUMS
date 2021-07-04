using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace UUMS.API
{
    public class JwtTokenOptions
    {
        /// <summary>
        /// 签发者
        /// </summary>
        public string Issuer { get; set; } = "server";

        /// <summary>
        /// 颁发给谁
        /// </summary>
        public string Audience { get; set; } = "client";

        /// <summary>
        /// 令牌密码（至少16位）
        /// </summary>
        public string SecurityKey { get; set; }

        /// <summary>
        /// 对称秘钥
        /// </summary>
        public SymmetricSecurityKey Key => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey));

        /// <summary>
        /// 数字签名
        /// </summary>
        public SigningCredentials Credentials => new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
    }
}
