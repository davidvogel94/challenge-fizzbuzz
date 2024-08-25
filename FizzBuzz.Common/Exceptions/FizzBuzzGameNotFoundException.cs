using System;
using System.Net;

namespace FizzBuzz.Common.Exceptions;

public sealed class FizzBuzzGameNotFoundException : FizzBuzzException
{
    public FizzBuzzGameNotFoundException(string gameId)
        : base(gameId, $"Game not found for ID '{gameId}'", HttpStatusCode.NotFound)
    {
    }
}