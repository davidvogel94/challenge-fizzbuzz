namespace FizzBuzz.Common.Constants;

using System.Text.Json;


internal static class ConfigurationDefaults
{

    internal static class FizzBuzz
    {
        internal const string PLAYER_NAME = "Anonymous";
        internal const string _RULESET_STR = "{\"2\":\"fizz\",\"3\":\"buzz\"}";
        internal static Dictionary<string,string> RULESET => JsonSerializer.Deserialize<Dictionary<string,string>>(_RULESET_STR)!;
        internal const int MIN_DURATION = 30;
        internal const int DURATION = 60;
        internal const int MIN_NUMBER = 1;
        internal const int MAX_NUMBER = 100;
    }

    

    internal static class MongoDB
    {
        internal const string APPLICATION_NAME = "fizzbuzz";
        internal const string DATABASE = "fizzbuzz";
        internal const string HOST = "localhost";
        internal const int CONNECT_TIMEOUT = 60;
        internal const int PORT = 27017;
        internal const bool ALLOW_INSECURE_TLS = false;
    }
}
