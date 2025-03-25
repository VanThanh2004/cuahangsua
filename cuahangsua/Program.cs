using cuahangsua.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký ApplicationDbContext với SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký IHttpContextAccessor để dùng Session trong View
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Cấu hình Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Thêm dịch vụ SignalR
builder.Services.AddSignalR();

// Thêm dịch vụ MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Bảo vệ ứng dụng với HTTPS
app.UseHttpsRedirection();
app.UseStaticFiles();

// Kích hoạt Session Middleware
app.UseSession();

// Cấu hình định tuyến
app.UseRouting();
app.UseAuthorization();

// Thêm endpoint cho SignalR
app.MapHub<cuahangsua.Hubs.ChatHub>("/chatHub");

// Định nghĩa route mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();