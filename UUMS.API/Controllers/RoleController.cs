using AdunTech.AutoMapperExtension;
using AdunTech.Co2Net.Models;
using AdunTech.CommonDomain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using UUMS.Application.Dtos;
using UUMS.Application.Specifications;
using UUMS.Application.Util;
using UUMS.Application.Vos;
using UUMS.Domain.DO;

namespace UUMS.API.Controllers
{
    public class RoleController : UumsControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<Menu> _menuRepository;
        private readonly IRepository<User> _userRepository;

        public RoleController(IUnitOfWork unitOfWork, IRepository<Role> roleRepository, IRepository<Menu> menuRepository, IRepository<User> userRepository)
        {
            _unitOfWork = unitOfWork;
            _roleRepository = roleRepository;
            _menuRepository = menuRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        ///  获取角色(分页)
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
        public ActionResult<PaginatedItems<RoleDto>> GetPage(Guid clientId, int pageIndex, int pageSize, string name)
        {
            var spec = new RoleFilterSpecification(clientId, name);
            int total = _roleRepository.Count(spec);
            var specPaginated = new RoleFilterSpecification(clientId, pageIndex, pageSize, name);
            var roles = _roleRepository.Query(specPaginated);
            var dtos = roles.Map<Role, RoleDto>();
            return new PaginatedItems<RoleDto>(pageIndex, pageSize, total, dtos);
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public ActionResult<RoleDto> Post(RoleVO param)
        {
            if (string.IsNullOrWhiteSpace(param.Name))
            {
                return BadRequest("参数错误");
            }
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst("username")?.Value;

            RoleDto dto = param.Map<RoleVO, RoleDto>();
            dto.Id = Guid.NewGuid();
            var entity = dto.Map<RoleDto, Role>();
            entity.CreatedAt = DateTime.Now;
            entity.CreatedBy = username;
            _unitOfWork.Add(entity);
            _unitOfWork.Commit();
            return dto;
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public ActionResult Put(Guid id, [FromBody] RoleVO param)
        {
            if (string.IsNullOrWhiteSpace(param.Name))
            {
                return BadRequest("参数错误");
            }
            var role = _roleRepository.Find(id);
            if (role == null)
            {
                return BadRequest("参数错误，id不存在");
            }
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst("username")?.Value;

            role.Name = param.Name;
            role.Description = param.Description;
            role.Enabled = param.Enabled;
            role.LastUpdatedBy = username;
            role.LastUpdatedAt = DateTime.Now;
            _unitOfWork.Modify(role);
            _unitOfWork.Commit();

            return Ok();
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public ActionResult Delete(Guid id)
        {
            var role = _roleRepository.Find(id);
            if (role == null)
            {
                return BadRequest("参数错误，id不存在");
            }
            _unitOfWork.Remove(role);
            _unitOfWork.Commit();
            return Ok();
        }

        /// <summary>
        /// 角色关联菜单
        /// </summary>
        /// <param name="id">角色id</param>
        /// <param name="menuids">菜单id列表</param>
        /// <returns></returns>
        [HttpPut("{id}/Menus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public ActionResult PutMenus(Guid id, [FromBody] List<Guid> menuids)
        {
            var spec = new RoleFilterSpecification(id, true, false);
            var role = _roleRepository.Query(spec).FirstOrDefault();
            if (role == null)
            {
                return BadRequest("参数错误，id不存在");
            }
            var specMenu = new MenuFilterSpecification(menuids);
            var menus = _menuRepository.Query(specMenu);
            role.Menus = menus;
            _unitOfWork.Modify(role);
            _unitOfWork.Commit();
            return Ok();
        }

        /// <summary>
        /// 获取角色关联菜单
        /// </summary>
        /// <param name="id">角色id</param>
        /// <returns></returns>
        [HttpGet("{id}/Menus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public ActionResult<List<Guid>> GetMenus(Guid id)
        {
            var spec = new RoleFilterSpecification(id, true, false);
            var role = _roleRepository.Query(spec).FirstOrDefault();
            return role.Menus?.Select(o => o.Id).ToList();
        }

        /// <summary>
        /// 获取角色关联用户
        /// </summary>
        /// <param name="id">角色id</param>
        /// <returns></returns>
        [HttpGet("{id}/Users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public ActionResult<List<UserDto>> GetUsers(Guid id)
        {
            var spec = new RoleFilterSpecification(id, false, true);
            var role = _roleRepository.Query(spec).FirstOrDefault();
            List<UserDto> dtos = role.Users?.Map<User, UserDto>().ToList();
            return dtos;
        }

        /// <summary>
        /// 角色关联用户（删除）
        /// </summary>
        /// <param name="id">角色id</param>
        /// <param name="userids">用户id列表</param>
        /// <returns></returns>
        [HttpDelete("{id}/Users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteUsers(Guid id, [FromBody] List<Guid> userids)
        {
            var spec = new RoleFilterSpecification(id, false, true);
            var role = _roleRepository.Query(spec).FirstOrDefault();
            if (role == null)
            {
                return BadRequest("参数错误，id不存在");
            }
            var specUsers = new UserFilterSpecification(userids);
            var users = _userRepository.Query(specUsers);
            foreach (var user in users)
            {
                role.Users.Remove(user);
            }
            _unitOfWork.Modify(role);
            _unitOfWork.Commit();
            return Ok();
        }

        /// <summary>
        /// 角色关联用户（新增）
        /// </summary>
        /// <param name="id">角色id</param>
        /// <param name="userids">用户id列表</param>
        /// <returns></returns>
        [HttpPost("{id}/Users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public ActionResult PostUsers(Guid id, [FromBody] List<Guid> userids)
        {
            var spec = new RoleFilterSpecification(id, false, true);
            var role = _roleRepository.Query(spec).FirstOrDefault();
            if (role == null)
            {
                return BadRequest("参数错误，id不存在");
            }
            var specUsers = new UserFilterSpecification(userids);
            var users = _userRepository.Query(specUsers);
            foreach (var user in users)
            {
                role.Users.Add(user);
            }
            _unitOfWork.Modify(role);
            _unitOfWork.Commit();
            return Ok();
        }

        /// <summary>
        /// 获取角色关联用户
        /// </summary>
        /// <param name="id">角色id</param>
        /// <returns></returns>
        [HttpGet("{id}/UnlinkedUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public ActionResult<List<UserDto>> GetUnlinkedUsers(Guid id, string name)
        {
            var spec = new RoleFilterSpecification(id, false, true);
            var role = _roleRepository.Query(spec).FirstOrDefault();
            var specUsers = new UserFilterSpecification(name);
            var users = _userRepository.Query(specUsers);
            var unlinkedUsers = users.Except(role.Users);
            List<UserDto> dtos = unlinkedUsers?.Map<User, UserDto>().ToList();
            return dtos;
        }
    }
}
