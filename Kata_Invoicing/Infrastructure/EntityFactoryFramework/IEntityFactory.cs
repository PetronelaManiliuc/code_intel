using System.Data;
using Kata_Invoicing.Infrastructure.DomainBase;

namespace Kata_Invoicing.Infrastructure.EntityFactoryFramework
{
    public interface IEntityFactory<T> where T : IEntity
    {
        T BuildEntity(IDataReader reader);
    }
}
