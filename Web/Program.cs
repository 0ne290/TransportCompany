using Dal;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace Web;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options => options.LoginPath = new PathString("/login"));
        /*builder.Services.AddSingleton<DrinksVendingMachine>(serviceProvider =>
        {
            var optionsBuilder = new DbContextOptionsBuilder<TransportCompanyContext>();
            var connectionString = serviceProvider.GetService<IConfiguration>()!.GetConnectionString("MySql")!;

            var options = optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .Options;

            return new DrinksVendingMachine(new Dao<Drink>(new VendingContext(options)),
                new Dao<Coin>(new VendingContext(options)));
        });*/
        builder.Services.AddDbContext<TransportCompanyContext>((serviceProvider, options) =>
        {
            var connectionString = serviceProvider.GetService<IConfiguration>()!.GetConnectionString("MySql")!;

            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddCoreAdmin("Administrator");

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
            pattern: "{controller=Home}/{action=Index}/{id?}");

        await app.RunAsync();
    }
}