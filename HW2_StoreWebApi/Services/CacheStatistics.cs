using System.Text;
using HW2_StoreWebApi.Abstraction;
using Microsoft.Extensions.Caching.Memory;

namespace HW2_StoreWebApi.Services;

public class CacheStatistics : ICacheStatistics
{
    private readonly IMemoryCache _cache;

    public CacheStatistics(IMemoryCache cache)
    {
        _cache = cache;
    }

    public MemoryCacheStatistics? GetMemoryCacheStatistics()
    {
        return _cache.GetCurrentStatistics();
    }

    public string GetMemoryCacheStatisticsCsv()
    {
        var stats = GetMemoryCacheStatistics();
        var sb = new StringBuilder();
        sb.AppendLine("CurrentEntryCount;CurrentEstimatedSize;TotalMisses;TotalHits");
        if (stats != null)
        {
            sb.AppendLine(
                $"{stats.CurrentEntryCount};{stats.CurrentEstimatedSize};{stats.TotalMisses};{stats.TotalHits}");
        }

        return sb.ToString();
    }
}