using HEVEQ.Application.Features.Auth.Commad.Login;
using HEVEQ.Application.Features.Auth.Commad.Logout;
using HEVEQ.Application.Features.Auth.Commad.RefreshToken;
using HEVEQ.Application.Features.Auth.Commad.Regiser;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> refreshToken(RefreshTokenCommand command)
        {
            var result =await mediator.Send(command);
            if(!result.IsAuthenticated)
                return BadRequest(result.Message);
            return Ok(result);
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
    }
}
