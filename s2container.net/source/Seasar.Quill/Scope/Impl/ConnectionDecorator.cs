using System;
using System.Data;
using Quill.Exception;
using QM = Quill.QuillManager;

namespace Quill.Scope.Impl {
    /// <summary>
    /// コネクション接続修飾クラス
    /// </summary>
    public class ConnectionDecorator : IQuillDecorator<IDbConnection>, IDisposable {
        /// <summary>
        /// DB接続
        /// </summary>
        [ThreadStatic]
        protected static IDbConnection _connection = null;

        /// <summary>
        /// 呼び出し大元の関数
        /// </summary>
        [ThreadStatic]
        protected static object _ornerInvoker = null;

        /// <summary>
        /// DB接続
        /// </summary>
        public IDbConnection Connection { get { return _connection; } }

        /// <summary>
        /// 前後をDB接続の開始、終了で挟んで実行
        /// </summary>
        /// <param name="action">本処理</param>
        public virtual void Decorate(Action<IDbConnection> action) {
            ReadyConnection();
            OpenConnection(action);

            try {
                action(_connection);
            } finally {
                CloseConnection(action);
            }
        }

        /// <summary>
        /// 前後をDB接続の開始、終了で挟んで実行
        /// </summary>
        /// <typeparam name="RETURN_TYPE">本処理の戻り値型</typeparam>
        /// <param name="func">本処理</param>
        /// <returns>本処理の戻り値</returns>
        public virtual RETURN_TYPE Decorate<RETURN_TYPE>(Func<IDbConnection, RETURN_TYPE> func) {
            ReadyConnection();
            OpenConnection(func);

            try {
                return func(_connection);
            } finally {
                CloseConnection(func);
            }
        }

        /// <summary>
        /// DB接続開始済か判定
        /// </summary>
        /// <returns>true:Open, false:Close or 未設定</returns>
        public virtual bool IsOpen() {
            return (_connection != null && _connection.State == ConnectionState.Open);
        }

        /// <summary>
        /// リソースの破棄
        /// </summary>
        public void Dispose() {
            if(_connection != null) {
                if(_connection.State == ConnectionState.Open) {
                    _connection.Close();
                }
                _connection.Dispose();
                _connection = null;
            }
        }

        /// <summary>
        /// コネクションが未設定か判定
        /// </summary>
        /// <returns>true:未設定, false:設定済</returns>
        protected virtual bool IsNoDbConnection() {
            return (_connection == null);
        }

        /// <summary>
        /// DB接続準備（未設定の場合は設定する）
        /// </summary>
        protected virtual void ReadyConnection() {
            if(_connection == null) {
                // コネクションをまだ保持していない場合はQuillコンポーネントとして取得
                // スレッドごとにコネクションを管理したいのでキャッシュはしない
                _connection = QM.Container.GetComponent<IDbConnection>(false, true);
            }
        }

        /// <summary>
        /// DB接続開始
        /// </summary>
        /// <param name="invoker">委譲処理</param>
        protected virtual void OpenConnection(object invoker) {
            if(_connection == null) {
                throw new QuillException(QM.Message.GetNoDbConnectionInstance());
            }

            if(_connection.State == ConnectionState.Closed && _ornerInvoker == null) {
                _connection.Open();
                _ornerInvoker = invoker;
            }
        }

        /// <summary>
        /// DB接続終了
        /// </summary>
        /// <param name="invoker"></param>
        protected virtual void CloseConnection(object invoker) {
            if(_connection == null) {
                throw new QuillException(QM.Message.GetNoDbConnectionInstance());
            }

            if(_connection.State == ConnectionState.Open && _ornerInvoker == invoker) {
                _connection.Close();
                _ornerInvoker = null;
            }
        }
    }
}
