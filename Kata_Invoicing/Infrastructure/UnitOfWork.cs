using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Kata_Invoicing.Infrastructure.DomainBase;
using Kata_Invoicing.Infrastructure.RepositoryFramework;
using Kata_Invoicing.Infrastructure.Transactions;

namespace Kata_Invoicing.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields

        private Guid key;
        private List<Operation> operations;

        #endregion

        #region Constructors

        public UnitOfWork()
        {
            this.key = Guid.NewGuid();
            this.operations = new List<Operation>();
        }

        #endregion

        #region Operation

        /// <summary>
        /// Provides a snapshot of an entity and the repository reference it belongs to.
        /// </summary>
        private sealed class Operation
        {
            /// <summary>
            /// Gets or sets the entity.
            /// </summary>
            /// <value>The entity.</value>
            public IEntity Entity { get; set; }

            /// <summary>
            /// Gets or sets the process date.
            /// </summary>
            /// <value>The process date.</value>
            public DateTime ProcessDate { get; set; }

            /// <summary>
            /// Gets or sets the repository.
            /// </summary>
            /// <value>The repository.</value>
            public IUnitOfWorkRepository Repository { get; set; }

            /// <summary>
            /// Gets or sets the type of operation.
            /// </summary>
            /// <value>The type of operation.</value>
            public TransactionType Type { get; set; }
        }

        #endregion

        #region IUnitOfWork Members

        /// <summary>
        /// Registers an <see cref="IEntity" /> instance to be added through this <see cref="UnitOfWork" />.
        /// </summary>
        /// <param name="entity">The <see cref="IEntity" />.</param>
        /// <param name="repository">The <see cref="IUnitOfWorkRepository" /> participating in the transaction.</param>
        public void RegisterAdded(IEntity entity,
            IUnitOfWorkRepository repository)
        {
            this.operations.Add(
                new Operation
                {
                    Entity = entity,
                    ProcessDate = DateTime.Now,
                    Repository = repository,
                    Type = TransactionType.Insert
                });

        }

        /// <summary>
        /// Registers an <see cref="IEntity" /> instance to be changed through this <see cref="UnitOfWork" />.
        /// </summary>
        /// <param name="entity">The <see cref="IEntity" />.</param>
        /// <param name="repository">The <see cref="IUnitOfWorkRepository" /> participating in the transaction.</param>
        public void RegisterChanged(IEntity entity,
            IUnitOfWorkRepository repository)
        {
            this.operations.Add(
                new Operation
                {
                    Entity = entity,
                    ProcessDate = DateTime.Now,
                    Repository = repository,
                    Type = TransactionType.Update
                });
        }

        /// <summary>
        /// Registers an <see cref="IEntity" /> instance to be removed through this <see cref="UnitOfWork" />.
        /// </summary>
        /// <param name="entity">The <see cref="IEntity" />.</param>
        /// <param name="repository">The <see cref="IUnitOfWorkRepository" /> participating in the transaction.</param>
        public void RegisterRemoved(IEntity entity,
            IUnitOfWorkRepository repository)
        {
            this.operations.Add(
                new Operation
                {
                    Entity = entity,
                    ProcessDate = DateTime.Now,
                    Repository = repository,
                    Type = TransactionType.Delete
                });
        }

        /// <summary>
        /// Commits all batched changes within the scope of a <see cref="TransactionScope" />.
        /// </summary>
        public bool Commit()
        {
            bool IsExecutedWitSuccess = false;
            try
            {
                using (var scope = new TransactionScope())
                {
                    foreach (var operation in this.operations.OrderBy(o => o.ProcessDate))
                    {
                        switch (operation.Type)
                        {
                            case TransactionType.Insert:
                                IsExecutedWitSuccess = operation.Repository.PersistNewItem(operation.Entity);
                                break;

                            case TransactionType.Update:
                                IsExecutedWitSuccess = operation.Repository.PersistUpdatedItem(operation.Entity);
                                break;

                            case TransactionType.Delete:
                                IsExecutedWitSuccess = operation.Repository.PersistDeletedItem(operation.Entity);
                                break;
                        }

                        //if one operation failed then abort the others
                        if (!IsExecutedWitSuccess)
                            break;
                    }

                    // Commit the transaction if all operations succeded
                    if (IsExecutedWitSuccess)
                        scope.Complete();
                    else
                        scope.Dispose();
                }

                DisposeOperations();

                return IsExecutedWitSuccess;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void DisposeOperations()
        {
            // Clear everything
            this.operations.Clear();
            this.key = Guid.NewGuid();
        }

        public object Key
        {
            get => this.key;
        }


        #endregion
    }
}
