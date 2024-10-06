using Microsoft.EntityFrameworkCore;
using HotelBackendApi;
using System.Text.Json.Serialization;
using HotelBackendApi.Domain.Services;
using Microsoft.AspNetCore.Identity;
using AspNetCore.Identity.Extensions;
using NuGet.Protocol;
using HotelBackendApi.Domain.Authorization;


var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Add services to the container.

builder.Services
    .AddControllers()
    .AddJsonOptions(options => {
        var enumConverter = new JsonStringEnumConverter();
        options.JsonSerializerOptions.Converters.Add(enumConverter);
    });

builder.Services.AddDbContext<MainContext>(opt => 
    opt.UseSqlite(connectionString));

builder.Services.AddAuthorization(options => {
    AuthorizationSetupService.SetupClaims(options);
});
builder.Services.AddAuthentication()
    .AddBearerToken(IdentityConstants.BearerScheme, options => {
        options.BearerTokenExpiration = TimeSpan.FromSeconds(600);
    });

builder.Services
    .AddIdentityCore<User>()
    .AddApiEndpoints()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MainContext>()
    .AddUserManager<HotelUserManager>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
    await AuthorizationSetupService.SetupTestUsers(app);
}

await AuthorizationSetupService.SetupRoles(app);

app.UseCors(builder => builder
        .AllowAnyHeader()
        .AllowAnyOrigin()
        .AllowAnyMethod()
);


app.UseHttpsRedirection();

app.MapControllers();

app.MapIdentityApi<User>();

await app.RunAsync();