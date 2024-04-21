using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GameServer.Auth
{
    public class TokenUtility
    {
        private readonly string _secretKey;
        private readonly double _tokenExpirationInMinutes;

        public TokenUtility(string secretKey, double tokenExpirationInMinutes)
        {
            _secretKey = secretKey;
            _tokenExpirationInMinutes = tokenExpirationInMinutes;
        }

        public string GenerateToken(int userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Convert.FromBase64String(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(_tokenExpirationInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                                            SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Convert.FromBase64String(_secretKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal;

        }
    }
}