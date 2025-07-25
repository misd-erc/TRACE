using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.EntityFrameworkCore;
using TRACE.Helpers;
using TRACE.Context;
using System.Security.Claims;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

builder.Configuration["AzureAd:Instance"] = Environment.GetEnvironmentVariable("AZURE_AD_INSTANCE");
builder.Configuration["AzureAd:TenantId"] = Environment.GetEnvironmentVariable("AZURE_AD_TENANT_ID");
builder.Configuration["AzureAd:ClientId"] = Environment.GetEnvironmentVariable("AZURE_AD_CLIENT_ID");
builder.Configuration["AzureAd:ClientSecret"] = Environment.GetEnvironmentVariable("AZURE_AD_CLIENT_SECRET");
builder.Configuration["AzureAd:GroupId"] = Environment.GetEnvironmentVariable("AZURE_AD_GROUP_ID");
builder.Configuration["AzureAd:CallbackPath"] = Environment.GetEnvironmentVariable("AZURE_AD_CALLBACK_PATH");
builder.Configuration["AzureAd:Scopes"] = Environment.GetEnvironmentVariable("AZURE_AD_SCOPES");


builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ErcdbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ErcDatabase")));

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddInMemoryTokenCaches();

builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://localhost:44333") // <- your frontend URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(120);
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

builder.Services.AddScoped<EventLogger>();

builder.Services.AddHttpClient<GetGroupMemberHelper>();

var app = builder.Build();
app.UseCors("AllowFrontend");

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
