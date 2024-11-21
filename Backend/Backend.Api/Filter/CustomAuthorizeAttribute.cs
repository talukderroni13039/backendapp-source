using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Backend.Application.Interface.DomainRepository;

namespace Backend.Api.Filter
{
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    { 
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // validate token with Issuer Audience
            // Validate User With UserId
            var allowsAnonymous = context.ActionDescriptor.EndpointMetadata.Any(em => em.GetType() == typeof(AllowAnonymousAttribute));
            if (allowsAnonymous) // Skip authorization if anonymous access is allowed
            {
                return;
            }

            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            //Access Database for
            
          
            var claims = context.HttpContext.User.Claims;
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
          
            if (token == null )
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }

            var userId = ValidateJwtToken(token, configuration);

            if (userId!= null)
            {
                // check The endpoint is validate to this user or Not

              var IProductRepository = context.HttpContext.RequestServices.GetRequiredService<IProductRepository>();
            }
            else
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }

       
        }
        private string ValidateJwtToken(string token, IConfiguration _configuration)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Identity:Secret"]);

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


                var UserId = jwtToken.Claims.First(x => x.Type == "UserId").Value;
                // if validation is successful then return UserId from JWT token 
                return UserId;
            }
            catch (Exception ex)
            {
                // if validation fails then return null
                return null;
            }
        }
    }
}
