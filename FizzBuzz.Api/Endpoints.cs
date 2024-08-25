using FizzBuzz.Api.Services;
using FizzBuzz.Common.Requests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;

namespace FizzBuzz.Api;

internal static class Endpoints
{
    public static void ConfigureEndpoints(this WebApplication app)
    {
        // CREATE GAME
        app.MapPut("/", 
            async ([FromBody] FizzBuzzNewGameRequest request, IFizzBuzzGameService gameService) 
            => await gameService.NewGameAsync(request))
        .WithName("PutNewGame")
        .WithOpenApi();


        // GET EXISTING GAME
        app.MapGet("/{gameId}", 
            async ([FromRoute] string gameId, IFizzBuzzGameService gameService) 
            => await gameService.GetGameDataAsync(gameId))
        .WithName("GetGame")
        .WithOpenApi();


        // SUBMIT GAME ANSWER
        app.MapPost("/{gameId}", 
            async ([FromRoute] string gameId, [FromBody] FizzBuzzAnswerRequest answer, IFizzBuzzGameService gameService) 
            => await gameService.ProcessAnswerAsync(gameId, answer))
        .WithName("PostGameAnswer")
        .WithOpenApi();
    }
}