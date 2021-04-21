using HerosApi.Configuration;
using HerosApi.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HerosApi.Services
{
    public class HeroService
    {
        private readonly IMongoCollection<Hero> _heros;

        public HeroService(IDatabaseConfiguration dbConfig)
        {
            var client = new MongoClient(dbConfig.ConnectionString);
            var db = client.GetDatabase(dbConfig.DatabaseName);

            _heros = db.GetCollection<Hero>(dbConfig.HerosCollectionName);
        }

        public Task<List<Hero>> Get()
            => _heros.Find(Hero => true).ToListAsync();

        public Task<Hero> Get(string id) =>
            _heros.Find(Hero => Hero.Id == id).FirstOrDefaultAsync();

        public async Task<Hero> Create(Hero Hero)
        {
            await _heros.InsertOneAsync(Hero);
            return Hero;
        }

        public Task Update(string id, Hero HeroIn)
            => _heros.ReplaceOneAsync(Hero => Hero.Id == id, HeroIn);

        public Task Remove(Hero HeroIn)
            => _heros.DeleteOneAsync(Hero => Hero.Id == HeroIn.Id);

        public Task Remove(string id)
            => _heros.DeleteOneAsync(Hero => Hero.Id == id);
    }
}
