using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata_Invoicing.Infrastructure.DomainBase
{
    public interface IEntity
    {
        int Key { get; }
    }
}
