using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace UUMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("policy")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UumsControllerBase : ControllerBase
    {
        /// <summary>
        /// 登录用户ID
        /// </summary>
        protected Guid LoginUserId
        {
            get
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                string userid = claimsIdentity.FindFirst("userid")?.Value;
                return string.IsNullOrEmpty(userid) ? default : new Guid(userid);
            }
        }

        /// <summary>
        /// 登录用户名
        /// </summary>
        protected string LoginUserName
        {
            get
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                return claimsIdentity.FindFirst("username")?.Value;
            }
        }
    }
}
