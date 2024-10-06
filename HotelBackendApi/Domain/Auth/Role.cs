namespace HotelBackendApi.Authorization;
public static class Role {
	
	public static IReadOnlyList<string> Roles = new List<string> {
		Guest,
		Manager
	};
	
	public const string Guest = "Guest";
	public const string Manager = "Manager";
}