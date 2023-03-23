using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<User>().AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

// builder.Services.AddAuthentication().AddCookie(
//     options =>
//     {
//         options.LoginPath = "/Login";
//         options.LogoutPath = "/logout";
//     }
// );

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login/";
});

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = true; //! reqem true
    options.Password.RequireLowercase = true; //! herf true 
    options.Password.RequireNonAlphanumeric = false; //? simvol folse
    options.Password.RequireUppercase = false; //* boyuk herf   
    options.Password.RequiredLength = 6; //? length  sayi 
    // options.Password.RequiredUniqueChars = 1; //todo  bir dene uniq karakter olmalidir 
});

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

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
    );
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}/{seourl?}");

app.Run();