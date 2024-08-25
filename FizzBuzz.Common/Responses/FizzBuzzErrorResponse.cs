using System.Net;

namespace FizzBuzz.Common.Responses;

public record FizzBuzzErrorResponse(HttpStatusCode StatusCode, string Message);