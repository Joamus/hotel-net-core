using HotelBackendApi.Domain.Services;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ConfigServiceCollectionExtensions
	{
		public static IServiceCollection AddServices(
			this IServiceCollection services
		) {

			services.AddScoped<RoomReservationService>();
			
			return services;
		}
		
	}

}