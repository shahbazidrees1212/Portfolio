using Microsoft.EntityFrameworkCore;
using Portfolio.Models;

var builder = WebApplication.CreateBuilder(args);
// Railway/Docker port
builder.WebHost.UseUrls("http://0.0.0.0:8080");
// Add services
builder.Services.AddControllersWithViews();

// 🔹 Database Connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));


// 🔹 Email Configuration
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

var app = builder.Build();

// Error Handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // IMPORTANT

app.UseRouting();

app.UseAuthorization();

// Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
