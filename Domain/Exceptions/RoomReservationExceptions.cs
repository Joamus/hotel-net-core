namespace HotelBackendApi.Domain.Exceptions.RoomReservationExceptions;

public class RoomIsUnavailableException : Exception {
	public RoomIsUnavailableException() : base("Room is unavailable") {}
}