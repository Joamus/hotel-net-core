namespace HotelBackendApi;

public class Guest {
	public long Id { get; set; }
	public string? Name { get; set; }

	public string? Birthdate { get; set; }
	
	public Title Title { get; set; }
	
}

public enum Title {
	Mx,
	Mr,
	Ms,
}