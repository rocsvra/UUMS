using AdunTech.AutoMapperExtension;
using AdunTech.Co2Net.Models;
using AdunTech.CommonDomain;
using AdunTech.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using UUMS.Application.Dtos;
using UUMS.Application.Specifications;
using UUMS.Application.Util;
using UUMS.Application.Vos;
using UUMS.Domain.DO;
using UUMS.Infrastructure;

namespace UUMS.API.Controllers
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class UserController : UumsControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly UumsDbContext _uumsDbContext;

        public UserController(IUnitOfWork unitOfWork
            , IRepository<User> userRepository
            , IRepository<Role> roleRepository
            , UumsDbContext uumsDbContext)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _uumsDbContext = uumsDbContext;
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
            var spec = new UserFilterSpecification(name);
            int total = _userRepository.Count(spec);
            var specPaginated = new UserFilterSpecification(pageIndex, pageSize, name);
            var users = _userRepository.Query(specPaginated);
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
            _unitOfWork.Add(entity);
            _unitOfWork.Commit();
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

            var user = _userRepository.Find(id);
            if (user == null)
            {
                return BadRequest("参数错误，id不存在");
            }
            var spec = new UserFilterSpecification(o => o.Account == param.Account);
            var sameAccountUser = _userRepository.FirstOrDefault(spec);
            if (user.Account != param.Account && sameAccountUser != null)
            {
                return BadRequest("参数错误，account已被使用");
            }
            user.Name = param.Name;
            user.Account = param.Account;
            user.Sex = param.Sex;
            user.Mobile = param.Mobile;
            user.Mail = param.Mail;
            _unitOfWork.Modify(user);
            _unitOfWork.Commit();

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
            var user = _userRepository.Find(id);
            if (user == null)
            {
                return BadRequest("参数错误，id不存在");
            }
            _unitOfWork.Remove(user);
            _unitOfWork.Commit();
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
            var user = _userRepository.Find(id);
            if (user == null)
            {
                return BadRequest("参数错误，id不存在");
            }
            var spec = new RoleFilterSpecification(roleids);
            var roles = _roleRepository.Query(spec);
            user.Roles = roles;
            _unitOfWork.Modify4Aggregate(user);
            _unitOfWork.Commit();
            return Ok();
        }       
    }
}
