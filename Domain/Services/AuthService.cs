using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Domain.Services
{
    public class AuthService
    {
        //private readonly string secureKey = "This is my secure key. It is a very secure key.";    //key used to encode the token
        private readonly string _secureKey;

        //Inject IConfiguration to fetch the secure key from the configuration.
        public AuthService(IConfiguration configuration)
        {
            _secureKey = configuration["Jwt:SecureKey"];
        }

        #region Password encryption functions
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
        #endregion

        #region JWT(JSON Web Token) functions
        public string GenerateToken(Guid userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secureKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var header = new JwtHeader(credentials);

            //payload contains encoded data
            //var payload = new JwtPayload(userId.ToString(), null, null, null, DateTime.Today.AddDays(1));   //last 24 hours
            var payload = new JwtPayload(issuer: userId.ToString(), audience: null, claims: null, notBefore: null, expires: DateTime.UtcNow.AddDays(1), issuedAt: DateTime.UtcNow);

            var token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public JwtSecurityToken VerifyToken(string jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            byte[] key = Encoding.ASCII.GetBytes(_secureKey);

            try
            {
                tokenHandler.ValidateToken(jwt, new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true // Ensures token hasn't expired
                }, out SecurityToken validatedToken);

                return (JwtSecurityToken)validatedToken;
            }
            catch (SecurityTokenException ex)
            {
                // Handle specific token validation exceptions if needed
                Console.WriteLine($"Token validation failed: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                Console.WriteLine($"An error occurred during token validation: {ex.Message}");
                return null;
            }
        }
        #endregion
    }
}