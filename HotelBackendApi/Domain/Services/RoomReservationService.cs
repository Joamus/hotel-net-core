using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using HotelBackendApi.Authorization;
using FluentResults;

namespace HotelBackendApi.Domain.Services;

public class RoomReservationService {
	readonly ILogger<RoomReservationService> LoggerContext;
	readonly MainContext Context;

	public RoomReservationService(ILogger<RoomReservationService> loggerContext, MainContext context) {
		LoggerContext = loggerContext;
		Context = context;
	}

    public async Task<ActionResult<IEnumerable<RoomReservation>>> GetRoomReservations(string? userId, ClaimsPrincipal requestingUser)
    {
        if (requestingUser.IsInRole(Role.Manager))
        {
            if (userId != null && !await Context.Users.AnyAsync(user => user.Id == userId))
            {
                return ApiError.NotFound("User not found").ToHttpError();
            }
        } else {
            userId = requestingUser.GetUserId();
        }
        return await Context.RoomReservations.Where(roomReservation => userId == null || roomReservation.UserId == userId).ToListAsync();
    }

    public async Task<ActionResult<RoomReservationDTO>> PostRoomReservation(RoomReservationDTO roomReservationDTO, ClaimsPrincipal requestingUser) {
            string? userId = requestingUser.IsInRole("Manager") ? roomReservationDTO.UserId : requestingUser.GetUserId();
            userId ??= requestingUser.GetUserId();

            var roomReservation = new RoomReservation {
                Id = roomReservationDTO.Id,
                Guests = new List<Guest>(),
                DepartureTime = roomReservationDTO.DepartureTime,
                ArrivalTime = roomReservationDTO.ArrivalTime,
                ReservationTime = roomReservationDTO.ReservationTime,
                RoomId = roomReservationDTO.RoomId,
                UserId = userId
            };

            var now = DateTime.Now;
            roomReservation.ReservationTime = now;
            
            // Results<> result = 
            if (roomReservation.ArrivalTime <= now) {
                return ApiError.Conflict("Arrival time cannot be in the past").ToHttpError();
            }

            if (roomReservation.DepartureTime <= roomReservation.ArrivalTime) {
                return ApiError.Conflict("Departure time must be later than arrival time").ToHttpError();
            }

            var allRoomReservations = await Context.RoomReservations.Where(reservation => reservation.RoomId == roomReservation.RoomId).ToListAsync();
            bool overlappingReservation = allRoomReservations.Find(reservation => DoesReservationOverlapWithExistingReservation(reservation, roomReservation)) != null;
            
            if (overlappingReservation) {
                return ApiError.Conflict("Room is unavailable").ToHttpError();
            }

            await Context.RoomReservations.AddAsync(roomReservation);
            await Context.SaveChangesAsync();
		
            return RoomReservationToDTO(roomReservation);
    }

    public async Task<ActionResult> PutRoomReservation(long id, RoomReservationDTO roomReservationDTO, HttpContext httpContext) {
        if (id != roomReservationDTO.Id) {
            return ApiError.NotFound().ToHttpError();
        }

        var originalRoomReservation = await Context.RoomReservations.FindAsync(id);
        
        if (originalRoomReservation == null) {
            return ApiError.NotFound().ToHttpError();
        }

        var requestingUser = httpContext.User;
        if (!requestingUser.IsInRole("Manager")) {
            roomReservationDTO.UserId = originalRoomReservation.UserId;
        }

        Context.Entry(roomReservationDTO).State = EntityState.Modified;

        try {
            await Context.SaveChangesAsync();
        } catch (DbUpdateConcurrencyException) {
            if (!RoomReservationExists(id)) {
                return ApiError.NotFound().ToHttpError();
            } else {
                return ApiError.InternalError("internal_error", "An unknown error occured").ToHttpError();
            }
        }
        return new EmptyResult();
    }

    public async Task<IActionResult> DeleteRoomReservation(long id, HttpContext httpContext) {
        var reservation = await Context.RoomReservations.FindAsync(id);
        
        if (reservation == null) {
            return ApiError.NotFound().ToHttpError();
        }

        var requestingUser = httpContext.User;

        if (requestingUser.GetUserId() == reservation.UserId || requestingUser.IsInRole("Manager")) {
            Context.Remove(id);
            await Context.SaveChangesAsync();
            
            return new EmptyResult();
        } else {
            return ApiError.NotFound().ToHttpError();
        }
    }
	
	public static bool DoesReservationOverlapWithExistingReservation(RoomReservation existingReservation, RoomReservation newReservation) {
		if (existingReservation.RoomId != newReservation.RoomId) {
			return false;
		}
		RoomReservation earliestReservation = existingReservation.ArrivalTime > newReservation.ArrivalTime ? newReservation : existingReservation;
		RoomReservation latestReservation = earliestReservation == newReservation ? existingReservation : newReservation;

		return earliestReservation.DepartureTime > latestReservation.ArrivalTime;
	}

    public async Task<ActionResult<RoomReservationDTO>> ApproveRoomReservation(RoomReservation roomReservation) {
        roomReservation.Approved = true;
        
        Context.Update(roomReservation);
        Context.Entry(roomReservation).State = EntityState.Modified;
        
        await Context.SaveChangesAsync();
        
        return RoomReservationToDTO(roomReservation);
    }

    public static RoomReservationDTO RoomReservationToDTO(RoomReservation roomReservation) {
        return new RoomReservationDTO {
            Id = roomReservation.Id,
            Guests = roomReservation.Guests,
            ReservationTime = roomReservation.ReservationTime,
            ArrivalTime = roomReservation.ArrivalTime,
            DepartureTime = roomReservation.DepartureTime,
            RoomId = roomReservation.RoomId,
            UserId = roomReservation.UserId
        };
    }
    
    public bool RoomReservationExists(long id) {
        return Context.RoomReservations.Any(e => e.Id == id);
    }
}