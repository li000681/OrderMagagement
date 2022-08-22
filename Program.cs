using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FinalProject.Models.AdminAccess;
using FinalProject.Models.DataAccess;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews();
string dbConnStr = builder.Configuration.GetConnectionString("CustomerRecord");
builder.Services.AddDbContext<CustomerRecordContext>(options => options.UseSqlServer(dbConnStr));
builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(builder.Configuration["Connectionstrings:IdentityConnection"]));
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();



app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");
IdentitySeedData.EnsurePopulated(app);
app.Run();
