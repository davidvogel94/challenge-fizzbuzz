using System.Net;

namespace FizzBuzz.Common.Exceptions;

public sealed class FizzBuzzSessionExpiredException : FizzBuzzException
{
    public FizzBuzzSessionExpiredException(string gameId) : base (gameId, $"Session has expired for game ID '{gameId}'", HttpStatusCode.Gone) { }
}