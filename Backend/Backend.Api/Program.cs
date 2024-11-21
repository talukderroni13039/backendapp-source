
using Backend.Api.ServiceExtenstion;
using Backend.Application.Middleware;
using Microsoft.EntityFrameworkCore;
using Backend.Application.Mapper;
using Backend.Infrastructure.Database;
using Serilog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Backend.Api.Filter;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            builder.Configuration
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("Config/appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"Config/appsettings.{env}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();


            builder.Services.AddDbContext<ApplicationDBContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            string[] origins = builder.Configuration["Origins"].Split(",");
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins(origins)
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
                //.WithHeaders(HeaderNames.ContentType));
            });

            // logger
            //Log.Logger = new LoggerConfiguration().ReadFrom   //Initialize Logger   
            //                          .Configuration(builder.Configuration)
            //                          .CreateLogger();


            // Add services to the container.
            builder.Services.AddDIServices(builder.Configuration);
            builder.Services.AddIdentityAuthorizationServices(builder.Configuration);
            builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            builder.Services.AddApiVersioning();



            //Global Authorization
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(new CustomAuthorizeAttribute());
            });

            //pass camel case rersult to the api.
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            //redis cacheConfiguration
            builder.Services.AddStackExchangeRedisCache((options) =>
            {
                options.Configuration = builder.Configuration.GetValue<string>("RedisConfig:RedisConn");
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            // Configure the HTTP request pipeline.

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseStaticFiles(); //for file upload
       
            app.UseCors();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            //custom middleware
            app.UseMiddleware<ExceptionMiddleware>();

            

            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"EmployeFiles")),
            //    RequestPath = new PathString("/EmployeFiles")
            //});

            app.MapControllers();
            app.Run();
        }
    }
}
