using Microsoft.AspNetCore.Mvc;
using MusicStreaming.Api.DTOs.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthFactory _factory;

    public AuthController(AuthFactory factory)
    {
        _factory = factory;
    }

    [HttpPost("email")]
    public async Task<IActionResult> EmailLogin(LoginDto dto)
    {
        var provider = _factory.Create("email");
        var token = await provider.LoginAsync(dto);

        return token == null ? Unauthorized() : Ok(new { token });
    }

    [HttpPost("google")]
    public async Task<IActionResult> GoogleLogin(GoogleLoginDto dto)
    {
        var provider = _factory.Create("google");
        var token = await provider.LoginAsync(dto);

        return token == null ? Unauthorized() : Ok(new { token });
    }
}
