using Kata_Invoicing.Infrastructure.DomainBase;
using Kata_Invoicing.Infrastructure.RepositoryFramework;

namespace Kata_Invoicing.Infrastructure
{
    public interface IUnitOfWork
    {
        void RegisterAdded(IEntity entity, IUnitOfWorkRepository repository);
        void RegisterChanged(IEntity entity, IUnitOfWorkRepository repository);
        void RegisterRemoved(IEntity entity, IUnitOfWorkRepository repository);
        bool Commit();
        void DisposeOperations();
        object Key { get; }
    }
}
