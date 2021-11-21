using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdServer.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Linq;
using System.Reflection;

namespace IdServer.Web
{
    public class Startup
    {
        private readonly AssemblyName _assemblyName;

        public Startup(IConfiguration configuration)
        {
            _assemblyName = GetType().Assembly.GetName();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme =
            //})
            var migrationAssembly = typeof(MigrationAssemblyTypeClass).Assembly.GetName().Name;

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddTestUsers(Config.GetTestUsers())
                //.AddInMemoryApiResources(Config.GetAllApiResources())
                //.AddInMemoryClients(Config.GetClients())
                //.AddInMemoryApiScopes(Config.GetScopes());
                // clients, resources
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = opt => opt.UseSqlite(Configuration.GetConnectionString("DefaultConnection"), sql => sql.MigrationsAssembly(migrationAssembly));
                })
                // token, consents, codes, etc
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = opt => opt.UseSqlite(Configuration.GetConnectionString("DefaultConnection"), sql => sql.MigrationsAssembly(migrationAssembly));
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            InitializeIdentityServerDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseIdentityServer()
                .UseAuthentication()
                .UseAuthorization();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        private void InitializeIdentityServerDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();

                if (!context.Clients.Any())
                {
                    context.Clients.AddRange(Config.GetClients().Select(c => c.ToEntity()));
                }

                if (!context.IdentityResources.Any())
                {
                    context.IdentityResources.AddRange(Config.GetIdentityResources().Select(ir => ir.ToEntity()));
                }

                if (!context.ApiResources.Any())
                {
                    context.ApiResources.AddRange(Config.GetAllApiResources().Select(ar => ar.ToEntity()));
                }

                if (!context.ApiScopes.Any())
                {
                    context.ApiScopes.AddRange(Config.GetScopes().Select(s => s.ToEntity()));
                }

                context.SaveChanges();
            }
        }
    }
}