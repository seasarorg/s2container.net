using System;
using System.Data;

namespace Quill.Scope.Impl {
    /// <summary>
    /// DB接続を伴う修飾クラス
    /// </summary>
    public abstract class AbstractDbConnectDecorator : IQuillDecorator<IDbConnection> {
        [ThreadStatic]
        private static IDbTransaction _transaction;

        [ThreadStatic]
        private static object _ornerInvoker = null;

        private ConnectionDecorator _connectionDecorator = null;

        public void Decorate(Action<IDbConnection> action) {
            if(_connectionDecorator == null) {
                // TODO connectionDecoratorが未設定である旨をログ出力
                action(null);
                return;
            }

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

        public RETURN_TYPE Decorate<RETURN_TYPE>(Func<IDbConnection, RETURN_TYPE> func) {
            if(_connectionDecorator == null) {
                // TODO connectionDecoratorが未設定である旨をログ出力
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

        protected abstract void ExecuteAction(IDbConnection connection, Action<IDbConnection> action);

        protected abstract RETURN_TYPE ExecuteFunc<RETURN_TYPE>(IDbConnection connection, Func<IDbConnection, RETURN_TYPE> func);

        protected abstract void HandleException(IDbConnection connection, System.Exception ex, object invoker);

        protected abstract void HandleFinally(IDbConnection connection, object invoker);
    }
}
