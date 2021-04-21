using HerosApi.Models;
using HerosApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HerosApi.Controllers
{
    [Route("api/[controller]")]
    public class HerosController : Controller
    {
        private readonly HeroService _heroService;

        public HerosController(HeroService HeroService)
        {
            _heroService = HeroService;
        }

        [HttpGet(Name = "GetHeros")]
        public async Task<IActionResult> Get()
            => Ok(await _heroService.Get());

        [HttpGet("{id:length(24)}", Name = "GetHero")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            var hero = await _heroService.Get(id);

            if (hero == null)
                return NotFound();

            return Ok(hero);
        }

        [HttpPost(Name = "CreateHero")]
        public async Task<IActionResult> Create([FromBody] Hero hero)
        {
            await _heroService.Create(hero);

            return CreatedAtRoute("GetHero", new { id = hero.Id.ToString() }, hero);
        }

        [HttpPut("{id:length(24)}", Name = "UpdateHero")]
        public IActionResult Update([FromRoute] string id, [FromBody] Hero heroIn)
        {
            var heroDb = _heroService.Get(id);

            if (heroDb == null)
                return NotFound();

            _heroService.Update(id, heroIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteHero")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var heroDb = await _heroService.Get(id);

            if (heroDb == null)
                return NotFound();

            await _heroService.Remove(heroDb);

            return NoContent();
        }
    }
}
