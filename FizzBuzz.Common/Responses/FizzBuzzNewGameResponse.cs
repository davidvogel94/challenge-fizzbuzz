using System.Net;
using FizzBuzz.Common.Models;

namespace FizzBuzz.Common.Responses;

public record FizzBuzzNewGameResponse(FizzBuzzGameData? GameData);