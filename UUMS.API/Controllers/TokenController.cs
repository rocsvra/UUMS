using AdunTech.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UUMS.Domain.IRepositories;

namespace UUMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("policy")]
    public class TokenController : ControllerBase
    {
        private readonly JwtTokenOptions _jwtTokenOptions;
        private readonly IUserRepository _userRepository;

        public TokenController(IOptionsMonitor<JwtTokenOptions> jwtTokenOptions
            , IUserRepository userRepository)
        {
            _jwtTokenOptions = jwtTokenOptions.CurrentValue;
            _userRepository = userRepository;
        }

        /// <summary>
        /// 获取口令
        /// </summary>
        /// <param name="username">账户</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public ActionResult<JwtToken> Get(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return BadRequest("参数不能为空");
            }
            var user = _userRepository.FirstOrDefault(o => o.Account == username);
            if (user == null)
            {
                return BadRequest("account不存在");
            }
            var psw = MD5.Encrypt(password);
            if (user.Password != psw)
            {
                return BadRequest("密码错误");
            }

            //创建用户身份标识
            var claims = new Claim[]
            {
                new Claim("userid", user.Id.ToString()),
                new Claim("username", user.Name),
                new Claim("avatar",user.Avatar??"")
            };
            //创建令牌
            var jstoken = new JwtSecurityToken(
                issuer: _jwtTokenOptions.Issuer,
                audience: _jwtTokenOptions.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: _jwtTokenOptions.Credentials);
            return Ok(new JwtToken
            {
                token = new JwtSecurityTokenHandler().WriteToken(jstoken),
                expiration = jstoken.ValidTo
            });
        }
    }
}
