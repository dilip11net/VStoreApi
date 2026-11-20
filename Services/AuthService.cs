using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VueStoreApi.Data;
using VueStoreApi.DTOs;
using VueStoreApi.Models;

namespace VueStoreApi.Services;

public class AuthService(StoreDbContext db, ITokenService tokenService) : IAuthService
{
    private readonly PasswordHasher<AppUser> _hasher = new();

    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        if (await db.Users.AnyAsync(u => u.Email == email))
            return null;

        var user = new AppUser { Name = request.Name.Trim(), Email = email };
        user.PasswordHash = _hasher.HashPassword(user, request.Password);
        db.Users.Add(user);
        await db.SaveChangesAsync();

        var (token, expiry) = tokenService.CreateToken(user);
        return new AuthResponse(user.Id, user.Name, user.Email, token, expiry);
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await db.Users.SingleOrDefaultAsync(u => u.Email == email);
        if (user is null) return null;

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed) return null;

        var (token, expiry) = tokenService.CreateToken(user);
        return new AuthResponse(user.Id, user.Name, user.Email, token, expiry);
    }
}
