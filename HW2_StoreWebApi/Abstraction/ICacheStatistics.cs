using Microsoft.Extensions.Caching.Memory;

namespace HW2_StoreWebApi.Abstraction;

public interface ICacheStatistics
{
    public MemoryCacheStatistics? GetMemoryCacheStatistics();
    public string GetMemoryCacheStatisticsCsv();
}