using System;
using System.Data;
using Quill.Exception;
using Quill.Message;
using QM = Quill.QuillManager;

namespace Quill.Scope.Impl {
    /// <summary>
    /// トランザクション修飾クラス
    /// </summary>
    public class TransactionDecorator : IQuillDecorator<IDbTransaction> {
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
        public void Decorate(Action<IDbTransaction> action) {
            if(_connectionDecorator == null) {
                throw new QuillException(QMsg.NotFoundDBConnectionDecorator.Get());
            }

            _connectionDecorator.Decorate(connection => ExecuteTransaction(connection, action));
        }

        /// <summary>
        /// トランザクション実行
        /// </summary>
        /// <param name="func">委譲処理</param>
        public RETURN_TYPE Decorate<RETURN_TYPE>(Func<IDbTransaction, RETURN_TYPE> func) {
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
        protected virtual IDbTransaction Begin(IDbConnection connection) {
            if(!IsInTransaction(_transaction)) {
                _transaction = connection.BeginTransaction();
                QM.OutputLog(GetType(), EnumMsgCategory.DEBUG, QMsg.BeginTx.Get());
            }
            return _transaction;
        }

        /// <summary>
        /// コミット
        /// </summary>
        /// <param name="tx">トランザクション</param>
        protected virtual void Commit(IDbTransaction tx) {
            if(IsInTransaction(tx)) {
                tx.Commit();
                tx.Dispose();
                InitTransaction();

                QM.OutputLog(GetType(), EnumMsgCategory.DEBUG, QMsg.Committed.Get());
            }
        }

        /// <summary>
        /// ロールバック
        /// </summary>
        /// <param name="tx">トランザクション</param>
        protected virtual void Rollback(IDbTransaction tx) {
            if(IsInTransaction(tx)) {
                tx.Rollback();
                tx.Dispose();
                InitTransaction();

                QM.OutputLog(GetType(), EnumMsgCategory.DEBUG, QMsg.Rollbacked.Get());
            }
        }

        /// <summary>
        /// トランザクション実行
        /// </summary>
        /// <param name="connection">コネクション</param>
        /// <param name="action">委譲処理</param>
        protected virtual void ExecuteTransaction(IDbConnection connection, Action<IDbTransaction> action) {
            var tx = default(IDbTransaction);
            try {
                tx = Begin(connection);
                action(tx);
                Commit(tx);
            } catch(System.Exception) {
                Rollback(tx);
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
            IDbConnection connection, Func<IDbTransaction, RETURN_TYPE> func) {

            var tx = default(IDbTransaction);
            try {
                tx = Begin(connection);
                RETURN_TYPE retValue = func(tx);
                Commit(tx);
                return retValue;
            } catch(System.Exception) {
                Rollback(tx);
                throw;
            }
        }

        /// <summary>
        /// Trasaction中か判定
        /// </summary>
        /// <returns>true:Transaction中, false:Transaction外</returns>
        protected virtual bool IsInTransaction(IDbTransaction tx) {
            return (tx != default(IDbTransaction));
        }

        /// <summary>
        /// Transaction初期化
        /// </summary>
        protected virtual void InitTransaction() {
            _transaction = default(IDbTransaction);
        }
    }
}
