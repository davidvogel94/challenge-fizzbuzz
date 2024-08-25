namespace FizzBuzz.Common.Requests;

using System.Collections.Generic;
using System.ComponentModel;

using MongoDB.Entities;

using FizzBuzz.Common.Configuration;
using Defaults = Constants.ConfigurationDefaults.FizzBuzz;

public sealed class FizzBuzzNewGameRequest
{
    private readonly FizzBuzzDefaultsConfig _defaults;

    public FizzBuzzNewGameRequest()
    {
        _defaults = new();
    }

    public FizzBuzzNewGameRequest(FizzBuzzDefaultsConfig defaultsConfig)
    {
        _defaults = defaultsConfig;
    }

    private string? _playerName = null;
    [DefaultValue(Defaults.PLAYER_NAME)] 
    [Field("player_name")]
    public string PlayerName { 
        get => _playerName ??= _defaults.PlayerName; 
        set => _playerName = value; 
    }
    
    private int? _duration = null;
    [DefaultValue(Defaults.DURATION)]
    [Field("duration_s")] 
    public int Duration { 
        get => _duration ??= _defaults.Duration; 
        set => _duration = value; 
    }

    private Dictionary<string,string>? _ruleset = null;
    [DefaultValue(Defaults._RULESET_STR)]
    [Field("Ruleset")]
    public Dictionary<string,string> Ruleset { 
        get => _ruleset ??= _defaults.Ruleset; 
        set => _ruleset = value; 
    }

    private int? _minNumber = null;
    private int? _maxNumber = null;
    [DefaultValue(Defaults.MIN_NUMBER)]
    [Field("min_number")]
    public int MinNumber { 
        get => _minNumber ??= _defaults.MinNumber;
        set {
            if (value < 0)
                throw new ArgumentOutOfRangeException("Minimum game number cannot be less than 0.");

            if (_maxNumber is not null && value > _maxNumber)
                throw new ArgumentOutOfRangeException("Minimum game number cannot be greater than the specified maximum game number.");

            _minNumber = value;
        }
    }
    
    [DefaultValue(Defaults.MAX_NUMBER)]
    [Field("max_number")]
    public int MaxNumber { 
        get => _maxNumber ??= _defaults.MaxNumber;
        set {
            if (_minNumber is not null && value < _minNumber)
                throw new ArgumentOutOfRangeException("Maximum game number cannot be greater than the specified minimum game number.");
            
            _maxNumber = value;
        }
    }
};