using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using HotelBackendApi.Authorization;
using HotelBackendApi.Domain;

namespace HotelBackendApi.Controllers;
[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{

	private readonly ILogger<UserController> Logger;
	private readonly MainContext Context;

	private readonly UserManager<User> UserManager;

	public UserController(ILogger<UserController> logger, MainContext mainContext, UserManager<User> userManager)
	{
		Logger = logger;
		Context = mainContext;
		UserManager = userManager;
	}

	// [HttpGet("")]
	// [Authorize(Roles = "Manager")]
	// public async Task<ActionResult<IEnumerable<User>>> GetUsers()
	// {
	// 	Int32.TryParse(HttpContext.Request.Query["offset"], out int offset);
	// 	Int32.TryParse(HttpContext.Request.Query["offset"], out int limit);
	// }


	[Authorize(Policy = "MinManager")]
	[HttpPost("{id}/role")]
	public async Task<IActionResult> UpdateRole(string id, UpdateRoleBody body)
	{
		var user = await Context.Users.FindAsync(id);

		if (user == null)
		{
			return ApiError.NotFound().ToHttpError();
		}

		if (!Role.Roles.Any(role => role == body.role))
		{
			return ApiError.NotFound().ToHttpError();
		}

		if (await UserManager.IsInRoleAsync(user, body.role))
		{
			return NoContent();
		}

		await UserManager.AddToRoleAsync(user, body.role);

		return NoContent();
	}

	[Authorize(Policy = "MinManager")]
	[HttpDelete("{id}/role")]
	public async Task<IActionResult> RemoveRole(string id, UpdateRoleBody body)
	{
		var user = await Context.Users.FindAsync(id);

		if (user == null)
		{
			return ApiError.NotFound().ToHttpError();
		}

		if (!Role.Roles.Any(role => role == body.role))
		{
			return ApiError.NotFound().ToHttpError();
		}

		if (!await UserManager.IsInRoleAsync(user, body.role))
		{
			return NoContent();
		}

		await UserManager.RemoveFromRoleAsync(user, body.role);

		return NoContent();
	}
}


public class UpdateRoleBody {
	public required string role { get; set; }
}