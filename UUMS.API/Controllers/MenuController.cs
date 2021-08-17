using AdunTech.AutoMapperExtension;
using AdunTech.CommonDomain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UUMS.Application.Dtos;
using UUMS.Domain.DO;
using UUMS.Domain.IRepositories;

namespace UUMS.API.Controllers
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class MenuController : UumsControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMenuRepository _menuRepository;

        public MenuController(IUnitOfWork unitOfWork, IMenuRepository menuRepository)
        {
            _unitOfWork = unitOfWork;
            _menuRepository = menuRepository;
        }

        /// <summary>
        ///  获取客户端菜单列表
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{clientId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public ActionResult<List<MenuDto>> GetList(Guid clientId, string name)
        {
            Expression<Func<Menu, bool>> predicate = o =>
                (string.IsNullOrEmpty(name) || o.Name.Contains(name))
                && o.ClientId == clientId;
            var menus = _menuRepository
                .Query(predicate, o => o.SortNo)
                .ToList();
            return menus.Map<Menu, MenuDto>().ToList();
        }

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public ActionResult<MenuDto> Post(MenuDto param)
        {
            if (string.IsNullOrWhiteSpace(param.Name))
            {
                return BadRequest("参数错误");
            }

            param.Id = Guid.NewGuid();
            var entity = param.Map<MenuDto, Menu>();
            entity.CreatedAt = DateTime.Now;
            entity.CreatedBy = LoginUserName;
            _unitOfWork.AddAndCommit(entity);
            return param;
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public ActionResult Put(Guid id, [FromBody] MenuDto param)
        {
            if (string.IsNullOrWhiteSpace(param.Name))
            {
                return BadRequest("参数错误");
            }
            if (!_menuRepository.Exists(id))
            {
                return BadRequest("参数错误，id不存在");
            }

            var menu = _menuRepository.Find(id);
            menu.ParentId = param.ParentId;
            menu.Name = param.Name;
            menu.AlwaysShow = param.AlwaysShow;
            menu.Hidden = param.Hidden;
            menu.Redirect = param.Redirect;
            menu.Path = param.Path;
            menu.Component = param.Component;
            menu.Title = param.Title;
            menu.Icon = param.Icon;
            menu.NoCache = param.NoCache;
            menu.SortNo = param.SortNo;
            menu.LastUpdatedBy = LoginUserName;
            menu.LastUpdatedAt = DateTime.Now;
            _unitOfWork.ModifyAndCommit(menu);

            return Ok();
        }
    }
}
