using System.ComponentModel.DataAnnotations;

using Defaults = FizzBuzz.Common.Constants.ConfigurationDefaults.MongoDB;

namespace FizzBuzz.Common.Configuration;

public sealed class MongoDBConnectionConfig
{
    public string Host { get; set; } = Defaults.HOST;

    [Required]
    public string? User { get; set; }

    [Required]
    public string? Password { get; set; }

    public string Database { get; set; } = Defaults.DATABASE;

    public int Port { get; set; } = Defaults.PORT;

    public bool AllowInsecureTls { get; set; } = Defaults.ALLOW_INSECURE_TLS;

    public string ApplicationName { get; set; } = Defaults.APPLICATION_NAME;
    public int ConnectTimeout { get; set; } = Defaults.CONNECT_TIMEOUT;
}