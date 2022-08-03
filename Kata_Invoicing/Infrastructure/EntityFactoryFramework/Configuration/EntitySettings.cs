using System.Configuration;

namespace Kata_Invoicing.Infrastructure.EntityFactoryFramework.Configuration
{
    public class EntitySettings : ConfigurationSection
    {
        [ConfigurationProperty(EntityMappingConstants.ConfigurationPropertyName,
            IsDefaultCollection = true)]
        public EntityMappingCollection EntityMappings
        {
            get { return (EntityMappingCollection)base[EntityMappingConstants.ConfigurationPropertyName]; }
        }
    }
}
