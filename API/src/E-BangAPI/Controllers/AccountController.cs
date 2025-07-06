using E_BangApplication.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_BangAPI.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        #region Post Request
        [HttpPost("register")]
        [Transaction]
        public Task<IActionResult> RegisterAsync()
        {
            throw new NotImplementedException();
        }
        [HttpPost("login")]
        public Task<IActionResult> LoginAsync()
        {
            throw new NotImplementedException();
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
        #endregion

        #region Get Request
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
        #endregion
    }
}
