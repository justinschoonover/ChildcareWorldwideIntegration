using System.Diagnostics.CodeAnalysis;
using ChildcareWorldwide.Denari.Api;
using ChildcareWorldwide.Google.Api.Helpers;
using ChildcareWorldwide.Hubspot.Api;
using Google.Cloud.Diagnostics.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ChildcareWorldwide.Integration.Manager
{
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public sealed class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
                })
                .AddCookie()
                .AddGoogle(options =>
                {
                    options.ClientId = Configuration["Authentication_Google_ClientId"];
                    options.ClientSecret = Configuration["Authentication_Google_ClientSecret"];

                    options.Scope.Add("https://www.googleapis.com/auth/userinfo.email");
                    options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
                    options.ClaimActions.MapJsonKey("urn:google:name", "name", "string");
                    options.ClaimActions.MapJsonKey("urn:google:email", "email", "string");
                });

            services.AddSingleton<IDrapiService>(sp => new DrapiService(Configuration["DenariApiKey"]));
            services.AddSingleton<IHubspotService>(sp => new HubspotService(Configuration["HubspotApiKey"]));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Log to Google
                loggerFactory.AddGoogle(app.ApplicationServices, GoogleComputeEngineHelper.GetCurrentProjectIdAsync().GetAwaiter().GetResult());

                // force HTTPS in the cloud run environment
                app.Use(async (context, next) =>
                {
                    context.Request.Scheme = "https";
                    await next();
                });

                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseForwardedHeaders();

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
