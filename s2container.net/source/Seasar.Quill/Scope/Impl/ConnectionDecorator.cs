using System;
using System.Data;
using System.Reflection;
using Quill.Attr;
using Quill.DataSource;
using Quill.Message;
using Quill.Util;
using QM = Quill.QuillManager;

namespace Quill.Scope.Impl {
    /// <summary>
    /// コネクション接続修飾クラス
    /// </summary>
    public class ConnectionDecorator : IQuillDecorator<IDbConnection> {
        /// <summary>
        /// データソース
        /// </summary>
        public IDataSource DataSource { get; set; }

        /// <summary>
        /// 複数データソース対応フラグ
        /// </summary>
        public bool IsMultiDataSource { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ConnectionDecorator() {
            IsMultiDataSource = false;
        }

        /// <summary>
        /// 前後をDB接続の開始、終了で挟んで実行
        /// </summary>
        /// <param name="action">本処理</param>
        public virtual void Decorate(Action<IDbConnection> action) {
            string dataSourceName = GetDataSourceName(action);
            Tuple<IDbConnection, bool> connection = GetOpenedConnection(dataSourceName);

            try {
                action(connection.Item1);
            } finally {
                CloseConnection(connection.Item1, connection.Item2);
            }
        }

        /// <summary>
        /// 前後をDB接続の開始、終了で挟んで実行
        /// </summary>
        /// <typeparam name="RETURN_TYPE">本処理の戻り値型</typeparam>
        /// <param name="func">本処理</param>
        /// <returns>本処理の戻り値</returns>
        public virtual RETURN_TYPE Decorate<RETURN_TYPE>(Func<IDbConnection, RETURN_TYPE> func) {
            string dataSourceName = GetDataSourceName(func);
            Tuple<IDbConnection, bool> connection = GetOpenedConnection(dataSourceName);

            try {
                return func(connection.Item1);
            } finally {
                CloseConnection(connection.Item1, connection.Item2);
            }
        }

        /// <summary>
        /// メソッド情報の取得
        /// </summary>
        /// <param name="invoker">委譲処理</param>
        /// <returns>メソッド情報</returns>
        protected virtual string GetDataSourceName(Delegate invoker) {
            var dataSourceName = string.Empty;
            if(IsMultiDataSource) {
                // 複数データソース対応時のみメソッド情報を取り出す
                var methodInfo = invoker.GetMethodInfo();
                var dataSourceAttr = methodInfo.GetCustomAttribute<DataSourceAttribute>();                
                if(dataSourceAttr != null) {
                    dataSourceName = dataSourceAttr.DataSourceName;
                }

                QM.OutputLog("ConnectionDecorator#GetDataSourceName", EnumMsgCategory.INFO,
                    string.Format("DataSourceName:{0}", dataSourceName));
            }
            return dataSourceName;
        }

        /// <summary>
        /// Open状態のコネクションを取得
        /// </summary>
        /// <param name="dataSourceName">データソース名</param>
        /// <returns>コネクション、接続開始処理フラグ</returns>
        protected virtual Tuple<IDbConnection, bool> GetOpenedConnection(string dataSourceName) {
            IDbConnection connection = DataSource.GetConnection(dataSourceName);
            bool isOpener = false;
            if(connection.State != ConnectionState.Open) {
                connection.OpenConnection();
                isOpener = true;
            }
            return new Tuple<IDbConnection, bool>(connection, isOpener);
        }

        /// <summary>
        /// DB接続終了
        /// </summary>
        /// <param name="connection">コネクション</param>
        /// <param name="isOpener">接続開始フラグ</param>
        protected virtual void CloseConnection(IDbConnection connection, bool isOpener) {
            if(isOpener) {
                connection.CloseConnection();
            }
        }
    }
}
