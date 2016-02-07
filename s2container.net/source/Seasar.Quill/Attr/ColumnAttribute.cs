using System;

namespace Quill.Attr {
    /// <summary>
    /// SQL,DB列マッピング属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute {
        /// <summary>
        /// マッピングする列名
        /// </summary>
        public string ColumnName { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="columnName">マッピングする列名</param>
        public ColumnAttribute(string columnName) {
            ColumnName = columnName;
        }
    }
}
