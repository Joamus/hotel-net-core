using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
		var now = DateTime.Now;
		roomReservation.ReservationTime = now;
		
		if (roomReservation.ArrivalTime <= now) {
			throw new Exception("Arrival time cannot be in the past");
		}

		if (roomReservation.DepartureTime <= roomReservation.ArrivalTime) {
			throw new Exception("Departure time must be later than arrival time");
		}

		var allRoomReservations = await _context.RoomReservations.Where(reservation => reservation.RoomId == roomReservation.RoomId).ToListAsync();
		bool overlappingReservation = allRoomReservations.Find(reservation => DoesReservationOverlapWithExistingReservation(reservation, roomReservation)) != null;
		
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

		return earliestReservation.DepartureTime > latestReservation.ArrivalTime;
	}
}