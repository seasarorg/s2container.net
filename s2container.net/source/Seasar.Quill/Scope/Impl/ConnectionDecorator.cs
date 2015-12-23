using System;
using System.Data;
using QM = Quill.QuillManager;

namespace Quill.Scope.Impl {
    /// <summary>
    /// コネクション接続修飾クラス
    /// </summary>
    public class ConnectionDecorator : IQuillDecorator<IDbConnection> {
        [ThreadStatic]
        private static IDbConnection _connection = null;

        [ThreadStatic]
        private static object _ornerInvoker = null;

        /// <summary>
        /// コネクション
        /// </summary>
        public IDbConnection Connection { get { return _connection; } }

        public virtual void Decorate(Action<IDbConnection> action) {
            if(_connection == null) {
                _connection = QM.Container.GetComponent<IDbConnection>(false, true);
            }

            if(_connection.State == ConnectionState.Closed && _ornerInvoker == null) {
                _connection.Open();
                _ornerInvoker = action;
            }

            try {
                action(_connection);
            } finally {
                if(_connection.State == ConnectionState.Open && _ornerInvoker != null && _ornerInvoker.Equals(action)) {
                    _connection.Close();
                    _ornerInvoker = null;
                }
            }
        }

        public virtual RETURN_TYPE Decorate<RETURN_TYPE>(Func<IDbConnection, RETURN_TYPE> func) {
            if(_connection == null) {
                _connection = QM.Container.GetComponent<IDbConnection>(false, true);
            }

            if(_connection.State == ConnectionState.Closed && _ornerInvoker == null) {
                _connection.Open();
                _ornerInvoker = func;
            }

            try {
                return func(_connection);
            } finally {
                if(_connection.State == ConnectionState.Open && _ornerInvoker != null && _ornerInvoker.Equals(func)) {
                    _connection.Close();
                    _ornerInvoker = null;
                }
            }
        }

        public virtual bool IsOpen() {
            return (_connection != null && _connection.State == ConnectionState.Open);
        }
    }
}
