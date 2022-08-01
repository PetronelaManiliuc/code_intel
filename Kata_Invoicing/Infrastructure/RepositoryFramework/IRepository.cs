using System.Collections.Generic;
using Kata_Invoicing.Infrastructure.DomainBase;

namespace Kata_Invoicing.Infrastructure.RepositoryFramework
{
    /// <summary>
    /// Generic Repository interface.
    ///  We do not put a   Find  or   FindBy  method on this interface that takes some type of generic predicate or expression - can be complicated.
    /// We'll put these in the Aggregate-specific types of repositories.
    /// </summary>
    /// <typeparam name="T">any class derived from EntityBase abstract class</typeparam>
    public interface IRepository<T> where T : IAggregateRoot
    {
        /// <summary>
        /// Find object by its identifier - key. If returned object should not run all it's callbacks thinVersion should be false
        /// </summary>
        /// <param name="key"></param>
        /// <param name="thinVersion"></param>
        /// <returns></returns>
        T FindBy(int key, bool thinVersion = false);

        /// <summary>
        /// Find all 
        /// </summary>
        /// <returns></returns>
        IList<T> FindAll(bool thinVersion = true);

        /// <summary>
        /// Add new object of type T
        /// </summary>
        /// <param name="item">item of type T</param>
        void Add(T item);

        /// <summary>
        /// Indexer - repository should emulate a collection of objects in memory
        /// </summary>
        /// <param name="key">identifier</param>
        /// <returns></returns>
        T this[int key] { get; set; }

        /// <summary>
        /// Force call Commit() method of unitOfWork field for cases where we need an immediate execution of operations in queue
        /// </summary>
        bool CommitUnitOfWork();

        /// <summary>
        /// Remove object
        /// </summary>
        /// <param name="item">item of type T</param>
        void Remove(T item);
    }
}
