using System.Net;

namespace HotelBackendApi.Exceptions;
public class HttpException : Exception {
	public HttpStatusCode statusCode { get; }
	public string message { get; }
	public HttpException(HttpStatusCode statusCode, string message) { 
		this.statusCode = statusCode;
		this.message = message;
	}
}