using System;
using System.Transactions;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
            
        private TransactionScope _transaction;

        public UnitOfWork()
        {
               
        }

        #region IUnitOfWork Members

        public bool IsInTransaction
        {
            get { return _transaction != null; }
        }

        public void BeginTransaction()
        {
            BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            if (_transaction != null)
            {
                throw new ApplicationException(
                    "Cannot begin a new transaction while an existing transaction is still running. " +
                    "Please commit or rollback the existing transaction before starting a new one.");
            }
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = isolationLevel;
            transactionOptions.Timeout = TransactionManager.MaximumTimeout;
        
            _transaction = new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled);

                    
        }

        public void RollBackTransaction()
        {
            if (_transaction == null)
            {
                throw new ApplicationException("Cannot roll back a transaction while there is no transaction running.");
            }

            try
            {
                _transaction.Dispose();
            }
            catch
            {
                throw;
            }
            finally
            {
                ReleaseCurrentTransaction();
            }
        }

        public void CommitTransaction()
        {
            if (_transaction == null)
            {
                throw new ApplicationException("Cannot commit back a transaction while there is no transaction running.");
            }

            try
            {
                _transaction.Complete();
            }
            catch
            {
                _transaction.Dispose();
                throw;
            }
            finally
            {
                ReleaseCurrentTransaction();
            }
        }

        
        #endregion

        /// <summary>
        /// Releases the current transaction
        /// </summary>
        private void ReleaseCurrentTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }


        #region Implementation of IDisposable

        private bool _disposed;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the managed and unmanaged resources.
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            if (_disposed)
                return;

            ReleaseCurrentTransaction();

            _disposed = true;
        }

        #endregion
    }
}