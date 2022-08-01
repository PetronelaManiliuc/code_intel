using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata_Invoicing.Infrastructure.PersistanceManager
{
    public interface IItem
    {
        /// <summary>
        /// Create IItem from IDataReader
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        IItem CreateFromReader(IDataReader dataReader);
    }
}
