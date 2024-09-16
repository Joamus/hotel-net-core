using HotelBackendApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using HotelBackendApi.Domain.Services;
using HotelBackendApi.Domain.Exceptions.RoomReservationExceptions;

namespace HotelBackendApi.Controllers;

[ApiController]
[Route("api/[controller]")]
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
        return await _context.RoomReservations.ToListAsync();
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
    public async Task<ActionResult<RoomReservation>> PostRoomReservation(RoomReservation roomReservation) {
        try {
            await _roomReservationService.CreateReservation(roomReservation);
            return CreatedAtAction("PostRoom", new { id = roomReservation.Id }, roomReservation);
        } catch (RoomIsUnavailableException e) {
            return Conflict(e);
        }
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<RoomReservation>> PutRoomReservation(long id, RoomReservation roomReservation) {
        var entry = await _context.RoomReservations.FindAsync(id);
        
        if (entry == null) {
            return NotFound();
        }
        
        _context.Entry(entry).State = EntityState.Modified;

        await _context.SaveChangesAsync();
        
        return Ok(roomReservation);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoomReservation(long id) {
        var reservation = await _context.RoomReservations.FindAsync(id);
        
        if (reservation == null) {
            return NotFound();
        }
        
        _context.Remove(id);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
}
