// using Microsoft.AspNetCore.Identity.Database;
using HotelBackendApi;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Identity.Extensions;

public static class MigrationExtensions
{
	public static void ApplyMigrations(this IApplicationBuilder app)
	{
		using IServiceScope scope = app.ApplicationServices.CreateScope();
		
		using MainContext context = scope.ServiceProvider.GetRequiredService<MainContext>();

		context.Database.Migrate();
	}
	
}
