using HEVEQ.Application.Features.Auth.Commad.Login;
using HEVEQ.Application.Features.Auth.Commad.Logout;
using HEVEQ.Application.Features.Auth.Commad.RefreshToken;
using HEVEQ.Application.Features.Auth.Commad.Regiser;
using HEVEQ.Application.Features.Auth.Query.Me;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HEVEQ.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IMediator mediator) : ControllerBase
    {

        [HttpPost("Register")]
        public async Task<IActionResult> register(RegisterComand comand)
        {
            var result =  await mediator.Send(comand);
            if(result.IsAuthenticated)
                return Ok(result);
            return BadRequest(result.Message);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> login(LoginCommand command)
        {
            var result = await mediator.Send(command);
            if (result.IsAuthenticated)
                return Ok(result);
            return BadRequest(result.Message);
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var result = await mediator.Send(command);

            if (!result.IsAuthenticated)
            {
                return Unauthorized(new { message = result.Message });
            }
            return Ok(new
            {
                accessToken = result.AccessToken,
                refreshToken = result.RefreshToken
            });
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(LogoutCommand command)
        {
            var result = await mediator.Send(command);

            if (!result)
                return BadRequest("Invalid Refresh Token");

            return Ok(new
            {
                Message = "Logged out successfully"
            });
        }

        [Authorize] // هذا الـ Attribute يضمن أن الطلب القادم يحتوي على JWT صالح
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {

            var userIdString = User.FindFirstValue("uid");

            // 2. التحقق من وجوده وتحويله إلى Guid بأمان
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userIdGuid))
            {
                return Unauthorized(new { message = "Invalid Token Claims" });
            }

            // 3. تمرير الـ Guid إلى الـ Query
            var query = new MeQuery(userIdGuid);
            var result = await mediator.Send(query);

            if (!result.IsSuccess)
            {
                if (result.StatusCode == 403)
                    return StatusCode(403, new { message = result.Message });

                return NotFound(new { message = result.Message });
            }

            return Ok(new
            {
                id = result.Id, // سيتم تحويل الـ Guid إلى نص تلقائياً في الـ JSON
                displayName = result.DisplayName,
                email = result.Email,
                phoneNumber = result.PhoneNumber,
                role = result.Role,
                isActive = result.IsActive,
                profileCompleted = result.ProfileCompleted,
                trustScore = result.TrustScore,
                dashboardUrl = result.DashboardUrl
            });
        }
            
    }

}
