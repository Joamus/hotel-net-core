
namespace HotelBackendApi;

public class RoomReservationDTO {
	
	public long Id { get; set; }
	public ICollection<Guest> Guests { get; set; } = null!;
	
	public DateTime ReservationTime { get; set; }
	
	public DateTime ArrivalTime { get; set; }
	
	public DateTime DepartureTime { get; set; }
	
	public long RoomId { get; set; }

	public string? UserId { get; set; } = null!;
}