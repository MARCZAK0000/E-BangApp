using E_BangDomain.Repository;
using E_BangDomain.ResponseDtos.SharedResponseDtos;
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
        public Task<IActionResult> GetUser()
        {
            // This is a placeholder for the actual implementation
            return Task.FromResult<IActionResult>(Ok("User data would be returned here."));
        }

        [HttpPost("update")]
        public Task<IActionResult> UpdateUserInfromations(string id)
        {
            // This is a placeholder for the actual implementation
            return Task.FromResult<IActionResult>(Ok($"User data for ID {id} would be returned here."));
        }
        [HttpPost("changePassword")]
        public Task<IActionResult> ChangePassword(string id)
        {
            // This is a placeholder for the actual implementation
            return Task.FromResult<IActionResult>(Ok($"Password for user ID {id} would be changed here."));
        }
    }   
}
