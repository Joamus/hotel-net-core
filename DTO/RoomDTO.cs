namespace HotelBackendApi.DTO;

public class RoomDTO {
	public long Id { get; set; }

	public required string RoomNumber { get; set; }

	public required RoomType Type { get; set; }
}	
