﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PlaygroundProject.Services.Interfaces;
using PlaygroundProject.ServicesResponce;
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
            var responce = new AdminOnlyResponce();
            return Ok(responce.Message);
        }
    }
}
