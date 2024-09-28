using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelBackendApi;
using HotelBackendApi.DTO;

namespace HotelBackendApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly ILogger<RoomReservation> _logger;
        private readonly MainContext _context;

        public RoomController(MainContext context, ILogger<RoomReservation> logger)
        {
            _logger = logger;
            _context = context;
        }

        // GET: Room
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomDTO>>> GetRooms()
        {
            return await _context.Rooms.Select(room => RoomToDTO(room)).ToListAsync();
        }

        // GET: Room/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDTO>> GetRoom(long id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
            {
                return NotFound();
            }

            return RoomToDTO(room);
        }

        // PUT: Room/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoom(long id, RoomDTO roomDTO)
        {
            if (id != roomDTO.Id)
            {
                return NotFound();
            }

            _context.Entry(roomDTO).State = EntityState.Modified;
            
            // var room = await _context.Rooms.FindAsync(roomDTO.RoomNumber);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: Room
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RoomDTO>> PostRoom(RoomDTO roomDTO)
        {
            var room = new Room {
                RoomNumber = roomDTO.RoomNumber,
                Type = roomDTO.Type,
            };

            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostRoom", new { id = room.Id }, RoomToDTO(room));
        }

        // DELETE: Room/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(long id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoomExists(long id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }
        
        private static RoomDTO RoomToDTO(Room room) {
            return new RoomDTO {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                Type = room.Type
            };
        }

    }
}
