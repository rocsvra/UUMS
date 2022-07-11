using AdunTech.AutoMapperExtension;
using AdunTech.CommonDomain;
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

namespace UUMS.API.Controllers
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class MenuController : UumsControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Menu> _menuRepository;

        public MenuController(IUnitOfWork unitOfWork, IRepository<Menu> menuRepository)
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
        public ActionResult<List<ElementMenuVO>> GetList(Guid clientId, string name)
        {
            var spec = new MenuFilterSpecification(clientId, name);
            var menus = _menuRepository.Query(spec);
            return menus.ToElementMenu();
        }

        /// <summary>
        ///  获取菜单父id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("ParentIds/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public ActionResult<List<Guid>> GetPIds(Guid id)
        {
            var menu = _menuRepository.Find(id);
            if (menu == null)
            {
                return BadRequest("参数错误");
            }
            var spec = new MenuFilterSpecification(menu.ClientId, string.Empty);
            var menus = _menuRepository.Query(spec);
            return menus.GetParentIds(id);
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
            _unitOfWork.Add(entity);
            _unitOfWork.Commit();
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
            var menu = _menuRepository.Find(id);
            if (menu == null)
            {
                return BadRequest("参数错误，id不存在");
            }

            //menu.ParentId = param.ParentId;
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
            _unitOfWork.Modify(menu);
            _unitOfWork.Commit();

            return Ok();
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public ActionResult Delete(Guid id)
        {
            var menu = _menuRepository.Find(id);
            if (menu == null)
            {
                return BadRequest("参数错误，id不存在");
            }
            _unitOfWork.Remove(menu);
            _unitOfWork.Commit();
            return Ok();
        }
    }
}
