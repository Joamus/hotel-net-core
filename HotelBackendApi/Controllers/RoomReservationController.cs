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
using HotelBackendApi.Authorization;
using HotelBackendApi.Domain;

namespace HotelBackendApi.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Policy = "MinGuest")]
public class RoomReservationController : ControllerBase
{
    private readonly ILogger<RoomReservationController> Logger;
    private readonly MainContext Context;
    
    private readonly RoomReservationService RoomReservationService;

    public RoomReservationController(ILogger<RoomReservationController> logger, MainContext context, RoomReservationService roomReservationService)
    {
        Logger = logger;
        Context = context;
        RoomReservationService = roomReservationService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomReservation>>> GetRoomReservations(string? userId)
    {
        var requestingUser = HttpContext.User;
        return await RoomReservationService.GetRoomReservations(userId, requestingUser);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoomReservation>> GetRoomReservation(long id) {
        var reservation = await Context.RoomReservations.FindAsync(id);

        if (reservation == null)
        {
            return ApiError.NotFound().ToHttpError();
        }

        return reservation;
    }
    
    [HttpPost]
    public async Task<ActionResult<RoomReservationDTO>> PostRoomReservation(RoomReservationDTO roomReservationDTO) {
        var requestingUser = HttpContext.User;
        return await RoomReservationService.PostRoomReservation(roomReservationDTO, requestingUser);
    }
    
    [HttpPost("{id}/approve")]
    [Authorize(Roles = "Manager")]
    [Produces("application/json")]
    public async Task<ActionResult<RoomReservationDTO>> ApproveRoomReservation(long id) {
        var roomReservation = await Context.RoomReservations.FindAsync(id);

        if (roomReservation == null) {
            return ApiError.NotFound().ToHttpError();
        }
        
        await RoomReservationService.ApproveRoomReservation(roomReservation);
        return RoomReservationService.RoomReservationToDTO(roomReservation);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> PutRoomReservation(long id, RoomReservationDTO roomReservationDTO) {
        await RoomReservationService.PutRoomReservation(id, roomReservationDTO, HttpContext);
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoomReservation(long id) {
        await RoomReservationService.DeleteRoomReservation(id, HttpContext);
        return NoContent();
    }
}
