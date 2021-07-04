using AdunTech.AutoMapperExtension;
using AdunTech.Co2Net.Models;
using AdunTech.CommonDomain;
using AdunTech.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using UUMS.Application.Dtos;
using UUMS.Domain.DO;
using UUMS.Domain.IRepositories;

namespace UUMS.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public UserController(IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
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
            int total = _userRepository.All().Count();
            var users = _userRepository
                .Page(pageSize, pageIndex, o => string.IsNullOrEmpty(name) || o.Name.Contains(name), o => o.Account)
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
        public ActionResult<UserDto> Post(UserModel param)
        {
            if (string.IsNullOrWhiteSpace(param.Name)
                || string.IsNullOrWhiteSpace(param.Account)
                || string.IsNullOrWhiteSpace(param.Mobile))
            {
                return BadRequest("参数错误");
            }

            var dto = param.Map<UserModel, UserDto>();
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
        public ActionResult Put(Guid id, [FromBody] UserModel param)
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

            var entity = param.Map<UserModel, User>();
            entity.Id = id;
            entity.CreatedAt = user.CreatedAt;
            _unitOfWork.ModifyAndCommit(entity);
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
        public ActionResult PutPsw(Guid id, [FromBody] UserPswModel param)
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
    }
}
