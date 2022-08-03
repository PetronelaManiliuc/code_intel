using System.Data.SqlClient;

namespace Kata_Invoicing.Infrastructure.Repositories
{
    public interface ISqlRepositoryBase
    {

        SqlCommand AllEntitiesSqlCommand(string commandText);
        SqlCommand EntityByKeySqlCommand(int key);

        //
        //
        string GetEntityName();
        string GetKeyFieldName();
        void BuildChildCallbacks();

    }
}
