using System.Configuration;

namespace Kata_Invoicing.Infrastructure.RepositoryFramework.Configuration
{
    public sealed class RepositoryMappingCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new RepositoryMappingElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RepositoryMappingElement)element).InterfaceShortTypeName;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get => ConfigurationElementCollectionType.BasicMap;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override string ElementName
        {
            get => RepositoryMappingConstants.ConfigurationElementName;
        }

        public RepositoryMappingElement this[int index]
        {
            get => (RepositoryMappingElement)this.BaseGet(index);
            set
            {
                if (this.BaseGet(index) != null)
                {
                    this.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interfaceShortTypeName"></param>
        /// <returns></returns>
        public new RepositoryMappingElement this[string interfaceShortTypeName]
        {
            get => (RepositoryMappingElement)this.BaseGet(interfaceShortTypeName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public bool ContainsKey(string keyName)
        {
            bool result = false;
            object[] keys = this.BaseGetAllKeys();
            foreach (object key in keys)
            {
                if ((string)key == keyName)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}
