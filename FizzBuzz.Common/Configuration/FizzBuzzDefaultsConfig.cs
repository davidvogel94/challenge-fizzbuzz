namespace FizzBuzz.Common.Configuration;

using Defaults = Constants.ConfigurationDefaults.FizzBuzz;

public sealed class FizzBuzzDefaultsConfig
{
    public string PlayerName { get; set; } = Defaults.PLAYER_NAME;
    public Dictionary<string,string> Ruleset = Defaults.RULESET;
    public int MinDuration { get; set; } = Defaults.MIN_DURATION;
    public int Duration { get; set; } = Defaults.DURATION;
    public int MinNumber { get; set; } = Defaults.MIN_NUMBER;
    public int MaxNumber { get; set; } = Defaults.MAX_NUMBER;
}