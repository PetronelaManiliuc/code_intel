using Kata_Invoicing.Infrastructure.DomainBase;

namespace Kata_Invoicing.Infrastructure.RepositoryFramework
{
    public interface IUnitOfWorkRepository
    {
        bool PersistNewItem(IEntity item);
        bool PersistUpdatedItem(IEntity item);
        bool PersistDeletedItem(IEntity item);
    }
}
