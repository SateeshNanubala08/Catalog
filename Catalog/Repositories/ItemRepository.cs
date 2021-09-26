using Catalog.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Repositories
{
    public class ItemRepository
    {
        private const string collectionName = "items";

        private readonly IMongoCollection<Items> dbCollection;

        private readonly FilterDefinitionBuilder<Items> filterBuilder = Builders<Items>.Filter;




        public ItemRepository()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("Catalog");
            dbCollection = database.GetCollection<Items>(collectionName); 
        }



        public async Task<IReadOnlyCollection<Items>> GetItemsAsync()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync(); 
        }

        public async Task<Items> GetAsync(int id)
        {
            FilterDefinition<Items> filter = filterBuilder.Eq(entity => entity.CatId, id);

            return await dbCollection.Find(filter).FirstOrDefaultAsync();
                
               
        }


        public async Task CreateAsync(Items entity)
        {
            if (entity == null)
            {
                throw new  ArgumentNullException(nameof(entity));
            }
             

              await dbCollection.InsertOneAsync(entity); 
        }


        public async Task UpdateAsync(Items entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }


            FilterDefinition<Items> filter = filterBuilder.Eq(existineEntity => existineEntity.CatId, entity.CatId);

            await dbCollection.ReplaceOneAsync(filter,entity);
        }


        public async Task RemoveAsync(int id)
        {
            FilterDefinition<Items> filter = filterBuilder.Eq(entity => entity.CatId, id);
            await dbCollection.DeleteOneAsync(filter);
        }
    }
}
