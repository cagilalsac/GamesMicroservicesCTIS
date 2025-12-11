using MediatR;
using Microsoft.AspNetCore.Mvc;
using Users.APP.Features.Tokens;

namespace Users.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly IConfiguration _configuration;

        public TokensController(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        // POST: Token
        [HttpPost]
        [Route("~/api/[action]")]
        public async Task<IActionResult> Token(TokenRequest request)
        {
            request.SecurityKey = _configuration["SecurityKey"];
            request.Audience = _configuration["Audience"];
            request.Issuer = _configuration["Issuer"];
            if (ModelState.IsValid)
            {
                var response = await _mediator.Send(request);
                if (response is null)
                    return BadRequest("Invalid user name or password!");
                return Ok(response);
            }
            return BadRequest(ModelState);
        }

        // POST: RefreshToken
        [HttpPost]
        [Route("~/api/[action]")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            request.SecurityKey = _configuration["SecurityKey"];
            request.Audience = _configuration["Audience"];
            request.Issuer = _configuration["Issuer"];
            if (ModelState.IsValid)
            {
                var response = await _mediator.Send(request);
                if (response is null)
                    return BadRequest("User not found!");
                return Ok(response);
            }
            return BadRequest(ModelState);
        }
    }
}
