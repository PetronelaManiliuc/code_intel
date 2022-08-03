using System.Collections.Generic;

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
