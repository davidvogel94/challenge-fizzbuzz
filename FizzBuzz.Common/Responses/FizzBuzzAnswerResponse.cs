using System.Net;
using FizzBuzz.Common.Models;

namespace FizzBuzz.Common.Responses;

public record FizzBuzzAnswerResponse(bool Correct, FizzBuzzGameData? GameData);