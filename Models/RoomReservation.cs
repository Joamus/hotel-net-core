namespace HotelBackendApi;

public class RoomReservation {
	
	public long Id { get; set; }
	public ICollection<Guest> Guests { get; set; } = null!;
	
	public DateTime ReservationTime { get; set; }
	
	public DateTime ArrivalTime { get; set; }
	
	public DateTime DepartureTime { get; set; }
	
	public ICollection<Order> Purchases { get; set; } = null!;
	
	public long RoomId { get; set; }
}