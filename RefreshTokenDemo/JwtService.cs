using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RefreshTokenDemo
{
    public class JwtService : IJwtService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _accessTokenExpirationMinutes;
        private readonly int _refreshTokenExpirationDays;

        public JwtService(IConfiguration config)
        {
            _secretKey = config["JwtSettings:SecretKey"]!;
            _issuer = config["JwtSettings:Issuer"]!;
            _audience = config["JwtSettings:Audience"]!;
            _accessTokenExpirationMinutes = int.Parse(config["JwtSettings:AccessTokenExpirationMinutes"]!);
            _refreshTokenExpirationDays = int.Parse(config["JwtSettings:RefreshTokenExpirationDays"]!);
        }

        public string GenerateAccessToken(string userId)
        {
            var claims = new[]
            {             
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.Now.AddMinutes(_accessTokenExpirationMinutes);

            var token = new JwtSecurityToken(_issuer, _audience, claims, expires: expiration, signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }


        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false, 
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey))
            }, out var validatedToken);

            return principal;
        }
    }
}
