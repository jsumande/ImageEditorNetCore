using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ImageEditor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddSession(options =>
            {
                options.Cookie.Name = "Image.Session";
                options.IdleTimeout = TimeSpan.FromMinutes(60); // set duration
                options.Cookie.IsEssential = true;
            });

            services.AddControllersWithViews().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            services.AddMvc().AddViewOptions(options =>
            {
                options.HtmlHelperOptions.ClientValidationEnabled = false;
            });
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc(options =>
            {
                options.CacheProfiles.Add("DefaultNoCacheProfile", new CacheProfile
                {
                    NoStore = true,
                    Location = ResponseCacheLocation.None
                });
                options.Filters.Add(new ResponseCacheAttribute
                {
                    CacheProfileName = "DefaultNoCacheProfile"
                });
            });

            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
          
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/Exception/HandleError/{0}");




            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession(); // ADD to Enable Session - JPE 2020.03.04


            //app.Use(async (context, next) =>
            //{
            //    context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            //    await next();
            //});


            app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Account}/{action=Login}/{id?}");
            //});


            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Account}/{action=Login}/{id?}");
            //});


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            });




        }
    }
}
