using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.EntityFrameworkCore;
using TRACE.Helpers;
using TRACE.Context;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ErcdbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ErcDatabase")));

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddInMemoryTokenCaches();

builder.Services.AddAuthorization();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<GenerateOTPHelper>();

builder.Services.AddScoped<CurrentUserHelper>(serviceProvider =>
{
    var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
    var user = httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal();
    var tokenAcquisition = serviceProvider.GetRequiredService<ITokenAcquisition>();
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var dbContext = serviceProvider.GetRequiredService<ErcdbContext>();

    return new CurrentUserHelper(user, tokenAcquisition, configuration, dbContext, httpContextAccessor);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=External}/{action=Login}/{id?}");

app.Run();
