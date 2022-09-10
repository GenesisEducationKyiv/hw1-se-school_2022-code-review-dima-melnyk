using Amazon.S3;
using GSES.BusinessLogic.Processors;
using GSES.BusinessLogic.Processors.Interfaces;
using GSES.BusinessLogic.Services;
using GSES.BusinessLogic.Services.Interfaces;
using GSES.DataAccess.Repositories;
using GSES.DataAccess.Repositories.Interfaces;
using GSES.DataAccess.Storages.Bases;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Net.Mail;
using FluentValidation;
using GSES.BusinessLogic.Validators;
using GSES.API.Consts;
using GSES.DataAccess.Storages.File;
using GSES.API.Middlewares;
using System.Net.Http;

namespace GSES.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GSES.API", Version = "v1" });
            });

            services.AddDefaultAWSOptions(this.Configuration.GetAWSOptions());

            services.AddHttpClient();
            _ = services.AddTransient<SmtpClient>((serviceProvider) =>
              {
                  return new SmtpClient()
                  {
                      Host = this.Configuration.GetValue<string>(ConfigConsts.SmtpHost),
                      Port = this.Configuration.GetValue<int>(ConfigConsts.SmtpPort),
                      Credentials = new NetworkCredential(
                              this.Configuration.GetValue<string>(ConfigConsts.SmtpEmail),
                              this.Configuration.GetValue<string>(ConfigConsts.SmtpPassword)),
                      EnableSsl = true
                  };
              });

            services.AddAWSService<IAmazonS3>();
            services.AddScoped<IStorage, FileStorage>();
            services.AddTransient<ISubscriberRepository, SubscriberRepository>();
            services.AddValidatorsFromAssemblyContaining<SubscriberValidator>();

            services.AddTransient<IRateProcessor>(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var httpClient = sp.GetRequiredService<HttpClient>();

                return config[ConfigConsts.CurrentRateApi] switch
                {
                    RateApisConsts.Coingecko => new CoingeckoRateProcessor(httpClient, config),
                    _ => new CoinApiRateProcessor(httpClient, config),
                };
            });
            services.AddTransient<IRateService, RateService>();
            services.AddTransient<ISubscriberService, SubscriberService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GSES.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
