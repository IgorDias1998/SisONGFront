using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("SisONGApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7090/api/");
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("SisONGApi"));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.AccessDeniedPath = "/Login/AcessoNegado";
        options.Cookie.Name = "SisONGAuthCookie";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;

        // Importante: amplia o tamanho do cookie
        options.Events.OnSigningIn = async context =>
        {
            var identity = (ClaimsIdentity)context.Principal.Identity!;
            // Garante que todos os claims sejam salvos
            var claims = identity.Claims.ToList();
        };
    });


builder.Services.AddAuthorization();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();