using FizzBuzz.Api;
using FizzBuzz.Api.Middleware;
using FizzBuzz.Api.Services;
using FizzBuzz.Data.Extensions;
using FizzBuzz.Data.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration
    .AddEnvironmentVariables()
    .Build();

var services = builder.Services;


/*
 * Add Service Configurations
 */
services
    .AddOptions()
    .ConfigureJsonSerializerDefaults(config)
    .ConfigureDBConnection(config)
    .ConfigureFizzBuzz(config)
    ;

/*
 * Configure services
 */
services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

services.AddMongoDB();
services.AddTransient<IFizzBuzzDataService, FizzBuzzDataService>();
services.AddTransient<IFizzBuzzGameService, FizzBuzzGameService>();

/*
 * Build app
 */
var app = builder.Build();

/*
 * Configure the HTTP request pipeline.
 */
if (app.Environment.IsDevelopment())
{
    app
        .UseSwagger()
        .UseSwaggerUI();
}

app.UseHttpsRedirection();
app.ConfigureEndpoints();


/*
 * Configure Custom Middleware
 */
app.UseMiddleware<ExceptionResponseMiddleware>();

/*
 * GO
 */
app.Run();

