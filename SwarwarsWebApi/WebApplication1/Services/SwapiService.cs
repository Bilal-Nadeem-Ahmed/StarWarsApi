using Microsoft.Extensions.Caching.Memory;
using MyStarwarsApi.Models.StarWarsApi.Models;
using MyStarwarsApi.Services.Interfaces;
using static MyStarwarsApi.Exceptions.Exceptions;

namespace MyStarwarsApi.Services
{
    public class SwapiService : ISwapiService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private const string PeopleCacheKey = "people";
        private readonly ILogger<SwapiService> _logger;
        public SwapiService(HttpClient httpClient, IMemoryCache memoryCache, ILogger<SwapiService> logger)
        {
            _httpClient = httpClient;
            _cache = memoryCache;
            _logger = logger;
        }
        public async Task<IEnumerable<Person>> GetAllCharacters()
        {
            if (_cache.TryGetValue(PeopleCacheKey, out List<Person> cachedPeople))
            {
                _logger.LogInformation("Returning from cache");
                return cachedPeople;
            }

            _logger.LogInformation("Fetching people from external api");

            var response = await _httpClient.GetFromJsonAsync<List<Person>>("people/");

            if (response == null || response.Count == 0)
            {
                throw new NotFoundException("No characters returned from SWAPI.");
            }

            var people = response;

            // should probably move this out for srp 
            _cache.Set(
                PeopleCacheKey,
                people,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1),
                    SlidingExpiration = TimeSpan.FromDays(1)
                });

            return people;
        }

    }
}
