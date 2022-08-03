using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace Kata_Invoicing.Infrastructure.DomainBase
{
    /// <summary>
    /// All classes from domain model will inherit from this class.
    /// We added this class here because it's not a part from domain logic but provides necessary functionality to the domain model
    /// </summary>
    [Serializable]
    public abstract class EntityBase : IEntity
    {
        private int key = 0;
        private bool toBeDeleted = false;
        private List<BrokenRule> brokenRules;
        private BrokenRuleMessages brokenRuleMessages;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        protected EntityBase()
            : this(0)
        {
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="key">An <see cref="object"/> that 
        /// represents the primary identifier value for the 
        /// class.</param>
        protected EntityBase(int key)
        {
            this.key = key;

            this.brokenRules = new List<BrokenRule>();
            this.brokenRuleMessages = this.GetBrokenRuleMessages();
        }

        /// <summary>
        /// An <see cref="object"/> that represents the 
        /// primary identifier value for the class.
        /// </summary>
        public int Key
        {
            get => this.key;
            set => this.key = value;

        }

        public static object NewKey()
        {
            return Guid.NewGuid();
        }

        public bool ToBeDeleted
        {
            get => this.toBeDeleted;
            set => this.toBeDeleted = value;

        }

        #region Validation and Broken Rules

        public abstract void Validate();
        protected abstract BrokenRuleMessages GetBrokenRuleMessages();

        public List<BrokenRule> BrokenRules
        {
            get => this.brokenRules;
        }

        public ReadOnlyCollection<BrokenRule> GetBrokenRules()
        {
            this.brokenRules.Clear();
            this.Validate();
            return this.brokenRules.AsReadOnly();
        }
        #endregion

        #region Equality Tests

        /// <summary>
        /// Determines whether the specified entity is equal to the 
        /// current instance.
        /// </summary>
        /// <param name="entity">An <see cref="object"/> that 
        /// will be compared to the current instance.</param>
        /// <returns>True if the passed in entity is equal to the 
        /// current instance.</returns>
        public override bool Equals(object entity)
        {
            return entity != null
                && entity is EntityBase
                && this == (EntityBase)entity;
        }

        /// <summary>
        /// Operator overload for determining equality.
        /// </summary>
        /// <param name="base1">The first instance of an 
        /// <see cref="EntityBase"/>.</param>
        /// <param name="base2">The second instance of an 
        /// <see cref="EntityBase"/>.</param>
        /// <returns>True if equal.</returns>
        public static bool operator ==(EntityBase base1,
            EntityBase base2)
        {
            // check for both null (cast to object or recursive loop)
            if ((object)base1 == null && (object)base2 == null)
            {
                return true;
            }

            // check for either of them == to null
            if ((object)base1 == null || (object)base2 == null)
            {
                return false;
            }

            if (base1.Key != base2.Key)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Operator overload for determining inequality.
        /// </summary>
        /// <param name="base1">The first instance of an 
        /// <see cref="EntityBase"/>.</param>
        /// <param name="base2">The second instance of an 
        /// <see cref="EntityBase"/>.</param>
        /// <returns>True if not equal.</returns>
        public static bool operator !=(EntityBase base1,
            EntityBase base2)
        {
            return (!(base1 == base2));
        }

        /// <summary>
        /// Serves as a hash function for this type.
        /// </summary>
        /// <returns>A hash code for the current Key 
        /// property.</returns>
        public override int GetHashCode()
        {
            return this.key.GetHashCode();
        }

        #endregion

        public static string GenerateKey(object sourceObject)
        {
            string hashString;

            //Catch unuseful parameter values
            if (sourceObject == null)
            {
                throw new ArgumentNullException("Null as parameter is not allowed");
            }
            else
            {
                //We determine if the passed object is really serializable.
                try
                {
                    //Now we begin to do the real work.
                    hashString = ComputeHash(ObjectToByteArray(sourceObject));
                    return hashString;
                }
                catch (Exception ame)
                {
                    Console.WriteLine("Could not definitely decide if object is serializable. Message:" + ame.Message);
                    return string.Empty;
                }
            }
        }

        public static string ComputeHash(byte[] objectAsBytes)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            try
            {
                byte[] result = md5.ComputeHash(objectAsBytes);

                // Build the final string by converting each byte
                // into hex and appending it to a StringBuilder
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < result.Length; i++)
                {
                    sb.Append(result[i].ToString("X2"));
                }

                // And return it
                return sb.ToString();
            }
            catch (ArgumentNullException _ane)
            {
                //If something occurred during serialization, 
                //this method is called with a null argument. 
                Console.WriteLine( "Hash has not been generated." + _ane.Message);
                return string.Empty;
            }
        }

        private static readonly object locker = new object();

        public static byte[] ObjectToByteArray(object objectToSerialize)
        {
            MemoryStream fs = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                //Here's the core functionality! One Line!
                //To be thread-safe we lock the object
                if (objectToSerialize != null)
                {
                    lock (locker)
                    {
                        formatter.Serialize(fs, objectToSerialize);
                    }
                }

                return fs.ToArray();
            }
            catch (SerializationException se)
            {
                Console.WriteLine("Error occurred during serialization. Message: " +
                se.Message);
                return Array.Empty<byte>(); 
            }
            finally
            {
                fs.Close();
            }
        }
    }
}
