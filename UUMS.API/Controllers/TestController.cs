using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace UUMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            var data = new TestData
            {
                items = new List<string>
                {
                     "事项 A", "事项 B", "事项 C"
                }
            };
            return Ok(data);
        }
    }

    class TestData
    {
        public List<string> items { get; set; }
    }
}
