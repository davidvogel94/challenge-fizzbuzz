using System;
using System.Net;
using System.Threading.Tasks;
using FizzBuzz.Common.Exceptions;
using FizzBuzz.Common.Responses;
using Microsoft.AspNetCore.Http;

namespace FizzBuzz.Api.Middleware;

public sealed class ExceptionResponseMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionResponseMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        HttpStatusCode exceptionStatusCode;
        string exceptionMessage;

        try
        {
            await _next(context);
            return;
        }
        catch (FizzBuzzException ex)
        {
            exceptionMessage = ex.Message;
            exceptionStatusCode = ex.StatusCode;
        }
        catch (Exception ex) when (ex is InvalidOperationException or BadHttpRequestException)
        {
            exceptionMessage = ex.Message;
            exceptionStatusCode = HttpStatusCode.BadRequest;
        }
        catch (Exception ex)
        {
            exceptionMessage = ex.Message;
            exceptionStatusCode = HttpStatusCode.InternalServerError;
        }

        var response = new FizzBuzzErrorResponse(exceptionStatusCode, exceptionMessage);

        context.Response.StatusCode = (int)exceptionStatusCode;
        await context.Response.WriteAsJsonAsync(response, typeof(FizzBuzzErrorResponse));
    }
}