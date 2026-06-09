// Services/CacheService.cs
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using RemoteJobBoard.Core.Interfaces;

namespace RemoteJobBoard.Infrastructure.Services;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;

    public CacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var json = await _cache.GetStringAsync(key);
        return json is null ? default : JsonSerializer.Deserialize<T>(json);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expiry)
    {
        var json = JsonSerializer.Serialize(value);
        await _cache.SetStringAsync(key, json, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiry
        });
    }

    public async Task RemoveAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }

    public async Task RemoveByPrefixAsync(string prefix)
    {
        // For Redis — we track keys by prefix convention
        // Full implementation uses IConnectionMultiplexer in Phase 5
        await _cache.RemoveAsync(prefix);
    }
}