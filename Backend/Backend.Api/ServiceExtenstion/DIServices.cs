using Backend.Application.Interface.Caching;
using Backend.Application.Interface.DomainRepository;
using Backend.Application.Interface.ReportRepository;
using Backend.Application.Services;
using Backend.Application.Validation;
using Backend.Infrastructure.Cacheing.InMemory.Backend.Infrastructure.Cacheing.InMemory;
using Backend.Infrastructure.Cacheing.Redis;
using Backend.Infrastructure.Interface.Email;
using Backend.Infrastructure.Interface.OTP;
using Backend.Infrastructure.Services.DomainRepository;
using Backend.Infrastructure.Services.External;
using Backend.Infrastructure.Services.ReportRepository;
using Backend.Reporting.Service;

namespace Backend.Api.ServiceExtenstion
{
    public static class DIServices
    {
        public static IServiceCollection AddDIServices(this IServiceCollection services, IConfiguration config)
        {
            // add all kinds of DI that  has been used in this projets
            
            //Service DI
            services.AddTransient<IDatabaseHelper, DatabaseHelper>();
            services.AddTransient<ProductsPayloadHandlerService>();

            //Domainvalidator
            services.AddScoped<ProductValidator>();

            //domainRepository DI
            services.AddTransient<IProductRepository, ProductRepositoryService>();

            //ReportService
            services.AddTransient<IProductReportService, ProductReportService>();
            services.AddTransient<IReportService, ReportService>();

            //External Services
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IOTPService, OTPService>();
            services.AddTransient<ISmsService, SmsService>();
            //Cacheing 
            //  services.AddScoped<IInMemoryCache, InMemoryCache>();
            services.AddScoped<IRedisCache, RedisCache>();

            return services;
        }

    }
}
