using System.Collections.Generic;
using System.Threading.Tasks;
using DeliveryOrderProcessor.MongoDb.Collections;
using DeliveryOrderProcessor.MongoDb.Context;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DeliveryOrderProcessor.MongoDb.Repositories
{
    /// <summary>
    /// Default implementation of <see cref="IMongoDbRepository{TDocument}"/>.
    /// </summary>
    public class GenericMongoDbRepository<TDocument> : IMongoDbRepository<TDocument>
        where TDocument : IEntity
    {
        private readonly IMongoCollection<TDocument> collection;

        /// <summary>
        /// Instantiates a <see cref="GenericMongoDbRepository{TDocument}"/>.
        /// </summary>
        /// <param name="mongoDbContext">The <see cref="IMongoDbContext"/> to use.</param>
        /// <param name="collectionName"></param>
        public GenericMongoDbRepository(IMongoCollection<TDocument> collection)
        {
            this.collection = collection;
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<TDocument>> GetAllAsync() =>
            await collection
                .Find(_ => true)
                .ToListAsync();

        /// <inheritdoc />
        public virtual async Task<IEnumerable<TDocument>> GetByAsync(string propertyName, string stringToContain) =>
            await collection
                .Find(Builders<TDocument>.Filter.Regex(propertyName, new BsonRegularExpression($@".?{stringToContain}?.")))
                .ToListAsync();

        /// <inheritdoc />
        public virtual async Task<TDocument> GetById(string id) =>
            await collection
                .Find(x => x.Id == id)
                .FirstAsync();

        /// <inheritdoc />
        public virtual async Task<TDocument> CreateAsync(TDocument newDocument)
        {
            await collection
                .InsertOneAsync(newDocument);
            return newDocument;
        }

        /// <inheritdoc />
        public virtual async Task UpdateAsync(TDocument updatedDocument) =>
            await collection
                .ReplaceOneAsync(x => x.Id == updatedDocument.Id, updatedDocument);


        /// <inheritdoc />
        public virtual async Task<DeleteResult> RemoveAsync(TDocument documentToRemove)
        {
            return await collection
                .DeleteOneAsync(x => x.Id.Equals(documentToRemove.Id));
        }
    }
}
