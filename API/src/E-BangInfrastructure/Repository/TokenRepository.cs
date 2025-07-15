using E_BangDomain.Entities;
using E_BangDomain.Repository;
using E_BangDomain.Settings;
using E_BangDomain.StaticHelper;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.SymbolStore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_BangInfrastructure.Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly JWTTokenOptions _jwtToken;

        public TokenRepository(JWTTokenOptions jwtToken)
        {
            _jwtToken = jwtToken;
        }

        public string GenerateJWTTokenAsync(Account account)
        {
            List<Claim> claims =
            [
                new(type: ClaimTypes.NameIdentifier, value: account.Id),
                new(type: ClaimTypes.Email, value: account.Email!),
                new(type: ClaimTypes.Name, value: account.UserName!),
                new(type: ClaimTypes.MobilePhone, value: account.PhoneNumber is null?string.Empty:account.PhoneNumber)
            ];

            //TODO for rolse

            //
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtToken.Key));
            var credentails = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expireDays = DateTime.UtcNow.AddHours(_jwtToken.ExpireMinutes);

            var jwtToken = new JwtSecurityToken(
                issuer: _jwtToken.Issure,
                audience: _jwtToken.Audience,
                claims: claims,
                notBefore: null,
                expires: expireDays,
                credentails);

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        public string GenerateTwoWayFactoryToken()
            => CodeGenerator.GenerateRandomNumberCode();
    }
}
