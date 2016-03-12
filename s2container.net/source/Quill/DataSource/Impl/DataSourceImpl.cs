using System;
using System.Data;
using System.Threading;
using Quill.Util;

namespace Quill.DataSource.Impl {
    /// <summary>
    /// データソース実装クラス
    /// </summary>
    public class DataSourceImpl : IDataSource {
        /// <summary>
        /// コネクション
        /// </summary>
        private ThreadLocal<IDbConnection> _connection;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connectionFactory">コネクション生成処理</param>
        public DataSourceImpl(Func<IDbConnection> connectionFactory) {
            _connection = new ThreadLocal<IDbConnection>(connectionFactory);
        }

        /// <summary>
        /// リソース解放
        /// </summary>
        public void Dispose() {
            IDbConnection connection = _connection.Value;
            connection.CloseConnection();
            connection.Dispose();
        }

        /// <summary>
        /// コネクション取得
        /// </summary>
        /// <param name="dataSourceName">データソース名（省略可能）</param>
        /// <returns>コネクション</returns>
        public virtual IDbConnection GetConnection(string dataSourceName = null) {
            return _connection.Value;
        }
    }
}
