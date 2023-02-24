using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VendingMachine.API.Infrastructure;
using VendingMachine.DataAccess;
using VendingMachine.Domain.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddFastEndpoints();
builder.Services.AddSwaggerDoc(config =>
{
    config.Title = "Vending machine API";
    config.Version = "v1";
});

builder.Services.AddCors(config =>
{
#if DEBUG
    config.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
#endif
});

builder.Services
    .AddDbContext<VendingMachineDbContext>(config =>
    {
        config.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    })
    .AddIdentity<VendingMachine.Domain.Identity.User, Role>()
    .AddEntityFrameworkStores<VendingMachineDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
});

builder.Services.AddJWTBearerAuth(builder.Configuration.GetValue<string>("Auth:Secret"));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseDefaultExceptionHandler();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints(config =>
{
    config.Endpoints.RoutePrefix = "api";

    //config.Endpoints.Configurator = ep =>
    //{
    //    ep.PreProcessors(Order.Before, new RequestLogger());
    //    ep.PostProcessors(Order.After, new ResponseLogger());
    //};
});

app.UseOpenApi();
app.UseSwaggerUi3(s => s.ConfigureDefaults());

app.Run();