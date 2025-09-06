using MyStarwarsApi.Models.StarWarsApi.Models;

namespace MyStarwarsApi.Services.Interfaces
{
    public interface ISwapiService
    {
        Task<IEnumerable<Person>> GetAllCharacters();
    }
}
