using AdunTech.AutoMapperExtension;
using AdunTech.Co2Net.Models;
using AdunTech.CommonDomain;
using AdunTech.ExceptionDetail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using UUMS.Application.Dtos;
using UUMS.Application.Specifications;
using UUMS.Domain.DO;

namespace UUMS.API.Controllers
{
    public class ClientController : UumsControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Client> _clientRepository;

        public ClientController(IRepository<Client> clientRepository, IUnitOfWork unitOfWork)
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
            var spec = new ClientFilterSpecification(name);
            int total = _clientRepository.Count(spec);
            var specPaginated = new ClientFilterSpecification(pageIndex, pageSize, name);
            var clients = _clientRepository.Query(specPaginated);
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
        public ActionResult<ClientDto> Post(ClientVO param)
        {
            if (string.IsNullOrWhiteSpace(param.Name))
            {
                throw new BadRequestException(HttpContext.TraceIdentifier, "ParameterError");
            }
            var dto = param.Map<ClientVO, ClientDto>();
            dto.Id = Guid.NewGuid();
            var entity = dto.Map<ClientDto, Client>();
            _unitOfWork.Add(entity);
            _unitOfWork.Commit();
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
        public ActionResult Put(Guid id, [FromBody] ClientVO param)
        {
            if (string.IsNullOrWhiteSpace(param.Name))
            {
                throw new BadRequestException(HttpContext.TraceIdentifier, "ParameterError");
            }
            if (_clientRepository.Find(id) == null)
            {
                throw new BadRequestException(HttpContext.TraceIdentifier, "ParameterError", "id不存在");
            }

            var entity = param.Map<ClientVO, Client>();
            entity.Id = id;
            _unitOfWork.Modify(entity);
            _unitOfWork.Commit();
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
