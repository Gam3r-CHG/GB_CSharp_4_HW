using System.Text;
using HW2_StoreWebApi.Abstraction;
using HW2_StoreWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace HW2_StoreWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class StatController : ControllerBase
{
    private readonly ICacheStatistics _statistics;

    public StatController(ICacheStatistics statistics)
    {
        _statistics = statistics;
    }

    [HttpGet("get_cache_statistics")]
    public ActionResult<MemoryCacheStatistics> GetCacheStatistics()
    {
        var cacheStatistics = _statistics.GetMemoryCacheStatistics();
        return Ok(cacheStatistics);
    }

    [HttpGet("get_cache_statistics_csv")]
    public ActionResult<string> GetCacheStatisticsCsv()
    {
        var cacheStatisticsCsv = _statistics.GetMemoryCacheStatisticsCsv();
        return Ok(cacheStatisticsCsv);
    }

    [HttpGet("get_cache_statistics_csv_url")]
    public ActionResult<string> GetCacheStatisticsCsvUrl()
    {
        var cacheStatisticsCsv = _statistics.GetMemoryCacheStatisticsCsv();

        string fileName = "cache-statistics" + DateTime.Now.ToBinary() + ".csv";

        System.IO.File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles", fileName),
            cacheStatisticsCsv);
        return "http://" + Request.Host + "/static/" + fileName;
    }

    [HttpGet("get_cache_statistics_csv_file")]
    public FileContentResult GetCacheStatisticsCsvFile()
    {
        var cacheStatisticsCsv = _statistics.GetMemoryCacheStatisticsCsv();
        string fileName = "cache-statistics" + DateTime.Now.ToBinary() + ".csv";
        return File(new UTF8Encoding().GetBytes(cacheStatisticsCsv), "text/csv", fileName);
    }
}