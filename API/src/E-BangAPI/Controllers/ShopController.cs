using E_BangApplication.CQRS.Command.ShopHandler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
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

        [HttpPost("{shopId}/create-branches")]
        public async Task<IActionResult> CreateShopBranches([FromRoute] string shopId, [FromBody] CreateShopBranchesCommand request, CancellationToken cancellationToken)
        {
            request.ShopId = shopId;
            var response = await _sender.SendToMediator(request, cancellationToken);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPatch("update-shop")]
        public async Task<IActionResult> UpdateShop([FromBody] UpdateShopCommand command, CancellationToken cancellationToken)
        {
            var response = await _sender.SendToMediator(command, cancellationToken);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [HttpPatch("{shopId}/update-branch")]
        public async Task<IActionResult> UpdateShopBranch([FromRoute] string shopId, [FromBody] UpdateBranchCommand command, CancellationToken cancellationToken)
        {
            command.ShopID = shopId;
            var response = await _sender.SendToMediator(command, cancellationToken);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [HttpDelete("{shopId}/delete-branch/{branch_id}]")]
        public async Task<IActionResult> DeleteShopBranch([FromRoute] string shopId, [FromRoute] string branch_id, CancellationToken cancellationToken)
        {
            var response = await _sender.SendToMediator(new RemoveShopBranchesCommand { ShopID = shopId, BranchId = branch_id }, cancellationToken);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [HttpDelete("{shopId}/delete-shop")]
        public async Task<IActionResult> DeleteShop([FromRoute] string shopId, CancellationToken cancellationToken)
        {
            var response = await _sender.SendToMediator(new RemoveShopCommand { ShopID = shopId }, cancellationToken);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

    }
}
