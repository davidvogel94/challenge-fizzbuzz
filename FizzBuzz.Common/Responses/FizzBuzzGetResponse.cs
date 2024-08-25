using System.Net;
using FizzBuzz.Common.Models;

namespace FizzBuzz.Common.Responses;

public record FizzBuzzGetResponse(FizzBuzzGameData? GameData, string? Error = null);