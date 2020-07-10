
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RacLib;

namespace LibVisWeb
{
    public class Startup
    {

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {

            Configuration = configuration;
            Base.conf = new BaseJson(configuration);
            Base.conf.SetPaths(env.ContentRootPath, env.WebRootPath);
            BaseUserSource.source = new RacLib.RacWebUserSource();
            BaseLog.log = new RacWebLog();

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllersWithViews().AddJsonOptions(options =>
            {
                // Use the default property (Pascal) casing.
                options.JsonSerializerOptions.PropertyNamingPolicy = null;

            });
#pragma warning disable CS0618 // Type or member is obsolete
            services.AddSpaPrerenderer();
#pragma warning restore CS0618 // Type or member is obsolete
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
#pragma warning disable CS0618 // Type or member is obsolete
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                    ReactHotModuleReplacement = true
                });
#pragma warning restore CS0618 // Type or member is obsolete
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();
       
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapFallbackToController("Index","Home");
            });

            app.UseHttpsRedirection();
        }
    }
}
