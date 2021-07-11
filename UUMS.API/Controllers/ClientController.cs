using AdunTech.AutoMapperExtension;
using AdunTech.Co2Net.Models;
using AdunTech.CommonDomain;
using AdunTech.ExceptionDetail;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using UUMS.Application.Dtos;
using UUMS.Domain.DO;
using UUMS.Domain.IRepositories;

namespace UUMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("UUMS")]
    public class ClientController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClientRepository _clientRepository;

        public ClientController(IClientRepository clientRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _clientRepository = clientRepository;
        }

        /// <summary>
        ///  获取客户端(分页)
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
        public ActionResult<PaginatedItems<ClientDto>> GetPage(int pageIndex, int pageSize, string name)
        {
            int total = _clientRepository.All().Count();
            var clients = _clientRepository
                .Page(pageSize, pageIndex, o => string.IsNullOrEmpty(name) || o.Name.Contains(name), o => o.SortNo)
                .ToList();
            var dtos = clients.Map<Client, ClientDto>();
            return new PaginatedItems<ClientDto>(pageIndex, pageSize, total, dtos);
        }

        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public ActionResult<ClientDto> Get(Guid id)
        {
            var entity = _clientRepository.Find(id);
            if (entity == null)
            {
                throw new BadRequestException(HttpContext.TraceIdentifier, "ParameterError", "id不存在");
            }
            return entity.Map<Client, ClientDto>();
        }

        /// <summary>
        /// 添加客户端
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public ActionResult<ClientDto> Post(ClientModel param)
        {
            if (string.IsNullOrWhiteSpace(param.Name))
            {
                throw new BadRequestException(HttpContext.TraceIdentifier, "ParameterError");
            }
            var dto = param.Map<ClientModel, ClientDto>();
            dto.Id = Guid.NewGuid();
            var entity = dto.Map<ClientDto, Client>();
            _unitOfWork.AddAndCommit(entity);
            return dto;
        }

        /// <summary>
        /// 修改客户端
        /// </summary>
        /// <param name="id"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public ActionResult Put(Guid id, [FromBody] ClientModel param)
        {
            if (string.IsNullOrWhiteSpace(param.Name))
            {
                throw new BadRequestException(HttpContext.TraceIdentifier, "ParameterError");
            }
            if (!_clientRepository.Exists(id))
            {
                throw new BadRequestException(HttpContext.TraceIdentifier, "ParameterError", "id不存在");
            }

            var entity = param.Map<ClientModel, Client>();
            entity.Id = id;
            _unitOfWork.ModifyAndCommit(entity);
            return Ok();
        }

        /// <summary>
        /// 删除客户端
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public ActionResult Post(Guid id)
        {
            var entity = _clientRepository.Find(id);
            if (entity == null)
            {
                throw new BadRequestException(HttpContext.TraceIdentifier, "ParameterError", "id不存在");
            }

            _unitOfWork.Remove(entity);
            _unitOfWork.Commit();
            return Ok();
        }
    }
}
