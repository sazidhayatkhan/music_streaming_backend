using Microsoft.EntityFrameworkCore;
using Google.Apis.Auth;
using MusicStreaming.Api.Data;
using MusicStreaming.Api.DTOs.Auth;
using MusicStreaming.Api.Entities;
using MusicStreaming.Api.Services;

public class GoogleAuthProvider : IAuthProvider
{
    private readonly AppDbContext _db;
    private readonly JwtService _jwt;
    private readonly IConfiguration _config;

    public GoogleAuthProvider(AppDbContext db, JwtService jwt, IConfiguration config)
    {
        _db = db;
        _jwt = jwt;
        _config = config;
    }

    public async Task<string?> LoginAsync(object request)
    {
        var dto = request as GoogleLoginDto;

        var payload = await GoogleJsonWebSignature.ValidateAsync(
            dto.IdToken,
            new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _config["GoogleAuth:ClientId"] }
            });

        var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == payload.Email);

        if (user == null)
        {
            user = new User
            {
                Name = payload.Name,
                Email = payload.Email,
                GoogleId = payload.Subject,
                IsGoogleUser = true
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }

        return _jwt.GenerateToken(user);
    }

    public Task<string?> RegisterAsync(object request)
    {
        throw new NotImplementedException("Google does not support manual registration");
    }
}