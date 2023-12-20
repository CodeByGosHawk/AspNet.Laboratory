using Microsoft.EntityFrameworkCore;
using SinglePageArchitectureTraining.ApplicationService.Contracts;
using SinglePageArchitectureTraining.ApplicationService.Services;
using SinglePageArchitectureTraining.Models.Services;
using SinglePageArchitectureTraining.Models.Services.Contracts;
using SinglePageArchitectureTraining.Models.Services.Repositories;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("OnlineShop");
builder.Services.AddDbContext<OnlineShopDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Person}/{action=Person}/{id?}");

app.Run();
