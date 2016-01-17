using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Quill.Util {
    /// <summary>
    /// 型情報処理ユーティリティクラス
    /// </summary>
    public static class TypeUtils {
        /// <summary>
        /// 親クラスも含めた全フィールド情報の取得
        /// </summary>
        /// <param name="type">フィールド情報を取得する型</param>
        /// <param name="flags">取得条件フラグ</param>
        /// <returns>フィールド情報</returns>
        public static FieldInfo[] GetAllFields(this Type type, BindingFlags flags) {
            ISet<FieldInfo> fields = new HashSet<FieldInfo>();
            for(Type currentType = type; currentType != null; currentType = currentType.BaseType) {
                FieldInfo[] currentFields = currentType.GetFields(flags);
                foreach(FieldInfo fi in currentFields) {
                    if(!fields.Contains(fi)) {
                        fields.Add(fi);
                    }
                }
            }
            return fields.ToArray();
        }
    }
}
