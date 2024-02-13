
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using SmartScannerBackend.Models;

namespace SmartScannerBackend.Services.Authentication
{
    public static class PasswordUtilities
    {
        private static string? _tokenIssuer;

        public static string tokenIssuer
        {
            get
            {
                if (_tokenIssuer.IsNullOrEmpty())
                {
                    _tokenIssuer =
                        System.Configuration.ConfigurationManager.AppSettings["JwtIssuer"]!;
                };

                return _tokenIssuer!;
            }
        }

        public static string? _tokenAudience;

        public static string tokenAudience
        {
            get
            {
                if (_tokenAudience.IsNullOrEmpty())
                {
                    _tokenAudience =
                        System.Configuration.ConfigurationManager.AppSettings["JwtAudience"]!;
                };

                return _tokenAudience!;
            }
        }

        private static X509Certificate2? _x509Certificate2;

        public static X509Certificate2? x509Certificate2
        {
            get
            {
                if (_x509Certificate2 == null)
                {
                    string jwtPath = System.Configuration.ConfigurationManager.AppSettings["JwtPath"]!;
                    string jwtPassword = System.Configuration.ConfigurationManager.AppSettings["JwtPassword"]!;

                    _x509Certificate2 = new X509Certificate2(jwtPath, jwtPassword);
                };

                return _x509Certificate2;
            }
        }

        public static byte[] GetNewSalt()
        {
            return RandomNumberGenerator.GetBytes(128 / 8);
        }

        public static string GetHashedPassword(byte[] salt, string password)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
        }

        public static string IssueJwt(User user)
        {
            var token = new JwtSecurityToken(
                issuer: tokenIssuer,
                audience: tokenAudience,
                claims:
                [
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                ],
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: new X509SigningCredentials(x509Certificate2)
            );

            var handler = new JwtSecurityTokenHandler();
            string jwt = handler.WriteToken(token);

            return jwt;
        }
    }
}