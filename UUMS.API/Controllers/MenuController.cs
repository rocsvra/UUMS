using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace UUMS.API.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("UUMS")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        public MenuController()
        {

        }
    }
}
