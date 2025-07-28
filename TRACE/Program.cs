using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.EntityFrameworkCore;
using TRACE.Helpers;
using TRACE.Context;
using System.Security.Claims;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables
DotNetEnv.Env.Load();

// Azure AD config
builder.Configuration["AzureAd:Instance"] = Environment.GetEnvironmentVariable("AZURE_AD_INSTANCE");
builder.Configuration["AzureAd:TenantId"] = Environment.GetEnvironmentVariable("AZURE_AD_TENANT_ID");
builder.Configuration["AzureAd:ClientId"] = Environment.GetEnvironmentVariable("AZURE_AD_CLIENT_ID");
builder.Configuration["AzureAd:ClientSecret"] = Environment.GetEnvironmentVariable("AZURE_AD_CLIENT_SECRET");
builder.Configuration["AzureAd:GroupId"] = Environment.GetEnvironmentVariable("AZURE_AD_GROUP_ID");
builder.Configuration["AzureAd:CallbackPath"] = Environment.GetEnvironmentVariable("AZURE_AD_CALLBACK_PATH");
builder.Configuration["AzureAd:Scopes"] = Environment.GetEnvironmentVariable("AZURE_AD_SCOPES");

// Database
builder.Services.AddDbContext<ErcdbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ErcDatabase")));

// Auth
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddInMemoryTokenCaches();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedOrigins", policy =>
    {
        policy.WithOrigins("https://localhost:44333", "https://localhost:44324", "https://icdms-uat.erc.ph", "https://localhost:44339")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// MVC + Session + Cache
builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(120);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Dependency injection
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

// ✅ Make sure this is LAST before app = builder.Build()
var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("AllowedOrigins");

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=External}/{action=Login}/{id?}");

app.Run();
