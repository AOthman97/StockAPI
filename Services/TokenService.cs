using api.Interfaces;
using api.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api.Services
{
    // This service is used to create the JWT token
    public class TokenService : ITokenService
    {
        // The IConfiguration will allow us access to the appsettings.json properties
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]));
        }

        public string CreateToken(AppUser user)
        {
            // Add required claims like this and others specific to the platform like isPaying
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName)
            };

            // Those are used to secure the token with the specified key and algo
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            // This will wrap all of the data needed to create the token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(5),
                SigningCredentials = creds,
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"]
            };

            // This will create the actual token using it's "description"
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Return the token as string and not and object
            return tokenHandler.WriteToken(token);
        }
    }
}