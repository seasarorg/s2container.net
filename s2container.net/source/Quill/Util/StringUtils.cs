using System.Collections.Generic;
using System.Text;

namespace Quill.Util {
    /// <summary>
    /// 文字列ユーティリティクラス
    /// </summary>
    public static class StringUtils {
        private const string NULL_STR = "[null]";

        /// <summary>
        /// Map形式から文字列への変換
        /// </summary>
        /// <typeparam name="KEY_TYPE">Mapキーの型</typeparam>
        /// <typeparam name="VALUE_TYPE">Map値の型</typeparam>
        /// <param name="map">変換対象Map</param>
        /// <returns>変換した文字列</returns>
        public static string MapToString<KEY_TYPE, VALUE_TYPE>(this IDictionary<KEY_TYPE, VALUE_TYPE> map) {
            if(map == null) {
                return NULL_STR;
            }

            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            foreach(KeyValuePair<KEY_TYPE, VALUE_TYPE> pair in map) {
                builder.AppendFormat("[{0}={1}],", pair.Key, pair.Value);
            }
            builder.Append("]");

            return builder.ToString();
        }

        /// <summary>
        /// Enumerable形式から文字列への変換
        /// </summary>
        /// <typeparam name="VALUE_TYPE">Listの要素型</typeparam>
        /// <param name="list">返還対象List</param>
        /// <returns>変換した文字列</returns>
        public static string EnumrableToString<VALUE_TYPE>(this IEnumerable<VALUE_TYPE> list) {
            if(list == null) {
                return NULL_STR;
            }

            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            foreach(VALUE_TYPE val in list) {
                builder.AppendFormat("[{0}],", val);
            }
            builder.Append("]");
            return builder.ToString();
        }
    }
}
