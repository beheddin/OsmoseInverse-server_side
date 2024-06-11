using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Domain.Models;
using System.Collections.Generic;
using System.Net;

namespace Domain.Services
{
    public class AuthService
    {
        //private readonly string secureKey = "This is my secure key. It is a very secure key.";    //key used to encode the token
        private readonly string _secureKey;
        private readonly string _issuer;
        private readonly string _audience;

        //Inject IConfiguration to fetch the secure key from the configuration.
        public AuthService(IConfiguration configuration)
        {
            _secureKey = configuration["Jwt:SecureKey"];
            _issuer = configuration["Jwt:Issuer"];
            _audience = configuration["Jwt:Audience"];
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
        //public string GenerateToken( Guid? compteId)
        public string GenerateToken(Compte compte)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secureKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            ////method 1

            //var header = new JwtHeader(credentials);

            ////payload contains encoded data
            ////var payload = new JwtPayload(compteId.ToString(), null, null, null, DateTime.Today.AddDays(1));   //last 24 hours
            //var payload = new JwtPayload(issuer: compteId.ToString(), audience: null, claims: null, notBefore: null, expires: DateTime.UtcNow.AddDays(1), issuedAt: DateTime.UtcNow);

            //var token = new JwtSecurityToken(header, payload);

            //method 2
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, compte.IdCompte.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                //new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.NameIdentifier, compte.CIN),
                new Claim("nom", compte.Nom),
            };

            var token = new JwtSecurityToken(
                //issuer: null,
                issuer: _issuer,
                //audience: null,
                audience: _audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
            );

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
                    //ValidateIssuer = true,
                    //ValidateAudience = true,
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,

                    ValidateLifetime = true, // Ensures token hasn't expired
                    ClockSkew = TimeSpan.Zero // Optional: eliminate clock skew
                }, out SecurityToken validatedToken);

                return (JwtSecurityToken)validatedToken;
            }
            catch (SecurityTokenExpiredException ex)
            {
                Console.WriteLine($"Token expired: {ex.Message}");
                return null;
            }
            catch (SecurityTokenValidationException ex)
            {
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