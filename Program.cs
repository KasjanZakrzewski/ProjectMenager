using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectMenager.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//builder.Services.AddAutoMapper(typeof(OutOfOfficeProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Administrator", "ProjectManager", "Guest", "Employee" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider
        .GetRequiredService<UserManager<IdentityUser>>();

    string email = "Tomasz@Test.com";
    string passwd = "zaq1@WSX";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new IdentityUser();
        user.UserName = "Tomasz Test";
        user.Email = email;
        user.EmailConfirmed = true;

        await userManager.CreateAsync(user, passwd);
        await userManager.AddToRoleAsync(user, "Employee");
    }

    email = "Gonzo@Guest.com";
    passwd = "zaq1@WSX";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new IdentityUser();
        user.UserName = "Gonzo Guest";
        user.Email = email;
        user.EmailConfirmed = true;

        await userManager.CreateAsync(user, passwd);
        await userManager.AddToRoleAsync(user, "Guest");
    }

    email = "Peter@Project.com";
    passwd = "zaq1@WSX";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new IdentityUser();
        user.UserName = "Peter Project";
        user.Email = email;
        user.EmailConfirmed = true;

        await userManager.CreateAsync(user, passwd);
        await userManager.AddToRoleAsync(user, "ProjectManager");
    }

    email = "Admin@Admin.com";
    passwd = "zaq1@WSX";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new IdentityUser();
        user.UserName = "Admin Admin";
        user.Email = email;
        user.EmailConfirmed = true;

        await userManager.CreateAsync(user, passwd);
        await userManager.AddToRoleAsync(user, "Administrator");
    }
}

app.Run();
