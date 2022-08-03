using System.Data;

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
