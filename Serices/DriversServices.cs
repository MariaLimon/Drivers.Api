using MongoDB.Driver;
using Drivers.Api.Models;
using Drivers.Api.Configurations;
using Microsoft.Extensions.Options;
using ZstdSharp.Unsafe;
using MongoDB.Bson;

using MongoDB.Bson.Serialization;
using System.Diagnostics;


namespace Drivers.Api.Services
{
    public class DriverServices
    {
        private readonly IMongoCollection<Drivers.Api.Models.Driver> _driverCollection;
        
        public DriverServices(IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var mongoDB =
            mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
                _driverCollection =
                    mongoDB.GetCollection<Drivers.Api.Models.Driver>(databaseSettings.Value.CollectionName);
        }
        public async Task<List<Drivers.Api.Models.Driver>> GetAsync() =>
            await _driverCollection.Find(_ => true).ToListAsync();

        public async Task InsertDriver(Driver driverInsert)
        {
            await _driverCollection.InsertOneAsync(driverInsert);
        }

        public async Task DeleteDriver(string driverId)
        {
            var filter = Builders<Driver>.Filter.Eq(s=>s.Id, driverId);
            await _driverCollection.DeleteOneAsync(filter);
        }

        public async Task UpdateDriver(Driver dataToUpdate)
        {
            var filter = Builders<Driver>.Filter.Eq(s=>s.Id, dataToUpdate.Id);
            await _driverCollection.ReplaceOneAsync(filter,dataToUpdate);
        }

        public async Task<Driver> GetDriverById(string idToSearch)
        {
            return await _driverCollection.FindAsync(new BsonDocument{{"_id", new ObjectId(idToSearch)}}).Result.FirstAsync();    
        }
    }
}