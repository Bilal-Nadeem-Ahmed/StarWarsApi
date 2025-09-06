using Microsoft.AspNetCore.Mvc;
using MyStarwarsApi.Services.Interfaces;

namespace MyStarwarsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CharacterController : ControllerBase
    {
        private ISwapiService _swapiService;
        private readonly ILogger<CharacterController> _logger;

        public CharacterController(ILogger<CharacterController> logger, ISwapiService swapiService)
        {
            _logger = logger;
            _swapiService = swapiService;
        }

        [HttpGet]
        [Route("GetAllCharacters")]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any, NoStore = false)]
        public async Task<IActionResult> GetAllCharacters()
        {
            var results = await _swapiService.GetAllCharacters();
            return Ok(results);
        }
    }
}
