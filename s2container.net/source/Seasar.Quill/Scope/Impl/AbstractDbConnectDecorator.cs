using System;
using System.Data;
using Quill.Message;
using QM = Quill.QuillManager;

namespace Quill.Scope.Impl {
    /// <summary>
    /// DB接続を伴う修飾抽象クラス
    /// </summary>
    public abstract class AbstractDbConnectDecorator : IQuillDecorator<IDbConnection> {
        /// <summary>
        /// ADO.NETトランザクション
        /// </summary>
        [ThreadStatic]
        private static IDbTransaction _transaction;

        /// <summary>
        /// 呼び出し元の関数
        /// </summary>
        [ThreadStatic]
        private static object _ornerInvoker = null;

        /// <summary>
        /// DB接続修飾オブジェクト
        /// </summary>
        private ConnectionDecorator _connectionDecorator = null;

        /// <summary>
        /// 修飾実行
        /// </summary>
        /// <param name="action">修飾対象処理</param>
        public void Decorate(Action<IDbConnection> action) {
            if(_connectionDecorator == null) {
                QM.OutputLog("AbstractDbConnectDecorator#Decorate",
                    EnumMsgCategory.WARN, QM.Message.GetNotFoundDBConnectionDecorator());
                action(null);
                return;
            }
            // 接続をまだ開始していない場合は接続開始から
            if(!_connectionDecorator.IsOpen()) {
                _connectionDecorator.Decorate(c => Decorate(action));
                return;
            }

            try {
                ExecuteAction(_connectionDecorator.Connection, action);
            } catch(System.Exception ex) {
                HandleException(_connectionDecorator.Connection, ex, action);
            } finally {
                HandleFinally(_connectionDecorator.Connection, action);
            }
        }

        /// <summary>
        /// 修飾実行
        /// </summary>
        /// <param name="func">修飾対象処理</param>
        /// <returns>修飾対象処理の戻り値</returns>
        public RETURN_TYPE Decorate<RETURN_TYPE>(Func<IDbConnection, RETURN_TYPE> func) {
            if(_connectionDecorator == null) {
                QM.OutputLog("AbstractDbConnectDecorator#Decorate",
                    EnumMsgCategory.WARN, QM.Message.GetNotFoundDBConnectionDecorator());
                return func(null);
            }

            if(!_connectionDecorator.IsOpen()) {
                return _connectionDecorator.Decorate(c => Decorate(func));
            }

            try {
                return ExecuteFunc(_connectionDecorator.Connection, func);
            } catch(System.Exception ex) {
                HandleException(_connectionDecorator.Connection, ex, func);
                return func(_connectionDecorator.Connection);
            } finally {
                HandleFinally(_connectionDecorator.Connection, func);
            }
        }

        /// <summary>
        /// 接続、トランザクション処理を実行可能か判定
        /// </summary>
        /// <param name="currentInvoker"></param>
        /// <returns></returns>
        protected virtual bool IsExecutable(object currentInvoker) {
            return (_ornerInvoker == currentInvoker);
        }

        /// <summary>
        /// 委譲元の処理実行（戻り値なし）
        /// </summary>
        /// <param name="connection">DB接続</param>
        /// <param name="action">委譲元の処理</param>
        protected abstract void ExecuteAction(IDbConnection connection, Action<IDbConnection> action);

        /// <summary>
        /// 委譲元の処理実行（戻り値型あり）
        /// </summary>
        /// <param name="connection">DB接続</param>
        /// <param name="func">委譲元の処理</param>
        /// <returns>委譲元処理の実装</returns>
        protected abstract RETURN_TYPE ExecuteFunc<RETURN_TYPE>(IDbConnection connection, Func<IDbConnection, RETURN_TYPE> func);

        /// <summary>
        /// 共通例外処理
        /// </summary>
        /// <param name="connection">DB接続</param>
        /// <param name="ex">発生例外</param>
        /// <param name="invoker">委譲元処理</param>
        protected abstract void HandleException(IDbConnection connection, System.Exception ex, object invoker);

        /// <summary>
        /// 共通後処理
        /// </summary>
        /// <param name="connection">DB接続</param>
        /// <param name="invoker">委譲元処理</param>
        protected abstract void HandleFinally(IDbConnection connection, object invoker);
    }
}
