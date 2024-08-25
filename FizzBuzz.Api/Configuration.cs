using System.Text.Json;
using System.Text.Json.Serialization;
using FizzBuzz.Common.Configuration;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FizzBuzz.Api;

public static class ConfigurationExtensions
{
    public static IServiceCollection ConfigureFizzBuzz(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<FizzBuzzDefaultsConfig>(config.GetSection(nameof(FizzBuzzDefaultsConfig)));
        
        return services;
    }

    public static IServiceCollection ConfigureDBConnection(this IServiceCollection services, IConfiguration config)
    {
        // Required config (e.g. database configs)
        services.Configure<MongoDBConnectionConfig>(config.GetRequiredSection(nameof(MongoDBConnectionConfig)));
    }


    public static IServiceCollection ConfigureJsonSerializerDefaults(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<JsonOptions>(options => {
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.SerializerOptions.AllowTrailingCommas = true;
            options.SerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.IncludeFields = false;
            options.SerializerOptions.UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip;
        });
        return services;
    }
}