namespace FizzBuzz.Common.Models;

using System;
using MongoDB.Entities;
using FizzBuzz.Common.Requests;
using System.ComponentModel.DataAnnotations;

public class FizzBuzzGameData : Entity
{
    private static Random? _random = null;
    private static Random Random => _random ??= new Random();

    [Field("game_config")]
    public FizzBuzzNewGameRequest? GameConfiguration { get; set; }

    [Field("created_timestamp")]    
    public long CreatedTimestamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    

    [Field("score")]
    public int Score { get; set; } = 0;

    private int? _currentNumber;
    [Field("current_number")]
    public int CurrentNumber { 
        get => _currentNumber ??= GetNextNumber();
        set => _currentNumber = value; 
    }

    public int GetNextNumber() => 
        CurrentNumber = Random.Next(GameConfiguration!.MinNumber, GameConfiguration.MaxNumber);
        
    public int IncrementScore() => Score += 1;
}
