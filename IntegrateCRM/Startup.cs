using IntegrateCRM.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using IntegrateCRM.Abstractions.DB;
using IntegrateCRM.Database;
using IntegrateCRM.Services;
using IntegrateCRM.Abstractions.Services.CRMService;
using IntegrateCRM.Abstractions.Services.CountryByIp;
using IntegrateCRM.Abstractions.Services.SmtpClientService;
using FluentValidation.AspNetCore;
using IntegrateCRM.Abstractions.Validators;
using System.IO;
using System;
using System.Linq;
using System.Reflection;

namespace IntegrateCRM
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            IdentityEnvironment(services);

            services.AddScoped<ICRMService, CRMService>();
            services.AddScoped<IMailChimpService, MailChimpService>();
            services.AddScoped<ICountryByIpService, CountryByIpService>();
            services.AddScoped<ISmtpClientService, SmtpClientService>();
            services.AddScoped<IGoogleReCaptchaService, GoogleReCaptchaService>();
            services.AddSingleton<ICRMDBContext, CRMDbContext>(
                sp => new CRMDbContext(@$"data source=CRM.sqlite; foreign keys=true"));

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
            services.AddCors();

            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<IValidatorModel>());
            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RegisterCRMModelValidator>());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(opts =>
                {
                    opts.AllowAnyOrigin();
                    opts.AllowAnyHeader();
                    opts.AllowAnyMethod() ;
                   // opts.AllowCredentials();
                });

            app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }

        private void IdentityEnvironment(IServiceCollection services)
        {
            var CRMConfigurationProviderSection = Configuration.GetSection(nameof(CRMConfiguration));
            services.Configure<CRMConfiguration>(CRMConfigurationProviderSection);

            var CountryByIpConfigurationSection = Configuration.GetSection(nameof(CountryByIpConfiguration));
            services.Configure<CountryByIpConfiguration>(CountryByIpConfigurationSection);

            var identityEmailProviderSection = Configuration.GetSection(nameof(EmailProvider));
            services.Configure<EmailProvider>(identityEmailProviderSection);

            var identityEmailTemplateSection = Configuration.GetSection(nameof(EmailTemplate));
            services.Configure<EmailTemplate>(identityEmailTemplateSection);

            var identityGoogleCaptchaSection = Configuration.GetSection(nameof(GoogleCaptcha));
            services.Configure<GoogleCaptcha>(identityGoogleCaptchaSection);

            var identityMailChimpConfiguration = Configuration.GetSection(nameof(MailChimpConfiguration));
            services.Configure<MailChimpConfiguration>(identityMailChimpConfiguration);
        }
    }
}
