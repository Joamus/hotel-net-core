using Xunit;
using Microsoft.IdentityModel.Logging;
using HotelBackendApi;
using HotelBackendApi.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace HotelBackendApi.Tests.Services;
public class RoomReservationServiceTests
{ 
	readonly LoggerContext _loggerContext;
	readonly MainContext _context;
	
	readonly RoomReservationService _roomReservationService;
	
	public RoomReservationServiceTests() {
		_context = new MainContext(new DbContextOptions<MainContext>());
		_loggerContext = new LoggerContext();
		_roomReservationService = new RoomReservationService(_loggerContext, _context);
	}
	[Fact]
	public void DoesReservationOverlapWithExistingReservation_SameRoomNoOverlap() {
		var existingReservation = new RoomReservation {
			Id = 1,
			Guests = new List<Guest>(),
			ReservationTime = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
			ArrivalTime = new DateTime(2024, 1, 5, 0, 0, 0, DateTimeKind.Utc),
			DepartureTime = new DateTime(2024, 1, 10, 0, 0, 0, DateTimeKind.Utc),
			Room = new Room { Id = 1, RoomNumber = "1", Type = RoomType.Single }
		};

		var newReservation = new RoomReservation {
			Id = 1,
			Guests = new List<Guest>(),
			ReservationTime = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
			ArrivalTime = new DateTime(2024, 1, 5, 0, 0, 0, DateTimeKind.Utc),
			DepartureTime = new DateTime(2024, 1, 10, 0, 0, 0, DateTimeKind.Utc),
			Room = new Room { Id = 1, RoomNumber = "1", Type = RoomType.Single }
		};

		var result = _roomReservationService.DoesReservationOverlapWithExistingReservation(existingReservation, newReservation);
		
		Assert.False(result);
	}
}
