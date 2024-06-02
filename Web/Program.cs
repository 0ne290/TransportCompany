using Application.Interactors;
using Dal;
using Dal.Daos;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using Serilog.Formatting.Json;
using Web.Middlewares;

namespace Web;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
            
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder()
                .WithDefaultDestructurers()
                .WithDestructurers(new[] { new DbUpdateExceptionDestructurer() }))
            
            .WriteTo.Async(a => a.File(new JsonFormatter(), @"E:\Logs\TransportCompany.log", retainedFileCountLimit: 4, rollOnFileSizeLimit: true, fileSizeLimitBytes: 5_368_709_120))// По умолчанию в файлы логируются обычные сообщения, но это можно исправить, передав в качестве первого параметра один из трех форматтеров - JsonFormatter, CompactJsonFormatter или RenderedCompactJsonFormatter - сравни результаты каждого из них в одном контексте
            
            .CreateLogger();
        
        try
        {
            Log.Information("Starting host build");
            
            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.
            builder.Services.AddSerilog();
            builder.Services.AddControllersWithViews();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, RedirectAfterFailedAuthentication>();
            builder.Services.AddScoped<UserInteractor>(serviceProvider =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<TransportCompanyContext>();
                var connectionString = serviceProvider.GetService<IConfiguration>()!.GetConnectionString("MySql")!;

                var options = optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                    .Options;

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
            
            app.UseSerilogRequestLogging();
            app.UseMiddleware<ExceptionLoggingMiddleware>();
            //app.UseMiddleware<RequestLoggingMiddleware>();

            // Configure the HTTP request pipeline.
            //if (!app.Environment.IsDevelopment())
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}

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

                if (!httpContext.User.IsInRole("Administrator"))
                    httpContext.Response.Redirect("/login/administrator");

                return await Task.FromResult(true);
            });
            app.UseCoreAdminCustomUrl("administrator");
            
            Log.Information("Success to build host. Starting web application");

            await app.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Failed to build host");
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }
}
