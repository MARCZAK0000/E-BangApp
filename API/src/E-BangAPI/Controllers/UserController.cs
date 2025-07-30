using E_BangApplication.Attributes;
using E_BangApplication.CQRS.Command.UserHandler;
using E_BangApplication.CQRS.Query.UserHandler;
using Microsoft.AspNetCore.Mvc;
using MyCustomMediator.Interfaces;

namespace E_BangAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ISender _sender;
        public UserController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetUser(CancellationToken token)
        {
            var response = await _sender.SendToMediator(new GetUserQuery(), token);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        [Transaction]
        [HttpPatch("update")]
        public async Task<IActionResult> UpdateUserInfromations(UpdateUserCommand reqeust, CancellationToken token)
        {
            var response = await _sender.SendToMediator(reqeust, token);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        [Transaction]
        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword(UpdatePasswordCommand request, CancellationToken token)
        {
            var result = await _sender.SendToMediator(new UpdatePasswordCommand(), token);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
