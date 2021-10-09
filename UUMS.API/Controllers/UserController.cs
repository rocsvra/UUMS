using AdunTech.AutoMapperExtension;
using AdunTech.Co2Net.Models;
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
using System.Linq.Expressions;
using System.Security.Claims;
using UUMS.API.Util;
using UUMS.Application.Dtos;
using UUMS.Domain.DO;
using UUMS.Domain.IRepositories;
using UUMS.Infrastructure;

namespace UUMS.API.Controllers
{
    public class UserController : UumsControllerBase
    {
        private readonly JwtTokenOptions _jwtTokenOptions;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly UumsDbContext _uumsDbContext;

        public UserController(IUnitOfWork unitOfWork
            , IOptionsMonitor<JwtTokenOptions> jwtTokenOptions
            , IUserRepository userRepository
            , IRoleRepository roleRepository
            , UumsDbContext uumsDbContext)
        {
            _jwtTokenOptions = jwtTokenOptions.CurrentValue;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _uumsDbContext = uumsDbContext;
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
        ///  获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public ActionResult<UserDto> Get(Guid id)
        {
            var user = _userRepository.Find(id);
            var dto = user.Map<User, UserDto>();
            dto.RelatedRoles = user.Roles?.Map<Role, RoleDto>().ToList();
            return dto;
        }

        /// <summary>
        ///  获取用户(分页)
        /// </summary>
        /// <param name="pageIndex">从0开始计数</param>
        /// <param name="pageSize"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("Page")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public ActionResult<PaginatedItems<UserDto>> GetPage(int pageIndex, int pageSize, string name)
        {
            Expression<Func<User, bool>> predicate = o => string.IsNullOrEmpty(name) || o.Name.Contains(name);
            int total = _userRepository.Query(predicate).Count();
            var users = _userRepository
                .Page(pageSize, pageIndex, predicate, o => o.Account)
                .ToList();
            var dtos = users.Map<User, UserDto>();
            return new PaginatedItems<UserDto>(pageIndex, pageSize, total, dtos);
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public ActionResult<UserDto> Post(UserVO param)
        {
            if (string.IsNullOrWhiteSpace(param.Name)
                || string.IsNullOrWhiteSpace(param.Account)
                || string.IsNullOrWhiteSpace(param.Mobile))
            {
                return BadRequest("参数错误");
            }

            var dto = param.Map<UserVO, UserDto>();
            dto.Id = Guid.NewGuid();
            dto.CreatedAt = DateTime.Now;
            var entity = dto.Map<UserDto, User>();
            entity.Password = MD5.Encrypt(param.Account);
            _unitOfWork.AddAndCommit(entity);
            return dto;
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public ActionResult Put(Guid id, [FromBody] UserVO param)
        {
            if (string.IsNullOrWhiteSpace(param.Name)
                || string.IsNullOrWhiteSpace(param.Account)
                || string.IsNullOrWhiteSpace(param.Mobile))
            {
                return BadRequest("参数错误");
            }

            if (!_userRepository.Exists(id))
            {
                return BadRequest("参数错误，id不存在");
            }

            var user = _userRepository.Find(id);
            var sameAccountUser = _userRepository.FirstOrDefault(o => o.Account == param.Account);
            if (user.Account != param.Account && sameAccountUser != null)
            {
                return BadRequest("参数错误，account已被使用");
            }
            user.Name = param.Name;
            user.Account = param.Account;
            user.Sex = param.Sex;
            user.Mobile = param.Mobile;
            user.Mail = param.Mail;
            _unitOfWork.ModifyAndCommit(user);

            return Ok();
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public ActionResult Delete(Guid id)
        {
            if (!_userRepository.Exists(id))
            {
                return BadRequest("参数错误，id不存在");
            }
            var user = _userRepository.Find(id);
            _unitOfWork.Remove(user);
            _unitOfWork.Commit();
            return Ok();
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("Password/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public ActionResult PutPsw(Guid id, [FromBody] UserPswVO param)
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
            if (!_userRepository.Exists(id))
            {
                return BadRequest("参数错误，id不存在");
            }
            var user = _userRepository.Find(id);
            user.Password = MD5.Encrypt(param.Password);
            _unitOfWork.ModifyAndCommit(user);
            return Ok();
        }

        /// <summary>
        /// 用户关联角色
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        [HttpPut("{id}/Roles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public ActionResult PutRoles(Guid id, [FromBody] List<Guid> roleids)
        {
            if (!_userRepository.Exists(id))
            {
                return BadRequest("参数错误，id不存在");
            }
            var roles = _roleRepository.Query(o => roleids.Contains(o.Id)).ToList();
            var user = _userRepository.Find(id);
            user.Roles = roles;
            _unitOfWork.ModifyIncludeRelationAndCommit(user);
            return Ok();
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
            var user = _userRepository.Find(LoginUserId);
            var roleIds = user.Roles?.Select(o => o.Id);
            if (roleIds != null)
            {
                var roles = _roleRepository.Query(o => roleIds.Contains(o.Id)).ToList();
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
    }
}
