using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CustomerSegmentator.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CustomerSegmentatorContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("CustomerSegmentatorContext") ?? throw new InvalidOperationException("Connection string 'CustomerSegmentatorContext' not found.")));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Director", policy => policy.RequireRole("Director"));
    options.AddPolicy("Administrator", policy => policy.RequireRole("Administrator"));
});
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<AppOptions>(builder.Configuration.GetSection("AppOptions"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=CustomerArrivedEvent}/{action=Create}/{id?}");


app.Run();

