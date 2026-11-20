using VueStoreApi.Models;   

namespace VueStoreApi.Services;

public interface ITokenService
{
    (string Token, DateTime ExpiresAtUtc) CreateToken(AppUser user);
}
    