using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace GameServer.Auth
{
    public class TokenUtility : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly string _secretKey;
        private readonly int _tokenExpirationInMinutes = 90;

        //public TokenUtility(string secretKey, double tokenExpirationInMinutes)
        //{
        //    _secretKey = secretKey;
        //    _tokenExpirationInMinutes = tokenExpirationInMinutes;
        //}

        public TokenUtility(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            var secretGenerator = new SecretGenerator();
            _secretKey = SecretGenerator.GenerateSecret(_tokenExpirationInMinutes);
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

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                // Retrieve the authorization header from the request
                if (!Request.Headers.ContainsKey("Authorization"))
                {
                    // No authorization header found, return failure
                    return AuthenticateResult.Fail("Authorization header not found.");
                }

                // Get the value of the Authorization header
                string authorizationHeader = Request.Headers["Authorization"];

                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    // Authorization header is present but empty, return failure
                    return AuthenticateResult.Fail("Authorization header is empty.");
                }

                // Check if the Authorization header contains a Bearer token
                if (!authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    // Authorization header does not contain a Bearer token, return failure
                    return AuthenticateResult.Fail("Authorization header does not contain a Bearer token.");
                }

                // Extract the token from the Authorization header
                string token = authorizationHeader.Substring("Bearer ".Length).Trim();

                // Implement your custom token validation logic here
                // Example: Retrieve token from request, validate it, and create ClaimsPrincipal

                // If token is valid, create a ClaimsPrincipal representing the authenticated user
                // ClaimsPrincipal principal = YourCustomTokenValidationMethod(Request);

                // If authentication succeeds, return AuthenticateResult.Success
                // Otherwise, return AuthenticateResult.Fail with appropriate error message

                // For simplicity, assume authentication always succeeds in this example
                var principal = new System.Security.Claims.ClaimsPrincipal();
                //var principal = ValidateToken(token);

                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return AuthenticateResult.Success(ticket);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail($"Authentication failed: {ex.Message}");
            }
        }
    }
}