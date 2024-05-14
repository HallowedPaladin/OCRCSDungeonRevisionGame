using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;


namespace InsigniaServer.Auth
{
    public class TokenUtility
    {
        private readonly string _jwtSecretString;
        private readonly int _tokenExpirationInMinutes = 90;
        private readonly string _jwtIssuer;

        public TokenUtility(string jwtSecretString, int tokenExpirationInMinutes, String jwtIssuer)
        {
            _jwtSecretString = jwtSecretString;
            _tokenExpirationInMinutes = tokenExpirationInMinutes;
            _jwtIssuer = jwtIssuer;
        }

        public string GenerateToken(int userId)
        {
            // TODO insert UserID into token
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretString));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var Sectoken = new JwtSecurityToken(_jwtIssuer,
              _jwtIssuer,
              null,
              expires: DateTime.Now.AddMinutes(_tokenExpirationInMinutes),
              signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            return token;
        }
    }
}