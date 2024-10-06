using System.Security.Claims;
using HotelBackendApi.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace HotelBackendApi.Domain.Authorization;
public static class AuthorizationSetupService {
	public static void SetupClaims(AuthorizationOptions options)
	{
		options.AddPolicy("Employee", policy => policy.RequireClaim("EmployeeNumber"));
		options.AddPolicy("MinGuest", policy => policy.RequireRole("Guest", "Manager"));
		options.AddPolicy("MinManager", policy => policy.RequireRole("Manager"));
	}
	
	public async static Task SetupRoles(WebApplication app)
	{
		
		using (var scope = app.Services.CreateScope()) {
		   var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
		   
		   var roles = Role.Roles;
		   
		   foreach (var role in roles) {
				if (!await roleManager.RoleExistsAsync(role))
				{
					await roleManager.CreateAsync(new IdentityRole(role));
				}
			}
		}
	}
	
	public async static Task SetupTestUsers(WebApplication app)
	{
		using (var scope = app.Services.CreateScope()) {
			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
			// Add a default manager

			string managerEmail = "manager@nethotel.com";
			string managerPassword = "ILoveGuests1234#@";
			
			if (await userManager.FindByEmailAsync(managerEmail) == null) {
				var user = new User();
				user.Email = managerEmail;
				user.UserName = managerEmail;
				user.PasswordHash = managerPassword;
				
				await userManager.CreateAsync(user, managerPassword);
				await userManager.AddToRoleAsync(user, "Manager");
			}
		}
	}
	
	

	// public async static ValueTask<bool> HasReadAccess(ClaimsPrincipal claims, IAuthorizationService authorizationService, long targetUserId)
	// {
	// 	// string requestingUserId = claims.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
	// 	string requestingUserId = claims.GetUserId();
		
	// 	if (requestingUserId == targetUserId.ToString()) {
	// 		return true; 
	// 	}
		
	// 	var isAuthorized = await authorizationService.AuthorizeAsync(claims, "IsManager");
	// 	return isAuthorized.Succeeded;
	// }
}