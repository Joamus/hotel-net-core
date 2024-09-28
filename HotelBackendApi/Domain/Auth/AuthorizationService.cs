using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
public class AuthorizationService {

	public static void RegisterAuthRules(AuthorizationOptions options)
	{
		SetupClaims(options);
	}
	
	static void SetupClaims (AuthorizationOptions options)
	{
		options.AddPolicy("Employee", policy => policy.RequireClaim("EmployeeNumber"));
	}
	
	public static bool HasReadAccess(ClaimsPrincipal claims, long targetUserId)
	{
		// string requestingUserId = claims.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
		string requestingUserId = claims.GetUserId();
		
		if (requestingUserId == targetUserId.ToString()) {
			return true; 
		}

		if (claims.IsInRole("Manager")) {
			return true;
		}
		
		return false;
	}
	
}