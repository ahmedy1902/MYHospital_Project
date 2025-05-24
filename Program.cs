using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CareNet_System.Models;
using CareNet_System.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<HosPitalContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("cs")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<HosPitalContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
});

builder.Services.AddScoped<IRepository<Staff>, StaffRepository>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>(); 
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IBillsRepository, BillsRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();

var app = builder.Build();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();