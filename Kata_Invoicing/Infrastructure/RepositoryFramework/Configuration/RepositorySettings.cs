using System.Configuration;

namespace Kata_Invoicing.Infrastructure.RepositoryFramework.Configuration
{
    public class RepositorySettings : ConfigurationSection
    {

        [ConfigurationProperty(RepositoryMappingConstants.ConfigurationPropertyName,
            IsDefaultCollection = true)]
        public RepositoryMappingCollection RepositoryMappings
        {
            get => (RepositoryMappingCollection)base[RepositoryMappingConstants.ConfigurationPropertyName];
        }
    }
}
