using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Separate.Data;
using Separate.Data.Entities;
using Separate.Services;

namespace Separate
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

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("SqliteConnection"),
                    assembly => assembly.MigrationsAssembly("Separate.Api"));
            });

            //services.AddDbContext<ApplicationDbContext>(options =>
            //{
            //    options.UseMySql(Configuration.GetConnectionString("MySqlConnection"),
            //        assembly => assembly.MigrationsAssembly("Separate.Api"));
            //});

            //services.AddDbContext<ApplicationDbContext>(options =>
            //{

            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
            //        assembly => assembly.MigrationsAssembly("Separate.Api"));

            //    options.UseOpenIddict();
            //});

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            });

            services.AddOpenIddict()

            .AddCore(options =>
            {
                options.UseEntityFrameworkCore().UseDbContext<ApplicationDbContext>();
            }).AddServer(options =>
            {
                options.UseMvc();
                options.EnableTokenEndpoint("/connect/token");
                options.AllowPasswordFlow();
                options.DisableHttpsRequirement();
                options.AcceptAnonymousClients();
            }).AddValidation();

            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder.WithOrigins("https://localhost:44329", "https://localhost:5001",
                    "https://localhost:5000").AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
            }));

            services.AddSignalR();

            services.AddControllers();

            services.AddTransient<IEmailServices, EmailServices>();
            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var serviceProvider = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider;
            SeedDatabase.Initialize(serviceProvider);

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors("CorsPolicy");
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chatHub");
            });
            
        }
    }
}
