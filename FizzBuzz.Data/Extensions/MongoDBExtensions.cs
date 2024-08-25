using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Entities;


using FizzBuzz.Common.Configuration;


namespace FizzBuzz.Data.Extensions;

public static class MongoDBExtensions
{
    /// <summary>
    /// TODO xmldoc
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param> 
    public static void AddMongoDB(this IServiceCollection services, MongoDBConnectionConfig? config = null) 
    => services.AddTransient(factory =>
    {
        config ??= factory
            .GetRequiredService<IOptions<MongoDBConnectionConfig>>()
            .Value;

        return new DBContext(config.Database, new MongoClientSettings
        {
            Server = new MongoServerAddress(config.Host, config.Port),
            Credential = MongoCredential.CreateCredential(config.Database, config.User, config.Password),
            AllowInsecureTls = config.AllowInsecureTls,
            ApplicationName = config.ApplicationName,
            ConnectTimeout = TimeSpan.FromSeconds(config.ConnectTimeout),
        });
    });
}