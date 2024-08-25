using System;
using System.Net;

namespace FizzBuzz.Common.Exceptions;

public class FizzBuzzException : Exception
{
    public string GameId { get; }
    public HttpStatusCode StatusCode { get; }
    
    public FizzBuzzException(string gameId, string message = "", HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        : base(message)
    {
        this.GameId = gameId;
        this.StatusCode = statusCode;
    }
}