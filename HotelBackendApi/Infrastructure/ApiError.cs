using HotelBackendApi;
using Microsoft.AspNetCore.Mvc;

public class ApiError {


	private ApiError(string code, string description, ApiErrorType errorType) {
		Property = code;
		Description = description;
		ErrorType = errorType;
	}
	
	public string Property { get; }
	
	public string Description { get; } 
	public ApiErrorType ErrorType { get;}
	
	public ApiError NotFound(string property, string description) {
		return new ApiError(property, description, ApiErrorType.NotFound);
	}

	public ApiError Validation(string property, string description) {
		return new ApiError(property, description, ApiErrorType.Validation);
	}

	public ApiError Conflict(string property, string description) {
		return new ApiError(property, description, ApiErrorType.Validation);
	}
	
	public ApiError InternalError(string property, string description) {
		return new ApiError(property, description, ApiErrorType.InternalError);
	}
	
	public ProblemDetails ToProblemDetails() {
    /// <param name="statusCode">The value for <see cref="ProblemDetails.Status" />.</param>
    /// <param name="detail">The value for <see cref="ProblemDetails.Detail" />.</param>
    /// <param name="instance">The value for <see cref="ProblemDetails.Instance" />.</param>
    /// <param name="title">The value for <see cref="ProblemDetails.Title" />.</param>
    /// <param name="type">The value for <see cref="ProblemDetails.Type" />.</param>
		var details = new ProblemDetails {
			Status = (int) ErrorType,
			Title = ErrorType.ToString(),
			Detail = Description
		};
		
		return details;
	}
	
	// public ObjectResult ToHttpError() {
	// 	return new ObjectResult(
	// 		StatusCode = (int) ErrorType

	// 	)
	// 	switch (ErrorType) {
	// 		default:
	// 			return new InternalError()
	// 			return ObjectResult
	// 		break;
	// 	}
	// }
}

public enum ApiErrorType {
	InternalError = 500,
	Validation = 422,
	NotFound = 404,
	Conflict = 422
}