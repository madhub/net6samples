using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace AuthWithJwt
{
    public static class Helper
    {
        public static SecurityToken JwtSignatureVaidator(string token, TokenValidationParameters validationParameters)
        {
            var jwt = new JwtSecurityToken(token);
            return jwt;
        }

    }
}
