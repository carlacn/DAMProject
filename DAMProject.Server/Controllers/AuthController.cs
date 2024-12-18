﻿using DAMProject.Server.Auth;
using DAMProject.Shared.Auth;
using DAMProject.Shared.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DAMProject.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("check-admin-privilege")]
        public ActionResult<string> GetAdminResource()
        {
            return "Este es un recurso solo para administradores.";
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpGet("check-user-privilege")]
        public ActionResult<string> GetUserResource()
        {
            return "Este es un recurso para usuarios regulares y administradores.";
        }

        [HttpGet("userID")]
        public ActionResult<int> GetUserId()
        {
            var userId = _authService.GetUserId();
            if (userId == null)
            {
                return NotFound();
            }
            return userId;

        }

        [HttpGet("status")]
        public ActionResult<AuthenticationStatus> GetAuthenticationStatus()
        {
            return _authService.GetUserAuthenticationStatus();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var auth = await _authService.Login(request);

            if (auth == false)
                return Unauthorized();

            return Ok();
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userExists = await _authService.UserExists(registerRequest.Email);
            if (userExists)
                return Conflict(new { message = "El usuario ya existe." });

            await _authService.Register(registerRequest);
            return Ok(new { message = "Usuario registrado exitosamente." });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            _authService.LogOut();
            return Ok();
        }
    }
}

