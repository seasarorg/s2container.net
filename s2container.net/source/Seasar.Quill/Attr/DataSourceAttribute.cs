using System;

namespace Quill.Attr {
    /// <summary>
    /// データソース指定属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class DataSourceAttribute : Attribute {
        /// <summary>
        /// データソース名
        /// </summary>
        public string DataSourceName { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataSourceName">データソース名</param>
        public DataSourceAttribute(string dataSourceName) {
            DataSourceName = dataSourceName;
        }
    }
}
