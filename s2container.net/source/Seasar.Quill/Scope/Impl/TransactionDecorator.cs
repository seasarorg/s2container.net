using System;
using System.Data;
using Quill.Exception;
using Quill.Message;
using QM = Quill.QuillManager;

namespace Quill.Scope.Impl {
    /// <summary>
    /// トランザクション修飾クラス
    /// </summary>
    public class TransactionDecorator : IQuillDecorator<IDbConnection> {
        /// <summary>
        /// トランザクション
        /// </summary>
        [ThreadStatic]
        private static IDbTransaction _transaction;

        /// <summary>
        /// コネクション接続
        /// </summary>
        private ConnectionDecorator _connectionDecorator = null;

        /// <summary>
        /// トランザクション実行
        /// </summary>
        /// <param name="action">委譲処理</param>
        public void Decorate(Action<IDbConnection> action) {
            if(_connectionDecorator == null) {
                throw new QuillException(QMsg.NotFoundDBConnectionDecorator.Get());
            }

            _connectionDecorator.Decorate(connection => ExecuteTransaction(connection, action));
        }

        /// <summary>
        /// トランザクション実行
        /// </summary>
        /// <param name="func">委譲処理</param>
        public RETURN_TYPE Decorate<RETURN_TYPE>(Func<IDbConnection, RETURN_TYPE> func) {
            if(_connectionDecorator == null) {
                throw new QuillException(QMsg.NotFoundDBConnectionDecorator.Get());
            }

            return _connectionDecorator.Decorate(
                connection => ExecuteTransaction(connection, func));
        }

        /// <summary>
        /// トランザクション開始
        /// </summary>
        /// <param name="connection">コネクション</param>
        /// <returns>トランザクション開始フラグ</returns>
        protected virtual bool Begin(IDbConnection connection) {
            bool isBeginTransaction = false;
            if(_transaction == default(IDbTransaction)) {
                _transaction = connection.BeginTransaction();
                isBeginTransaction = true;

                QM.OutputLog("TransactionDecorator#Begin", EnumMsgCategory.DEBUG,
                    QMsg.BeginTx.Get());
            }
            return isBeginTransaction;
        }

        /// <summary>
        /// コミット
        /// </summary>
        /// <param name="isBeginTransaction">トランザクション開始フラグ</param>
        protected virtual void Commit(bool isBeginTransaction) {
            if(isBeginTransaction) {
                _transaction.Commit();
                _transaction.Dispose();
                _transaction = default(IDbTransaction);

                QM.OutputLog("TransactionDecorator#Commit", EnumMsgCategory.DEBUG,
                    QMsg.Committed.Get());
            }
        }

        /// <summary>
        /// ロールバック
        /// </summary>
        /// <param name="isBeginTransaction">トランザクション開始フラグ</param>
        protected virtual void Rollback(bool isBeginTransaction) {
            if(isBeginTransaction) {
                _transaction.Rollback();
                _transaction.Dispose();
                _transaction = default(IDbTransaction);

                QM.OutputLog("TransactionDecorator#Rollback", EnumMsgCategory.DEBUG,
                   QMsg.Rollbacked.Get());
            }
        }

        /// <summary>
        /// トランザクション実行
        /// </summary>
        /// <param name="connection">コネクション</param>
        /// <param name="action">委譲処理</param>
        protected virtual void ExecuteTransaction(IDbConnection connection, Action<IDbConnection> action) {
            bool isBeginTransaction = false;
            try {
                isBeginTransaction = Begin(connection);
                action(connection);
                Commit(isBeginTransaction);
            } catch(System.Exception) {
                Rollback(isBeginTransaction);
                throw;
            }
        }

        /// <summary>
        /// トランザクション実行
        /// </summary>
        /// <typeparam name="RETURN_TYPE">委譲処理戻り値の型</typeparam>
        /// <param name="connection">コネクション</param>
        /// <param name="func">委譲処理</param>
        /// <returns>委譲処理戻り値</returns>
        protected virtual RETURN_TYPE ExecuteTransaction<RETURN_TYPE>(
            IDbConnection connection, Func<IDbConnection, RETURN_TYPE> func) {

            bool isBeginTransaction = false;
            try {
                isBeginTransaction = Begin(connection);
                RETURN_TYPE retValue = func(connection);
                Commit(isBeginTransaction);
                return retValue;
            } catch(System.Exception) {
                Rollback(isBeginTransaction);
                throw;
            }
        }
    }
}
