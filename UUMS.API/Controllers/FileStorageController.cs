using AdunTech.CommonDomain;
using AdunTech.FSS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using UUMS.Application.Vos;
using UUMS.Domain.DO;

namespace UUMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileStorageController : ControllerBase
    {
        private readonly IRepository<FssInfo> _fssinfoRepository;
        private readonly IFssService _fssService;
        private readonly IUnitOfWork _unitOfWork;

        public FileStorageController(IRepository<FssInfo> fssinfoRepository, IFssService fssService, IUnitOfWork unitOfWork)
        {
            _fssinfoRepository = fssinfoRepository;
            _fssService = fssService;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 图片上传(base64)
        /// 支持 ".pdf",".bmp",".jpg",".png",".jpeg"
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(UploadFileVO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [DisableRequestSizeLimit]
        [HttpPost("Base64")]
        public ActionResult<UploadFileVO> PostBase64(Base64VO param)
        {
            if (!string.IsNullOrEmpty(param.Name)
                && param.Name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                return BadRequest("文件名不能包含下列任何字符：\\/:*?\"<>|");
            }

            List<string> extensions = new List<string>
            {
                ".pdf",".bmp",".jpg",".png",".jpeg"
            };

            if (string.IsNullOrEmpty(param.Extension) || string.IsNullOrEmpty(param.Content))
            {
                return BadRequest("参数错误！");
            }
            param.Extension = param.Extension.StartsWith('.')
                ? param.Extension.ToLower()
                : string.Format(".{0}", param.Extension.ToLower());

            if (!extensions.Contains(param.Extension.ToLower()))
            {
                return BadRequest("不支持此扩展名");
            }

            byte[] buffer = Convert.FromBase64String(param.Content);
            Guid id = Guid.NewGuid();
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                string filePath = _fssService.UploadFile(stream, param.Extension);
                _unitOfWork.Add(new FssInfo
                {
                    Id = id,
                    FileName = filePath,
                    FileSize = buffer.Length,
                    Extension = param.Extension,
                    ContentType = "",
                    CreatedAt = DateTime.Now
                });

                return new UploadFileVO()
                {
                    id = id.ToString(),
                    name = "",
                    url = filePath
                };
            }
        }
    }
}
