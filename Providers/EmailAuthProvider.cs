using Microsoft.EntityFrameworkCore;
using MusicStreaming.Api.Data;
using MusicStreaming.Api.DTOs.Auth;
using MusicStreaming.Api.Services;

public class EmailAuthProvider : IAuthProvider
{
    private readonly AppDbContext _db;
    private readonly JwtService _jwt;

    public EmailAuthProvider(AppDbContext db, JwtService jwt)
    {
        _db = db;
        _jwt = jwt;
    }

    public async Task<string?> LoginAsync(object request)
    {
        var dto = request as LoginDto;

        var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
        if (user == null) return null;

        var valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!valid) return null;

        return _jwt.GenerateToken(user);
    }
}