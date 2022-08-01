using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata_Invoicing.Infrastructure.PersistanceManager
{
    public interface IBulkInsert
    {
        /// <summary>
        /// Gets TableColumsMappings for BulkInsert (key = ObjPropertyName, value = TableColumnName)
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> GetTableColumnMappings();
    }
}
