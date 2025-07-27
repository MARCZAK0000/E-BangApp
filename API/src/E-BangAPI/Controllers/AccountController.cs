using E_BangApplication.Attributes;
using E_BangApplication.CQRS.Command.AccountHandler;
using E_BangApplication.CQRS.Query.AccountHandler;
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

        [Transaction]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterAccountCommand register, CancellationToken token)
        {
            var response = await _sender.SendToMediator(register, token);
            return response.IsSuccess ?
                Created(string.Empty, response) :
                BadRequest(response);

        }

        [Transaction]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] VerifyCredentialsCommand verifyCredentials, CancellationToken token)
        {
            var response = await _sender.SendToMediator(verifyCredentials, token);
            return response.IsSuccess && !string.IsNullOrEmpty(response.TwoWayToken) ?
                Ok(response) :
                NotFound(response);
        }

        [Transaction]
        [HttpPost("loginWithTwoWay")]
        public async Task<IActionResult> LoginWithTwoWayAsync([FromBody] ValidateCredentialsTwoWayTokenCommand validateCredentials, CancellationToken token)
        {
            var response = await _sender.SendToMediator(validateCredentials, token);
            return response.IsSuccess ?
                Ok(response) :
                NotFound(response);
        }

        [HttpGet("logout")]
        public async Task<IActionResult> LogoutAsync(SignOutQuery signOut, CancellationToken token)
        {
            var response = await _sender.SendToMediator(signOut, token);
            return response.IsSuccess ?
                Ok(response) :
                BadRequest(response);
        }

        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshTokenAsync(CancellationToken token)
        {
            var response = await _sender.SendToMediator(new RefreshTokenCommand(), token);
            return response.IsSuccess ?
                Ok(response) :
                BadRequest(response);
        }
        [Transaction]
        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordCommand reset, CancellationToken token)
        {
            var response = await _sender.SendToMediator(reset, token);
            return response.IsSuccess ?
                Ok(response) :
                BadRequest(response);
        }

        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] ConfirmEmailCommand confirmEmailCommand, CancellationToken token)
        {
            var response = await _sender.SendToMediator(confirmEmailCommand, token);
            return response.IsSuccess ?
                Ok(response) :
                BadRequest(response);
        }

        [HttpGet("resendConfimEmail")]
        public async Task<IActionResult> ResendConfirmEmailAsync(CancellationToken token)
        {
            var response = await _sender.SendToMediator(new ReSendConfirmEmailQuery(), token);
            return response.IsSuccess ?
                Ok(response) :
                BadRequest(response);
        }

        [HttpGet("forgotPassword")]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordTokenQuery forgotPassword, CancellationToken token)
        {
            var response = await _sender.SendToMediator(forgotPassword, token);
            return response.IsSuccess ?
                Ok(response) :
                BadRequest(response);
        }

    }
}
