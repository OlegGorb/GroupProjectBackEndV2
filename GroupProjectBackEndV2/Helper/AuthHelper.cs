using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GroupProjectBackEndV2.Helper
{
    public class AuthHelper
    {
        public IConfiguration Configuration { get; }
        public AuthHelper(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string CreateToken(IEnumerable<Claim> claims, DateTime expiresAt)
        {
            byte[] secretKey = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("SecretKey"));

            JwtSecurityToken jwt = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiresAt,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(secretKey),
                    SecurityAlgorithms.HmacSha256Signature
                )
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
