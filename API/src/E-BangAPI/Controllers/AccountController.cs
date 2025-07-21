using E_BangApplication.Attributes;
using E_BangApplication.CQRS.Command.AccountCommand;
using E_BangApplication.CQRS.Query.AccountHandler;
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

        [Transaction]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterAccountCommand register, CancellationToken token)
        {
            var response = await _sender.SendToMediatoR(register, token);
            return response.IsSuccess ?
                Created(string.Empty, response) :
                BadRequest(response);

        }

        [Transaction]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] VerifyCredentialsCommand verifyCredentials, CancellationToken token)
        {
            var response = await _sender.SendToMediatoR(verifyCredentials, token);
            return response.IsSuccess && !string.IsNullOrEmpty(response.TwoWayToken)?
                Ok(response):
                NotFound(response);
        }

        [Transaction]
        [HttpPost("loginWithTwoWay)")]
        public async Task<IActionResult> LoginWithTwoWayAsync([FromBody] ValidateCredentialsTwoWayTokenCommand validateCredentials, CancellationToken token)
        {
            var response = await _sender.SendToMediatoR(validateCredentials, token);
            return response.IsSuccess?
                Ok(response) : 
                NotFound(response);
        }

        [HttpGet("logout")]
        public async Task<IActionResult> LogoutAsync(SignOutQuery signOut, CancellationToken token)
        {
           var response = await _sender.SendToMediatoR(signOut, token);
            return response.IsSuccess ?
                Ok(response) :
                BadRequest(response);
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

    }
}
