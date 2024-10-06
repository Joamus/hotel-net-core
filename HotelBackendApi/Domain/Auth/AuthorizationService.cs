using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using HotelBackendApi;
public class HotelAuthorizationService
{
	readonly ILogger<HotelAuthorizationService> LoggerContext;
	readonly MainContext Context;

	public HotelAuthorizationService(ILogger<HotelAuthorizationService> loggerContext, MainContext context)
	{
		LoggerContext = loggerContext;
		Context = context;
	}


	public async static ValueTask<bool> HasReadAccess(ClaimsPrincipal claims, IAuthorizationService authorizationService, long targetUserId)
	{
		// string requestingUserId = claims.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
		string requestingUserId = claims.GetUserId();
		
		if (requestingUserId == targetUserId.ToString()) {
			return true; 
		}
		
		var isAuthorized = await authorizationService.AuthorizeAsync(claims, "IsManager");
		return isAuthorized.Succeeded;
	}
}