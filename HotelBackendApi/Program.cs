using Microsoft.EntityFrameworkCore;
using HotelBackendApi;
using System.Text.Json.Serialization;
using HotelBackendApi.Domain.Services;
using Microsoft.AspNetCore.Identity;
using AspNetCore.Identity.Extensions;
using NuGet.Protocol;


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
    AuthorizationSetupService.RegisterAuthRules(options);
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
}

app.UseCors(builder => builder
        .AllowAnyHeader()
        .AllowAnyOrigin()
        .AllowAnyMethod()
);

using (var scope = app.Services.CreateScope()) {
   var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
   
   var roles = new [] { "Manager", "Guest" };
   
   foreach (var role in roles) {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
   
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    // Add a default manager

    string managerEmail = "admin@nethotel.com";
    string managerPassword = "ILoveGuests1234#@";
    
    if (await userManager.FindByEmailAsync(managerEmail) == null) {
        var user = new User();
        user.Email = managerEmail;
        user.PasswordHash = managerPassword;
        
        await userManager.CreateAsync(user, managerPassword);
        await userManager.AddToRoleAsync(user, "Manager");
    }

    // Add a default guest
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapIdentityApi<User>();

await app.RunAsync();