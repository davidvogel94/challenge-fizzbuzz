using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FizzBuzz.Common.Configuration;
using FizzBuzz.Common.Exceptions;
using FizzBuzz.Common.Models;
using FizzBuzz.Common.Requests;
using FizzBuzz.Common.Responses;
using FizzBuzz.Data.Services;
using Microsoft.Extensions.Options;

namespace FizzBuzz.Api.Services;


/// <summary>
/// TODO: xmldoc
/// </summary>
public interface IFizzBuzzGameService
{
    /// <summary>
    /// TODO: xmldoc
    /// </summary>
    /// <param name="gameId"></param>
    /// <returns></returns>
    Task<FizzBuzzGetResponse> GetGameDataAsync(string gameId);

    /// <summary>
    /// TODO: xmldoc
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<FizzBuzzNewGameResponse> NewGameAsync(FizzBuzzNewGameRequest request);

    /// <summary>
    /// TODO: xmldoc
    /// </summary>
    /// <param name="gameData"></param>
    /// <returns></returns>
    Task<FizzBuzzAnswerResponse> ProcessAnswerAsync(string gameId, FizzBuzzAnswerRequest answer);
}


/// <inheritdoc cref="IFizzBuzzGameService" />
public sealed class FizzBuzzGameService : IFizzBuzzGameService
{
    private readonly IFizzBuzzDataService _dataService;
    private readonly FizzBuzzDefaultsConfig _fizzBuzzDefaultsConfig;

    public FizzBuzzGameService(IFizzBuzzDataService dataService, IOptions<FizzBuzzDefaultsConfig> fizzBuzzDefaultsConfig)
    {
        _dataService = dataService;
        _fizzBuzzDefaultsConfig = fizzBuzzDefaultsConfig.Value;
    }


    /// <inheritdoc cref="IFizzBuzzGameService.GetGameDataAsync" /> 
    public async Task<FizzBuzzGetResponse> GetGameDataAsync(string gameId)
    {
        var gameData = await _dataService.GetAsync(gameId);
        return new(gameData);
    }


    /// <inheritdoc cref="IFizzBuzzGameService.NewGameAsync" /> 
    public async Task<FizzBuzzNewGameResponse> NewGameAsync(FizzBuzzNewGameRequest request)
    {
        FizzBuzzGameData gameData = new()
        {
            GameConfiguration = new(_fizzBuzzDefaultsConfig) {
                PlayerName = request.PlayerName,
                Duration = request.Duration,
                MinNumber = request.MinNumber,
                MaxNumber = request.MaxNumber
            }
        };

        // If ruleset is provided other than the default, assign and validate it.
        if (request.Ruleset is not null)
        {
            gameData.GameConfiguration.Ruleset = request.Ruleset;

            ValidateRuleset(request);
        }

        await _dataService.SaveAsync(gameData);
        return new(gameData);
    }

    /// <inheritdoc cref="IFizzBuzzGameService.ProcessAnswerAsync" /> 
    public async Task<FizzBuzzAnswerResponse> ProcessAnswerAsync(string gameId, FizzBuzzAnswerRequest request)
    {
        var gameData = await _dataService.GetAsync(gameId);


        ValidateGameTime(gameData);
        

        var answerIsValid = CheckAnswer(gameData, request);


        if (answerIsValid)
        {
            gameData.IncrementScore();
        }

        gameData.GetNextNumber();

        await _dataService.SaveAsync(gameData);


        return new (
            answerIsValid,
            gameData
        );
    }


    private static void ValidateRuleset(FizzBuzzNewGameRequest request)
    {
        if ((request.Ruleset?.Count ?? 0) < 2)
        {
            throw new InvalidOperationException("Ruleset must contain at least 2 rules.");
        }

        // Ruleset keys should be integers, and should be greater than 1
        // (all numbers are divisible by 1 so it's no good for fizzbuzz)
        var keysAreValid = request.Ruleset?.All(rule =>
        {
            var isInt = int.TryParse(rule.Key, out int key);
            return isInt && key > 1;
        }) ?? false;

        if (false == keysAreValid)
        {
            throw new InvalidOperationException("Rule keys must be numbers greater than 1.");
        };
    }



    private void ValidateGameTime(FizzBuzzGameData gameData)
    { 
        long expireTime = gameData.CreatedTimestamp + (gameData.GameConfiguration?.Duration ?? 0);
        bool hasExpired = expireTime <= DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        if (hasExpired)
        {
            throw new FizzBuzzSessionExpiredException(gameData.ID);
        }
    }

    private bool CheckAnswer(FizzBuzzGameData gameData, FizzBuzzAnswerRequest request)
    {
        /*
         * Create a dictionary with /expected/ rule answers as keys, count of individual rule answers expected as value
         * i.e. for the following rules:
         *      | Divisible by | Expected answer |
         *      |           2  | Fizz            |
         *      |           3  | Buzz            |
         *      |           5  | Fizz            |
         *
         * This dictionary will contain
         *      | Expected Answer | Expected Count |
         *      |           Fizz  |  2             |
         *      |           Buzz  |  1             |
        */

        Dictionary<string,int> expectedAnswers = new();

        if (gameData?.GameConfiguration?.Ruleset is null)
            throw new NullReferenceException("Ruleset is null");

        foreach (var rule in gameData.GameConfiguration.Ruleset)
        {
            if (false == expectedAnswers.ContainsKey(rule.Value))
            {
                expectedAnswers.Add(rule.Value, 0);
            }

            var key = int.Parse(rule.Key);

            // i.e. is the number applicable to the current rule
            var isFizzBuzz = gameData.CurrentNumber % key == 0;

            if (isFizzBuzz)
            {
                expectedAnswers[rule.Value]++;
            }
        }

        // If none of the rule answers are expected for a given number
        // then set the only rule to be to expect the number itself as an answer exactly once.
        if (expectedAnswers.All(answer => answer.Value == 0))
        {
            expectedAnswers.Clear();
            expectedAnswers.Add($"{gameData.CurrentNumber}", 1);
        }


        foreach (var expectedAnswer in expectedAnswers)
        {
            var answerCount = request.Answers.Count(a => a.Equals(expectedAnswer.Key, StringComparison.InvariantCultureIgnoreCase));
            
            if (answerCount != expectedAnswer.Value)
            {
                return false;
            }
        }

        return true;
    }
}