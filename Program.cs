
using AbidiCompanySenario.Data.Context;
using AbidiCompanySenario.Data.Interfaces;
using AbidiCompanySenario.Data.Interfaces.Personnels;
using AbidiCompanySenario.Data.Repositories;
using AbidiCompanySenario.Data.Repositories.Personnels;
using AspNetCoreHero.ToastNotification;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);
ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // or LicenseContext.Commercial

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<ProjectDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);


builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 5;
    config.IsDismissable = true;
    config.Position = NotyfPosition.BottomCenter;
    config.HasRippleEffect = true;
});
builder.Services.AddScoped<IPersonnelRepository, PersonnelRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));



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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Personnel}/{action=Index}/{id?}");

app.Run();
