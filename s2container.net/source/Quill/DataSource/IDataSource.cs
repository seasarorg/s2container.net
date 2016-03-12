using System;
using System.Data;

namespace Quill.DataSource {
    /// <summary>
    /// データソースインターフェース
    /// </summary>
    public interface IDataSource : IDisposable {
        /// <summary>
        /// コネクションの取得
        /// </summary>
        /// <param name="dataSourceName">データソース名（省略可）</param>
        /// <returns>コネクション</returns>
        IDbConnection GetConnection(string dataSourceName = default(string));
    }
}
