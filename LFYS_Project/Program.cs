using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LFYS_Project.Data;
using LFYS_Project.Models;
var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();


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

app.UseAuthorization();

#pragma warning disable ASP0014 // Suggest using top level route registrations
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllers(); // Đảm bảo rằng bạn đã thêm dòng này để cấu hình API routing
});
#pragma warning restore ASP0014 // Suggest using top level route registrations

// Seed roles and admin user
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    await SeedRolesAndAdminUserAsync(roleManager, userManager);
}
app.Run();
async Task SeedRolesAndAdminUserAsync(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
{
    string[] roleNames = { "Admin", "User", "Creater" };

    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // Create admin user if not exists
    var adminUser = await userManager.FindByEmailAsync("admin@example.com");
    if (adminUser == null)
    {
        var admin = new AppUser
        {
            UserName = "admin@example.com",
            Email = "admin@example.com",
            Name = "Admin User",
            Address = "Admin Address",
        };
        var createAdminResult = await userManager.CreateAsync(admin, "Admin@123");
        if (createAdminResult.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
    for(int i = 0; i < 4; i++)
    {
        var creater = await userManager.FindByEmailAsync("creater"+i+"@example.com");
        if (creater == null)
        {
            var admin = new AppUser
            {
                UserName = "creater" + i,
                Email = "creater" + i + "@example.com",
                Name = "creater" + i,
                Address = "Creater Address"
            };
            var createAdminResult = await userManager.CreateAsync(admin, "Creater@123");
            if (createAdminResult.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Creater");
            }
        }
    }
}
