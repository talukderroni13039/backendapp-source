using Backend.Application.Validation;
using Backend.Domain.Entites;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Backend.Api.ServiceExtenstion
{
    public static class ExternalServiceExtention
    {
        // add all third party libries service here
        // 
        public static IServiceCollection AddFluentValidation(this IServiceCollection services, IConfiguration config)
        {
           services.AddControllers()
    .           AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ProductValidator>());

            return services;
        }
        public static IServiceCollection AddApiVersionServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddApiVersioning(options =>
            {
                // Specify URL-based versioning
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                    new HeaderApiVersionReader("x-api-version"),
                                                    new MediaTypeApiVersionReader("x-api-version"));
            });
            return services;
        }
        public static IServiceCollection AddIdentityAuthorizationServices(this IServiceCollection services, IConfiguration config)
        {
            var secret = config.GetValue<string>("Identity:Secret");
            var issuer = config.GetValue<string>("Identity:Issuer");
            var audience = config.GetValue<string>("Identity:Audience");

            var key = Encoding.ASCII.GetBytes(secret);

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            return services;
        }

    }
}
