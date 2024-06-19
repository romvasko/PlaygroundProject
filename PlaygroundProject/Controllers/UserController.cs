using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PlaygroundProject.Services.Interfaces;
using PlaygroundProject.ServicesResponse;
using PlaygroundProject.ViewModels;
using System.Security.Cryptography.X509Certificates;

namespace PlaygroundProject.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("category:RolesEnum")]
        public async Task<IActionResult> GetToken(Roles role = Roles.Admin)
        {
            var responce = await _userService.GetToken(role);
            if (!responce.Success)
            {
                return Ok(responce.ErrorMessage);
            }
            return Ok(responce.TokenViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminOnly()
        {
            var responce = new AdminOnlyResponse();
            return Ok(responce.Message);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserInfo()
        {
            var userInfoResult = _userService.GetUserInfo();
            return Ok(userInfoResult);
        }
    }
}
