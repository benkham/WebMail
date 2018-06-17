using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using WebMail.Core;
using WebMail.Core.Abstractions;
using WebMail.MailGun;
using WebMail.SendGrid;

namespace WebMail
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
            services.AddMvc();
            services.AddCors();

            services.AddSingleton<IConfiguration>(Configuration);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "WebMail HTTP API",
                    Version = "v1"
                });
            });

            // Inject webmail dependencies
            services.AddTransient<IEmail, Email>(sp =>
                new Email(Configuration));

            services.AddTransient<ISender, FailoverSenderProxy>(sp =>
            {
                ISender sendGridSender = new SendGridSender(Configuration);
                ISender mailGunSender = new MailGunSender(Configuration);

                // Register email senders for failover. Failover follows registration order.
                return new FailoverSenderProxy()
                    .RegisterSender(sendGridSender)
                    .RegisterSender(mailGunSender);
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", "WebMail V1");
            });

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseMvc();

            app.UseSpa(spa =>
            {
                if (env.IsDevelopment())
                {
                    var spaDevServer = Configuration.GetSection("SPA_DEV_SERVER").Value;
                    spa.UseProxyToSpaDevelopmentServer(spaDevServer);
                }
            });
        }
    }
}
