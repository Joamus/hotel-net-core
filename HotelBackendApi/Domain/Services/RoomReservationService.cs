using HotelBackendApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using HotelBackendApi.Domain.Exceptions.RoomReservationExceptions;

namespace HotelBackendApi.Domain.Services;

public class RoomReservationService {
	readonly ILogger<RoomReservationService> _loggerContext;
	readonly MainContext _context;

	public RoomReservationService(ILogger<RoomReservationService> loggerContext, MainContext context) {
		_loggerContext = loggerContext;
		_context = context;
	}

	public async Task<ActionResult<RoomReservation>> CreateReservation(RoomReservation roomReservation) {
		var allRoomReservations = await _context.RoomReservations.Where(reservation => reservation.RoomId == roomReservation.RoomId).ToListAsync();
		bool overlappingReservation = allRoomReservations.Find(reservation => DoesReservationOverlapWithExistingReservation(reservation, roomReservation)) != null;
		// bool reservationAtSameTime = await _context.RoomReservations.Where(reservation => DoesReservationOverlapWithExistingReservation(reservation, roomReservation)).AsAsyncEnumerable().AnyAsync();
		
		if (overlappingReservation) {
			throw new RoomIsUnavailableException();
		}

        await _context.RoomReservations.AddAsync(roomReservation);
        await _context.SaveChangesAsync();
		
		return roomReservation;
	}
	
	public bool DoesReservationOverlapWithExistingReservation(RoomReservation existingReservation, RoomReservation newReservation) {
		if (existingReservation.RoomId != newReservation.RoomId) {
			return false;
		}
		RoomReservation earliestReservation = existingReservation.ArrivalTime > newReservation.ArrivalTime ? newReservation : existingReservation;
		RoomReservation latestReservation = earliestReservation == newReservation ? existingReservation : newReservation;

		return earliestReservation.DepartureTime < latestReservation.ArrivalTime;
	}
}