using E_BangApplication.CQRS.Command.ShopHandler;
using Microsoft.AspNetCore.Mvc;
using MyCustomMediator.Interfaces;

namespace E_BangAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShopStaffController :ControllerBase
    {
        private readonly ISender _sender;

        public ShopStaffController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("shop/{shopId}/shopstaff/add-shop-staff")]
        public async Task<IActionResult> AddShopStaff([FromRoute] string shopId, [FromBody] AddShopStaffCommand command, CancellationToken cancellationToken)
        {
            command.ShopId = shopId;
            var response = await _sender.SendToMediator(command, cancellationToken);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        //[HttpPatch("shop")]

    }
}
