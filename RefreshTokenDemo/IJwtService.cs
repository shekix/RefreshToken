using System.Security.Claims;

namespace RefreshTokenDemo
{
    public interface IJwtService
    {
        string GenerateAccessToken(string userId);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
