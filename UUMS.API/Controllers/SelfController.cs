using AdunTech.AutoMapperExtension;
using AdunTech.CommonDomain;
using AdunTech.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using UUMS.Application.Dtos;
using UUMS.Application.Specifications;
using UUMS.Application.Util;
using UUMS.Application.Vos;
using UUMS.Domain.DO;

namespace UUMS.API.Controllers
{
    /// <summary>
    /// 用户自我信息
    /// </summary>
    public class SelfController : UumsControllerBase
    {
        private readonly JwtTokenOptions _jwtTokenOptions;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;

        public SelfController(IUnitOfWork unitOfWork
            , IOptionsMonitor<JwtTokenOptions> jwtTokenOptions
            , IRepository<User> userRepository
            , IRepository<Role> roleRepository)
        {
            _jwtTokenOptions = jwtTokenOptions.CurrentValue;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// 获取口令
        /// </summary>
        /// <param name="username">账户</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("Token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public ActionResult<JwtToken> Get(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return BadRequest("参数不能为空");
            }
            var spec = new UserFilterSpecification(o => o.Account == username);
            var user = _userRepository.FirstOrDefault(spec);
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

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("Info")]
        public ActionResult<UserDto> GetUserInfo()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userid = claimsIdentity.FindFirst("userid")?.Value;
            var user = _userRepository.Find(new Guid(userid));
            var dto = user.Map<User, UserDto>();
            return Ok(dto);
        }

        /// <summary>
        ///  获取用户菜单列表
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet("ElementuiMenu")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public ActionResult<List<ElementMenuVO>> GetElementList(Guid clientId)
        {
            List<Menu> menus = new List<Menu>();
            var user = _userRepository.First(new UserFilterSpecification(LoginUserId));
            var roleIds = user.Roles?.Select(o => o.Id).ToList();
            if (roleIds != null)
            {
                var spec = new RoleFilterSpecification(roleIds);
                var roles = _roleRepository.Query(spec);
                foreach (var role in roles)
                {
                    if (role.Menus != null)
                    {
                        menus.AddRange(role.Menus);
                    }
                }
            }
            menus = menus.Distinct().Where(o => o.ClientId == clientId).ToList();
            return menus.ToElementMenu();
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("Password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public ActionResult PutPsw([FromBody] UserPswVO param)
        {
            if (string.IsNullOrWhiteSpace(param.Password)
                || string.IsNullOrWhiteSpace(param.RepeatPassword)
                || param.Password != param.RepeatPassword)
            {
                return BadRequest("参数错误，请保持两次输入密码一致");
            }
            if (param.Password.Length < 6)
            {
                return BadRequest("密码至少6位");
            }
            var user = _userRepository.Find(LoginUserId);
            if (user == null)
            {
                return BadRequest("参数错误，用户未登录");
            }
            user.Password = MD5.Encrypt(param.Password);
            _unitOfWork.Modify(user);
            _unitOfWork.Commit();
            return Ok();
        }
    }
}
