using Microsoft.Extensions.Caching.Memory;

namespace MovieStore.Service.Caching
{
    public class CustomMemoryCache
    {
        public MemoryCache Cache { get; } = new MemoryCache(
        new MemoryCacheOptions
        {
            SizeLimit = 1024
        });
    }
}
