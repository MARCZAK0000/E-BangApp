
using Microsoft.AspNetCore.Mvc;

namespace E_BangAPI.Controllers
{ 
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public async Task <IActionResult> Get()
        {
            return Ok(10);
        }
    }
}
