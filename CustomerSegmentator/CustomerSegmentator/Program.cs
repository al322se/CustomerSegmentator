using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CustomerSegmentator.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CustomerSegmentatorContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("CustomerSegmentatorContext") ?? throw new InvalidOperationException("Connection string 'CustomerSegmentatorContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=CustomerArrivedEvent}/{action=Create}/{id?}");


app.Run();

