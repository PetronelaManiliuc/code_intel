using System.Collections.Generic;
using System.Configuration;
using Kata_Invoicing.Infrastructure.DomainBase;

namespace Kata_Invoicing.Infrastructure.RepositoryFramework
{
    public abstract class RepositoryBase<T>
       : IRepository<T>, IUnitOfWorkRepository where T : IAggregateRoot
    {
        #region Fields and properties

        private IUnitOfWork unitOfWork;

        private static string _connectionString;

        /// <summary>
        /// The connection string
        /// </summary>
        protected internal static string ConnectionString
        {
            get => _connectionString;
        }

        #endregion Fields and properties

        #region Constructors

        protected RepositoryBase()
            : this(null)
        {

            if (ConfigurationManager.AppSettings[FieldNames.ConnectionString] != null)
            {
                _connectionString = ConfigurationManager.AppSettings[FieldNames.ConnectionString];
            }
        }

        protected RepositoryBase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            if (ConfigurationManager.AppSettings[FieldNames.ConnectionString] != null)
            {
                _connectionString = ConfigurationManager.AppSettings[FieldNames.ConnectionString]; ;
            }
        }

        #endregion

        #region IRepository<T> Members

        public abstract T FindBy(int key, bool thinVersion = false);

        public abstract IList<T> FindAll(bool thinVersion = true);

        public void Add(T item)
        {
            if (this.unitOfWork != null)
                this.unitOfWork.RegisterAdded(item, this);
        }

        public void Remove(T item)
        {
            if (this.unitOfWork != null)
                this.unitOfWork.RegisterRemoved(item, this);
        }

        public T this[int key]
        {
            get => this.FindBy(key);

            set
            {
                if (key == 0
                 || this.FindBy(key) == null)
                    this.Add(value);

                else
                {
                    if (this.unitOfWork != null)
                        this.unitOfWork.RegisterChanged(value, this);
                }
            }
        }

        public bool CommitUnitOfWork()
        {
            return this.unitOfWork.Commit();
        }

        #endregion

        #region IUnitOfWorkRepository Members

        public virtual bool PersistNewItem(IEntity item)
        {
            return this.PersistNewItem((T)item);
        }

        public virtual bool PersistUpdatedItem(IEntity item)
        {
            return this.PersistUpdatedItem((T)item);
        }

        public virtual bool PersistDeletedItem(IEntity item)
        {
            return this.PersistDeletedItem((T)item);
        }

        #endregion

        #region Properties

        protected IUnitOfWork UnitOfWork
        {
            get { return this.unitOfWork; }
        }

        #endregion

        #region Abstract methods

        protected abstract bool PersistNewItem(T item);
        protected abstract bool PersistUpdatedItem(T item);
        protected abstract bool PersistDeletedItem(T item);

        #endregion
    }
}
