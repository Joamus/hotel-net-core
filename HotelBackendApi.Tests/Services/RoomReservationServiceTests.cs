using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using HotelBackendApi;
using HotelBackendApi.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;

namespace HotelBackendApi.Tests.Services;
public class RoomReservationServiceTests
{ 
	readonly ILogger<RoomReservationService> _loggerContext;
	readonly MainContext _context;
	
	readonly RoomReservationService _roomReservationService;
	
	public RoomReservationServiceTests() {
		_context = new MainContext(new DbContextOptions<MainContext>());
		_loggerContext = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<RoomReservationService>();
		// _loggerContext = logger;
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
			RoomId = 1
		};

		var newReservation = new RoomReservation {
			Id = 1,
			Guests = new List<Guest>(),
			ReservationTime = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
			ArrivalTime = new DateTime(2024, 1, 11, 0, 0, 0, DateTimeKind.Utc),
			DepartureTime = new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc),
			RoomId = 1
		};

		var result = _roomReservationService.DoesReservationOverlapWithExistingReservation(existingReservation, newReservation);
		
		Assert.False(result);
	}

	[Fact]
	public void DoesReservationOverlapWithExistingReservation_SameRoomWithOverlap() {
		var existingReservation = new RoomReservation {
			Id = 1,
			Guests = new List<Guest>(),
			ReservationTime = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
			ArrivalTime = new DateTime(2024, 1, 5, 0, 0, 0, DateTimeKind.Utc),
			DepartureTime = new DateTime(2024, 1, 10, 0, 0, 0, DateTimeKind.Utc),
			RoomId = 1
		};

		var newReservation = new RoomReservation {
			Id = 1,
			Guests = new List<Guest>(),
			ReservationTime = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
			ArrivalTime = new DateTime(2024, 1, 5, 0, 0, 0, DateTimeKind.Utc),
			DepartureTime = new DateTime(2024, 1, 10, 0, 0, 0, DateTimeKind.Utc),
			RoomId = 1
		};

		var result = _roomReservationService.DoesReservationOverlapWithExistingReservation(existingReservation, newReservation);
		
		Assert.True(result);
	}
}
