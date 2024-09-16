using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace HotelBackendApi;

[Index(nameof(RoomNumber), IsUnique = true)]
public class Room {
	public long Id { get; set; }
	public required string RoomNumber { get; set; }
	public required RoomType Type { get; set;}

	public ICollection<RoomReservation> roomReservations { get; set; } = new List<RoomReservation>();
}
