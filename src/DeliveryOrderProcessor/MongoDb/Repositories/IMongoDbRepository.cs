using System.Collections.Generic;
using System.Threading.Tasks;
using DeliveryOrderProcessor.MongoDb.Collections;
using MongoDB.Driver;

namespace DeliveryOrderProcessor.MongoDb.Repositories;

/// <summary>
/// Interface for MongoDB service.
/// </summary>
/// <typeparam name="TDocument">The type of the document to return.</typeparam>
public interface IMongoDbRepository<TDocument>
    where TDocument : IEntity
{
    /// <summary>
    /// Gets all values of a collection. Awaitable.
    /// </summary>
    /// <returns>Returns a task that evaluates to a collection of <typeparamref name="TDocument"/> when finished.</returns>
    Task<IEnumerable<TDocument>> GetAllAsync();

    /// <summary>
    /// Gets all values of a collection where property with name <paramref name="propertyName"/> contains <paramref name="stringToContain"/>.
    /// </summary>
    /// <param name="propertyName">The name of the property to filter on.</param>
    /// <param name="stringToContain">The string to contain.</param>
    /// <returns>Returns a task that evaluates to a collection of <typeparamref name="TDocument"/> when finished.</returns>
    Task<IEnumerable<TDocument>> GetByAsync(string propertyName, string stringToContain);

    /// <summary>
    /// Gets all values of a collection where the id is equal to <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The id to search for.</param>
    /// <returns>Returns a task that evaluates to a collection of <typeparamref name="TDocument"/> when finished.</returns>
    Task<TDocument> GetById(string id);

    /// <summary>
    /// Adds the given <paramref name="newDocument"/> to the collection. Awaitable.
    /// </summary>
    /// <param name="newDocument">The <typeparamref name="TDocument"/> to add to the collection.</param>
    /// <returns>Returns a <see cref="Task"/> that will evaluates to the <see cref="TDocument"/> that was added to the collection.</returns>
    Task<TDocument> CreateAsync(TDocument newDocument);

    /// <summary>
    /// Updates the <typeparamref name="TDocument"/> with the given <paramref name="updatedDocument"/>.
    /// </summary>
    /// <param name="updatedDocument">The updated value of the <typeparamref name="TDocument"/>.</param>
    /// <returns>Returns a <see cref="Task"/> that will updates the <typeparamref name="TDocument"/>.</returns>
    Task UpdateAsync(TDocument updatedDocument);

    /// <summary>
    /// Removes the <typeparamref name="TDocument"/> with the given <paramref name="id"/> from the collection.
    /// </summary>
    /// <param name="documentToRemove">The document to remove.</param>
    /// <returns>Returns a <see cref="Task"/> that will remove the <typeparamref name="TDocument"/> from the collection.</returns>
    Task<DeleteResult> RemoveAsync(TDocument documentToRemove);
}
