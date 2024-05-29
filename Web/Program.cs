using Application.Interactors;
using Dal;
using Dal.Daos;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Web.Middlewares;

namespace Web;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
        builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, CustomAuthorizationMiddlewareResultHandler>();
        builder.Services.AddScoped<UserInteractor>(serviceProvider =>
        {
            var optionsBuilder = new DbContextOptionsBuilder<TransportCompanyContext>();
            var connectionString = serviceProvider.GetService<IConfiguration>()!.GetConnectionString("MySql")!;

            var options = optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)).Options;

            return new UserInteractor(new UserDao(new TransportCompanyContext(options)));
        });
        builder.Services.AddDbContext<TransportCompanyContext>((serviceProvider, options) =>
        {
            var connectionString = serviceProvider.GetService<IConfiguration>()!.GetConnectionString("MySql")!;

            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddCoreAdmin();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Login}/{action=Index}/{id?}");
        
        app.UseCoreAdminCustomAuth(async serviceProvider =>
        {
            var httpContext = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext!;
            
            if (httpContext.User.IsInRole("Administrator"))
                return await Task.FromResult(true);
            
            httpContext.Response.Redirect("/login/administrator");
            return await Task.FromResult(false);
        });
        app.UseCoreAdminCustomUrl("administrator");

        await app.RunAsync();
    }
}