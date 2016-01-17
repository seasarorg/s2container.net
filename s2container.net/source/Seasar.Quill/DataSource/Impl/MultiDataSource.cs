using System;
using System.Collections.Generic;
using System.Data;
using QM = Quill.QuillManager;

namespace Quill.DataSource.Impl {
    /// <summary>
    /// 複数データソース
    /// </summary>
    public class MultiDataSource : IDataSource {
        /// <summary>
        /// データソースMap
        /// </summary>
        protected readonly IDictionary<string, IDataSource> _sources;

        /// <summary>
        /// デフォルトのデータソース（データソース名が指定されない場合に使用）
        /// </summary>
        public IDataSource DefaultDataSource { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MultiDataSource() {
            _sources = new Dictionary<string, IDataSource>();
        }

        /// <summary>
        /// データソース登録
        /// </summary>
        /// <param name="dataSourceName">データソース名</param>
        /// <param name="dataSource">データソース</param>
        public virtual void RegisterDataSource(string dataSourceName, IDataSource dataSource) {
            _sources[dataSourceName] = dataSource;
        }

        /// <summary>
        /// コネクションの取得
        /// </summary>
        /// <param name="dataSourceName">データソース名（省略化）</param>
        /// <returns>コネクション</returns>
        public virtual IDbConnection GetConnection(string dataSourceName = null) {
            if(dataSourceName == null || dataSourceName == string.Empty) {
                return DefaultDataSource.GetConnection();
            }

            if(_sources.ContainsKey(dataSourceName)) {
                return _sources[dataSourceName].GetConnection(dataSourceName);
            }
            throw new ArgumentException(QM.Message.GetNotRegisteredDataSource(dataSourceName), dataSourceName);
        }

        /// <summary>
        /// リソース解放
        /// </summary>
        public virtual void Dispose() {
            foreach(IDataSource source in _sources.Values) {
                source.Dispose();
            }
            _sources.Clear();
        }
    }
}
