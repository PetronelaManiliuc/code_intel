using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata_Invoicing.Infrastructure.EntityFactoryFramework.Configuration
{
    public sealed class EntityMappingElement : ConfigurationElement
    {
        [ConfigurationProperty(EntityMappingConstants.EntityShortTypeNameAttributeName,
            IsKey = true, IsRequired = true)]
        public string EntityShortTypeName
        {
            get
            {
                return (string)this[EntityMappingConstants.EntityShortTypeNameAttributeName];
            }
            set
            {
                this[EntityMappingConstants.EntityShortTypeNameAttributeName] = value;
            }
        }

        [ConfigurationProperty(EntityMappingConstants.EntityFactoryFullTypeNameAttributeName,
            IsRequired = true)]
        public string EntityFactoryFullTypeName
        {
            get
            {
                return (string)this[EntityMappingConstants.EntityFactoryFullTypeNameAttributeName];
            }
            set
            {
                this[EntityMappingConstants.EntityFactoryFullTypeNameAttributeName] = value;
            }
        }
    }
}
