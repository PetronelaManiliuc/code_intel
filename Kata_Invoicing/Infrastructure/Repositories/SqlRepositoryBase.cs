using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Kata_Invoicing.Infrastructure.DomainBase;
using Kata_Invoicing.Infrastructure.EntityFactoryFramework;
using Kata_Invoicing.Infrastructure.RepositoryFramework;
using log4net;

namespace Kata_Invoicing.Infrastructure.Repositories
{
    public abstract class SqlRepositoryBase<T> : RepositoryBase<T>
      where T : IAggregateRoot
    {

        #region Fields and properties

        private static ILog _log = LogManager.GetLogger(typeof(SqlRepositoryBase<T>));

        #endregion Fields and properties

        #region AppendChildData Delegate

        /// <summary>
        /// The delegate signature required for callback methods
        /// </summary>
        /// <param name="entityAggregate"></param>
        /// <param name="childEntityKey"></param>
        public delegate void AppendChildData(T entityAggregate, object childEntityKeyValue);

        #endregion

        #region Private Fields


        private IEntityFactory<T> entityFactory;
        private Dictionary<string, AppendChildData> childCallbacks;
        private string entityName;
        private string keyFieldName;

        #endregion

        #region Constructors

        protected SqlRepositoryBase()
            : this(null)
        {
        }

        protected SqlRepositoryBase(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

            this.entityFactory = EntityFactoryBuilder.BuildFactory<T>();
            this.childCallbacks = new Dictionary<string, AppendChildData>();
            this.BuildChildCallbacks();
            this.entityName = this.GetEntityName();
            this.keyFieldName = this.GetKeyFieldName();
        }

        #endregion

        #region Abstract Methods

        protected abstract SqlCommand AllEntitiesSqlCommand(string commandText);
        protected abstract SqlCommand EntityByKeySqlCommand(int key);

        //
        //
        protected abstract string GetEntityName();
        protected abstract string GetKeyFieldName();
        protected abstract void BuildChildCallbacks();
        protected abstract override bool PersistNewItem(T item);
        protected abstract override bool PersistUpdatedItem(T item);
        protected abstract override bool PersistDeletedItem(T item);

        #endregion

        #region Properties

        protected Dictionary<string, AppendChildData> ChildCallbacks
        {
            get { return this.childCallbacks; }
        }

        protected string EntityName
        {
            get { return this.entityName; }
        }

        protected string KeyFieldName
        {
            get { return this.keyFieldName; }
        }

        #endregion

        #region Public Methods

        public override IList<T> FindAll(bool thinVersion = true)
        {
            return this.BuildEntitiesFromSqlCommand(this.AllEntitiesSqlCommand("InvoiceGetAll"), thinVersion);
        }

        public IList<T> FindAll(SqlCommand command, bool thinVersion = true)
        {
            return this.BuildEntitiesFromSqlCommand(command, thinVersion);
        }

        public override T FindBy(int key, bool thinVersion = false)
        {
            return this.BuildEntityFromSqlCommand(this.EntityByKeySqlCommand(key), thinVersion);
        }

        #endregion


        #region Protected Methods

        protected int ExecuteSqlCommand(SqlCommand command)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    command.Connection = connection;
                    using (command)
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        return (int)command.Parameters["@ID"].Value;
                    }
                }
            }
            catch (Exception exception)
            {
                _log.Error(exception);
                return -1;
            }
        }

        protected virtual T BuildEntityFromSqlCommand(SqlCommand command, bool thinVersion = false)
        {
            T entity = default(T);

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    command.Connection = connection;
                    using (command)
                    {
                        connection.Open();

                        using (IDataReader dataReader = command.ExecuteReader())
                        {
                            if (dataReader.Read())
                            {
                                entity = this.BuildEntityFromReader(dataReader, thinVersion);
                            }
                        }

                    }
                }
            }
            catch (Exception exception)
            {
                _log.Error(exception);
            }

            return entity;
        }

        protected virtual T BuildEntityFromReader(IDataReader reader, bool thinVersion)
        {
            T entity = this.entityFactory.BuildEntity(reader);
            if (thinVersion)
                return entity;

            if (this.childCallbacks != null && this.childCallbacks.Count > 0)
            {
                object childKeyValue = null;
                DataTable columnData = reader.GetSchemaTable();

                foreach (string childKeyName in this.childCallbacks.Keys)
                {
                    if (DataHelper.ReaderContainsColumnName(columnData, childKeyName))
                        childKeyValue = reader[childKeyName];
                    else
                        childKeyValue = null;

                    this.childCallbacks[childKeyName](entity, childKeyValue);
                }
            }

            return entity;
        }

        protected IDataReader ExecuteReader(SqlCommand command)
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConnectionString);

                connection.Open();
                command.Connection = connection;
                // When using CommandBehavior.CloseConnection, the connection will be closed when the 
                // IDataReader is closed.
                SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return null;
            }

        }

        protected virtual List<T> BuildEntitiesFromSqlCommand(SqlCommand command, bool thinVersion = true)
        {
            List<T> entities = new List<T>();
            try
            {
                using (IDataReader reader = this.ExecuteReader(command))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            entities.Add(this.BuildEntityFromReader(reader, thinVersion));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }

            return entities;
        }

        #endregion

        #region Private Helper Methods

        #endregion
    }
}
