using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kata_Invoicing.Infrastructure.DomainBase;

namespace Kata_Invoicing.Infrastructure.EntityFactoryFramework
{
    public interface IEntityFactory<T> where T : IEntity
    {
        T BuildEntity(IDataReader reader);
    }
}
