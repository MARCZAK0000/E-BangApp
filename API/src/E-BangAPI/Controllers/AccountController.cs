using E_BangApplication.Attributes;
using E_BangApplication.CQRS.Command;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCustomMediator.Interfaces;
namespace E_BangAPI.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly ISender _sender;
        public AccountController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("register")]
        [Transaction]
        public Task<IActionResult> RegisterAsync()
        {
            throw new NotImplementedException();
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(ValidatCredentialsTwoWayToken signInCommand, CancellationToken token)
        {
            var response = await _sender.SendToMediatoR(signInCommand, token);
            if (!response.IsSuccess)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("loginWithTwoWay)")]
        public async Task<IActionResult> LoginWithTwoWayAsync(ValidatCredentialsTwoWayToken signInCommand, CancellationToken token)
        {
            var response = await _sender.SendToMediatoR(signInCommand, token);
            if (!response.IsSuccess)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpPost("logout")]
        public Task<IActionResult> LogoutAsync()
        {
            throw new NotImplementedException();
        }

        [HttpPost("refreshToken")]
        public Task<IActionResult> RefreshTokenAsync()
        {
            throw new NotImplementedException();
        }
        [Transaction]
        [HttpPost("resetPasswor")]
        public Task<IActionResult> ResetPasswordAsync()
        {
            throw new NotImplementedException();
        }
        [Authorize]
        [Transaction]
        [HttpPost("updateInformations")]
        public Task<IActionResult> UpdateInformationsAsync()
        {
            throw new NotImplementedException();
        }



        [HttpGet("confirmEmail")]
        public Task<IActionResult> ConfirmEmailAsyncAsync()
        {
            throw new NotImplementedException();
        }

        [HttpGet("resendConfimEmail")]
        public Task<IActionResult> ResendConfirmEmailAsync()
        {
            throw new NotImplementedException();
        }
        [HttpGet("forgotPassword")]
        public Task<IActionResult> ForgotPasswordAsync()
        {
            throw new NotImplementedException();
        }
        [Authorize]
        [HttpGet("me")]
        public Task<IActionResult> AccountInfo()
        {
            throw new NotImplementedException();
        }

    }
}
