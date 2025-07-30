using E_BangApplication.CQRS.Command.ShopHandler;
using Microsoft.AspNetCore.Mvc;
using MyCustomMediator.Interfaces;

namespace E_BangAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShopController : ControllerBase
    {
        private readonly ISender _sender;

        public ShopController(ISender sender)
        {
            _sender = sender;
        }
        [HttpPost("add-shop-staff")]
        public async Task<IActionResult> AddShopStaff([FromBody] AddShopStaffCommand command, CancellationToken cancellationToken)
        {
            var response = await _sender.SendToMediator(command, cancellationToken);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("create-shop")]
        public async Task<IActionResult> CreateShop([FromBody] CreateShopCommand command, CancellationToken cancellationToken)
        {
            var response = await _sender.SendToMediator(command, cancellationToken);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("create-branches")]
        public async Task<IActionResult> CreateShopBranches([FromBody] CreateShopBranchesCommand request, CancellationToken cancellationToken)
        {
            var response = await _sender.SendToMediator(request, cancellationToken);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPatch("update-branch")]
        public async Task<IActionResult> UpdateShop([FromBody]UpdateShopCommand command, CancellationToken cancellationToken)
        {
            var response = await _sender.SendToMediator(command, cancellationToken);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
