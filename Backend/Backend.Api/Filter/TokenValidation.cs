
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Backend.Api.Filter
{
    public class TokenValidation:IDisposable
    {
       private  IConfiguration _configuration;
        public TokenValidation( IConfiguration configuration) 
        {
            _configuration = configuration;
        }
        public  int? ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Identity:Key"]);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = _configuration["Identity:Audience"],
                    ValidIssuer = _configuration["Identity:Issuer"],
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;


                var UserId = int.Parse(jwtToken.Claims.First(x => x.Type == "UserId").Value);
                // if validation is successful then return UserId from JWT token 
                return UserId;
            }
            catch (Exception ex)
            {
                // if validation fails then return null
                return null;
            }
        }
        public void Dispose()
        {
           // throw new NotImplementedException();
        }


    }
}
