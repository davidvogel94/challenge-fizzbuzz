namespace FizzBuzz.Data.Services;

using System;
using System.Threading.Tasks;

using MongoDB.Entities;

using FizzBuzz.Common.Exceptions;
using FizzBuzz.Common.Models;


public interface IFizzBuzzDataService
{
    Task<FizzBuzzGameData> GetAsync(string gameId);
    Task SaveAsync(FizzBuzzGameData data);
}


public sealed class FizzBuzzDataService : IFizzBuzzDataService
{
    private readonly DBContext _context;

    public FizzBuzzDataService(DBContext context)
    {
        this._context = context;
    }

    public async Task<FizzBuzzGameData> GetAsync(string gameId)
    {
        return await _context
            .Find<FizzBuzzGameData>()
            .Match(data => data.ID.Equals(gameId))
            .ExecuteFirstAsync()
            ?? throw new FizzBuzzGameNotFoundException(gameId);
    }

    public async Task SaveAsync(FizzBuzzGameData data)
    {   
        await _context.SaveAsync(data);
    }
}