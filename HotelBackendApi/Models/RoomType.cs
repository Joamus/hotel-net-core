using Newtonsoft.Json.Converters;

namespace HotelBackendApi;
// [Required]
// [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
public enum RoomType: byte {
	
	Single = 1,
	Double = 2,
	PenthouseSuite = 3
}