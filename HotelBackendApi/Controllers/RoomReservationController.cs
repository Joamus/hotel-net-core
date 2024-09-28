using HotelBackendApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using HotelBackendApi.Domain.Services;
using HotelBackendApi.Domain.Exceptions.RoomReservationExceptions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HotelBackendApi.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Guest")]
public class RoomReservationController : ControllerBase
{
    private readonly ILogger<RoomReservationController> _logger;
    private readonly MainContext _context;
    
    private readonly RoomReservationService _roomReservationService;

    public RoomReservationController(ILogger<RoomReservationController> logger, MainContext context, RoomReservationService roomReservationService)
    {
        _logger = logger;
        _context = context;
        _roomReservationService = roomReservationService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomReservation>>> GetRoomReservations()
    {
        string? userId = null;
        var claims = HttpContext.User;

        if (claims.IsInRole("Manager")) {
            userId = HttpContext.Request.Query["user"];
            
            if (!await _context.Users.AnyAsync(user => user.Id == userId)) {
                return NotFound("User not found");
            }
        } else {
            userId = claims.GetUserId();
        }
        return await _context.RoomReservations.Where(roomReservation => userId != null ? roomReservation.UserId == userId : true).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoomReservation>> GetRoomReservation(long id) {
        var reservation = await _context.RoomReservations.FindAsync(id);

        if (reservation == null)
        {
            return NotFound();
        }

        return reservation;
    }
    
    [HttpPost]
    [Authorize(Roles = "Guest")]
    public async Task<ActionResult<RoomReservationDTO>> PostRoomReservation(RoomReservationDTO roomReservationDTO) {
        try {
            var roomReservation = new RoomReservation {
                Id = roomReservationDTO.Id,
                Guests = new List<Guest>(),
                DepartureTime = roomReservationDTO.DepartureTime,
                ArrivalTime = roomReservationDTO.ArrivalTime,
                ReservationTime = roomReservationDTO.ReservationTime,
                RoomId = roomReservationDTO.RoomId,
                UserId = HttpContext.User.GetUserId()
            };

            await _roomReservationService.CreateReservation(roomReservation);
            return CreatedAtAction("PostRoomReservation", new { id = roomReservation.Id }, RoomReservationToDTO(roomReservation));
        } catch (RoomIsUnavailableException e) {
            return Conflict(e);
        }
    }
    
    [HttpPost("{id}/approve")]
    [Authorize(Roles = "Manager")]
    [Produces("application/json")]
    public async Task<ActionResult<RoomReservationDTO>> ApproveRoomReservation(long id) {
        var roomReservation = await _context.RoomReservations.FindAsync(id);
        
        if (roomReservation == null) {
            return NotFound();
        }
        
        roomReservation.Approved = true;
        
        _context.Update(roomReservation);
        _context.Entry(roomReservation).State = EntityState.Modified;
        
        await _context.SaveChangesAsync();
        
        return Ok(RoomReservationToDTO(roomReservation));
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> PutRoomReservation(long id, RoomReservationDTO roomReservationDTO) {
        if (id != roomReservationDTO.Id) {
            return NotFound();
        }

        var originalRoomReservation = await _context.RoomReservations.FindAsync(id);
        
        if (originalRoomReservation is null) {
            return NotFound();
        }

        roomReservationDTO.UserId = originalRoomReservation.UserId;

        _context.Entry(roomReservationDTO).State = EntityState.Modified;

        try {
            await _context.SaveChangesAsync();
        } catch (DbUpdateConcurrencyException) {
            if (!RoomReservationExists(id)) {
                return NotFound();
            }
            throw;
        }
        
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoomReservation(long id) {
        var reservation = await _context.RoomReservations.FindAsync(id);
        
        if (reservation == null) {
            return NotFound();
        }

        var claims = HttpContext.User;

        if (claims.GetUserId() == reservation.UserId || claims.IsInRole("Manager")) {
            _context.Remove(id);
            await _context.SaveChangesAsync();
            
            return NoContent();
        } else {
            return NotFound();
        }
    }
    
    private static RoomReservationDTO RoomReservationToDTO(RoomReservation roomReservation) {
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
    
    private bool RoomReservationExists(long id) {
        return _context.RoomReservations.Any(e => e.Id == id);
    }
}
