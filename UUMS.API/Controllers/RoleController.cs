using AdunTech.AutoMapperExtension;
using AdunTech.Co2Net.Models;
using AdunTech.CommonDomain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using UUMS.Application.Dtos;
using UUMS.Domain.DO;
using UUMS.Domain.IRepositories;

namespace UUMS.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [EnableCors("policy")]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoleRepository _roleRepository;

        public RoleController(IUnitOfWork unitOfWork, IRoleRepository roleRepository)
        {
            _unitOfWork = unitOfWork;
            _roleRepository = roleRepository;
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
        public ActionResult<PaginatedItems<RoleDto>> GetPage(int pageIndex, int pageSize, string name)
        {
            Expression<Func<Role, bool>> predicate = o => string.IsNullOrEmpty(name) || o.Name.Contains(name);
            int total = _roleRepository.Query(predicate).Count();
            var roles = _roleRepository
                .Page(pageSize, pageIndex, predicate, o => o.Name)
                .ToList();
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
        public ActionResult<RoleDto> Post(RoleModel param)
        {
            if (string.IsNullOrWhiteSpace(param.Name))
            {
                return BadRequest("参数错误");
            }
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst("username")?.Value;

            RoleDto dto = param.Map<RoleModel, RoleDto>();
            dto.Id = Guid.NewGuid();
            var entity = dto.Map<RoleDto, Role>();
            entity.CreatedAt = DateTime.Now;
            entity.CreatedBy = username;
            _unitOfWork.AddAndCommit(entity);
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
        public ActionResult Put(Guid id, [FromBody] RoleModel param)
        {
            if (string.IsNullOrWhiteSpace(param.Name))
            {
                return BadRequest("参数错误");
            }
            if (!_roleRepository.Exists(id))
            {
                return BadRequest("参数错误，id不存在");
            }
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst("username")?.Value;

            var role = _roleRepository.Find(id);
            role.Name = param.Name;
            role.Description = param.Description;
            role.Enabled = param.Enabled;
            role.LastUpdatedBy = username;
            role.LastUpdatedAt = DateTime.Now;
            _unitOfWork.ModifyAndCommit(role);

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
            if (!_roleRepository.Exists(id))
            {
                return BadRequest("参数错误，id不存在");
            }
            var roles = _roleRepository.Find(id);
            _unitOfWork.Remove(roles);
            _unitOfWork.Commit();
            return Ok();
        }
    }
}
