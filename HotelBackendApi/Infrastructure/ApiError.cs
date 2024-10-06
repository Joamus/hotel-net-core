using FluentResults;
using HotelBackendApi;
using Microsoft.AspNetCore.Mvc;

namespace HotelBackendApi.Domain;

public class ApiError: IError {


	private ApiError(string code, string? description, ApiErrorType errorType) {
		Property = code;
		Description = description;
		ErrorType = errorType;
	}
	
	public string Property { get; }
	
	public string? Description { get; } 
	public ApiErrorType ErrorType { get;}

    public List<IError> Reasons => new List<IError>();

    public string Message => Description ?? "";

    public Dictionary<string, object> Metadata => new Dictionary<string, object>();

    #region NotFound
    public static ApiError NotFound() {
		return new ApiError("Not found", null, ApiErrorType.NotFound);
	}
	public static ApiError NotFound(string property) {
		return new ApiError(property, null, ApiErrorType.NotFound);
	}
	public static ApiError NotFound(string property, string description) {
		return new ApiError(property, description, ApiErrorType.NotFound);
	}

	#endregion

	#region Validation
	public static ApiError Validation() {
		return new ApiError("Validation", null, ApiErrorType.Validation);
	}
	public static ApiError Validation(string property) {
		return new ApiError(property, null, ApiErrorType.Validation);
	}
	public static ApiError Validation(string property, string description) {
		return new ApiError(property, description, ApiErrorType.Validation);
	}

	#endregion

	#region Conflict
	public static ApiError Conflict() {
		return new ApiError("Conflict", null, ApiErrorType.Validation);
	}
	public static ApiError Conflict(string property) {
		return new ApiError(property, null, ApiErrorType.Validation);
	}
	public static ApiError Conflict(string property, string description) {
		return new ApiError(property, description, ApiErrorType.Validation);
	}

	#endregion
	
	#region InternalError
	public static ApiError InternalError(string property, string description) {
		return new ApiError(property, description, ApiErrorType.InternalError);
	}

	#endregion
	
	public ProblemDetails ToProblemDetails() {
    /// <param name="statusCode">The value for <see cref="ProblemDetails.Status" />.</param>
    /// <param name="detail">The value for <see cref="ProblemDetails.Detail" />.</param>
    /// <param name="instance">The value for <see cref="ProblemDetails.Instance" />.</param>
    /// <param name="title">The value for <see cref="ProblemDetails.Title" />.</param>
    /// <param name="type">The value for <see cref="ProblemDetails.Type" />.</param>
		var details = new ProblemDetails {
			Status = (int) ErrorType,
			Title = Property,
			// Title = ErrorType.ToString(),
			Detail = Description
		};
		
		return details;
	}
	
	public ObjectResult ToHttpError() {
		return new ObjectResult(this);
	}
}

public enum ApiErrorType {
	InternalError = 500,
	Validation = 422,
	NotFound = 404,
	Conflict = 422
}