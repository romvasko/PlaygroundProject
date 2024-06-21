using IdentityServer4.Models;
using IdentityServer;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer.Data.DbSeed;
using IdentityServer.Data;
using Microsoft.EntityFrameworkCore;
using IdentityServer.Data.Models;
using Microsoft.AspNetCore.Identity;
using IdentityServer4.Validation;
using IdentityServer4.AspNetIdentity;
using IdentityServer.Services;
using IdentityServer4.Services;
using PlaygroundProject.ViewModels;
using System.Web.Mvc;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddScoped<IDbSeed, DbSeed>();


        builder.Services.AddControllersWithViews();

        string dbConnectionString = builder.Configuration.GetConnectionString("DockerLocal");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(dbConnectionString));

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(config =>
        {
            config.Password.RequiredLength = 8;
            config.Password.RequireDigit = true;
            config.Password.RequireNonAlphanumeric = true;
            config.Password.RequireUppercase = true;
            config.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/ àáâãäå¸æçèéêëìíîïğñòóôõö÷øùúûüışÿÀÁÂÃÄÅ¨ÆÇÈÉÊËÌÍÎÏĞÑÒÓÔÕÖ×ØÙÚÛÜİŞß";
            config.Password.RequireLowercase = false;
            config.User.RequireUniqueEmail = true;
            config.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/ àáâãäå¸æçèéêëìíîïğñòóôõö÷øùúûüışÿÀÁÂÃÄÅ¨ÆÇÈÉÊËÌÍÎÏĞÑÒÓÔÕÖ×ØÙÚÛÜİŞß";
        })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddIdentityServer(options =>
        {
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;

            options.IssuerUri = "https://localhost:7094";
        })
            .AddInMemoryApiResources(Config.ApiResources)
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients)
            .AddAspNetIdentity<ApplicationUser>()
            .AddDeveloperSigningCredential()
            .AddProfileService<ProfileService>()
            .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
            .AddJwtBearerClientAuthentication();

        builder.Services.AddLocalApiAuthentication();
        builder.Services.AddAuthentication();

        var app = builder.Build();

        app.UseStaticFiles();

        app.UseRouting();
        app.UseCors(option =>
        {
            option.AllowAnyOrigin();
            option.AllowAnyHeader();
            option.AllowAnyMethod();
        });
        app.UseIdentityServer();

        app.UseAuthorization();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
        }
        var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

        using (var scope = scopeFactory.CreateScope())
        {
            var dbSeed = scope.ServiceProvider.GetService<IDbSeed>();
            dbSeed?.Seed();
        }

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.UseDeveloperExceptionPage();

        app.Run();
    }
}