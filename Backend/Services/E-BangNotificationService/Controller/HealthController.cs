using E_BangNotificationService.AppInfo;
using Microsoft.AspNetCore.Mvc;

namespace E_BangNotificationService.Controller
{
    [ApiController]
    [Route("api/health")]
    public class HealthController : ControllerBase
    {
        private readonly IInformations _informations;

        public HealthController(IInformations informations)
        {
            _informations = informations;
        }

        [HttpGet]
        public IActionResult HealthInfo()
        {
            return Ok(_informations);
        }
    }
}
