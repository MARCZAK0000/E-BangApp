using Microsoft.AspNetCore.Mvc;

namespace E_BangNotificationService.Controller
{
    [ApiController]
    [Route("api/notification")]
    public class NotificationController:ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetNotifications(string AccountId, CancellationToken cancellationToken)
        {
            return Ok();
        }
    }
}
