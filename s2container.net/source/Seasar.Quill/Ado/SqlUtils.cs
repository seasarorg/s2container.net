using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Quill.Attr;
using QM = Quill.QuillManager;

namespace Quill.Ado {
    /// <summary>
    /// SQLユーティリティクラス
    /// </summary>
    public static class SqlUtils {
        /// <summary>
        /// SQL情報キャッシュ
        /// </summary>
        private static readonly ConcurrentDictionary<string, SqlProp> _cachedSqls
            = new ConcurrentDictionary<string, SqlProp>();

        /// <summary>
        /// エンティティへの変換
        /// </summary>
        /// <typeparam name="ENTITY_TYPE">エンティティ型</typeparam>
        /// <param name="reader">DB読み込みオブジェクト</param>
        /// <param name="entity">変換先のエンティティ</param>
        public static void TransToEntity<ENTITY_TYPE>(this IDataReader reader, ENTITY_TYPE entity) where ENTITY_TYPE : new() {
            var fieldInfos = entity.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            if(fieldInfos.Count() == 0) {
                return;
            }

            // １レコード情報を列名-値のMap形式で保持
            var resultMap = new Dictionary<string, object>();
            for(int i = 0; i < reader.FieldCount; i++) {
                resultMap[reader.GetName(i)] = reader.GetValue(i);
            }

            // フィールド名と一致する項目に値設定
            foreach(var fieldInfo in fieldInfos) {
                var fieldName = GetPropertyName(fieldInfo);
                if(resultMap.ContainsKey(fieldName)) {
                    fieldInfo.SetValue(entity, resultMap[fieldName]);
                }
            }
        }

        /// <summary>
        /// 検索実行
        /// </summary>
        /// <typeparam name="ENTITY_TYPE">検索結果格納エンティティ</typeparam>
        /// <param name="connection">DB接続</param>
        /// <param name="sql">SQL</param>
        /// <param name="parameters">SQLパラメータ</param>
        /// <returns>検索結果リスト</returns>
        public static List<ENTITY_TYPE> Select<ENTITY_TYPE>(
            this IDbConnection connection, string sql, IDictionary<string, string> parameters = null)
            where ENTITY_TYPE : new() {

            return Select<ENTITY_TYPE>(connection, sql,SqlUtils.TransToEntity, parameters);
        }

        /// <summary>
        /// 検索実行
        /// </summary>
        /// <typeparam name="ENTITY_TYPE">検索結果格納エンティティ</typeparam>
        /// <param name="connection">DB接続</param>
        /// <param name="sql">SQL</param>
        /// <param name="setEntity">エンティティの設定</param>
        /// <param name="parameters">SQLパラメータ</param>
        /// <returns>検索結果リスト</returns>
        public static List<ENTITY_TYPE> Select<ENTITY_TYPE>(
            this IDbConnection connection, string sql, 
            Action<IDataReader, ENTITY_TYPE> setEntity, 
            IDictionary<string, string> parameters = null)
            where ENTITY_TYPE : new() {

            return Select(connection, () => sql, () => new ENTITY_TYPE(), setEntity,
                (index, paramName, dbParam) => {
                    string v = string.Empty;
                    if(parameters != null && parameters.ContainsKey(paramName)) {
                        v = parameters[paramName];
                    }
                    dbParam.Value = v;
                });
        }

        /// <summary>
        /// 検索実行
        /// </summary>
        /// <typeparam name="ENTITY_TYPE">検索結果格納エンティティ</typeparam>
        /// <param name="connection">DB接続</param>
        /// <param name="sqlGen">SQL生成</param>
        /// <param name="createEntity">エンティティのインスタンス生成</param>
        /// <param name="setEntity">エンティティの設定</param>
        /// <param name="setParameter">パラメータの設定</param>
        /// <returns>検索結果リスト</returns>
        public static List<ENTITY_TYPE> Select<ENTITY_TYPE>(
            this IDbConnection connection,
            Func<string> sqlGen,
            Func<ENTITY_TYPE> createEntity,
            Action<IDataReader, ENTITY_TYPE> setEntity,
            Action<int, string, IDataParameter> setParameter = null) where ENTITY_TYPE : new() {

            string sql = sqlGen();
            List<ENTITY_TYPE> results = new List<ENTITY_TYPE>();

            using(var command = CreateCommand(connection, sql, QM.ReplaceToParamMark, setParameter)) {
                using(var reader = command.ExecuteReader()) {
                    while(reader.Read()) {
                        ENTITY_TYPE entity = createEntity();
                        setEntity(reader, entity);
                        results.Add(entity);
                    }
                }
            }
            return results;
        }

        /// <summary>
        /// DB更新
        /// </summary>
        /// <param name="connection">DB接続</param>
        /// <param name="sqlGen">SQL生成処理</param>
        /// <param name="setParameter">SQLパラメータ設定</param>
        /// <returns>更新件数</returns>
        public static int Update(this IDbConnection connection, Func<string> sqlGen,
            Action<int, string, IDataParameter> setParameter = null) {

            string sql = sqlGen();
            int updateCount = 0;
            using(var command = CreateCommand(connection, sql, QM.ReplaceToParamMark, setParameter)) {
                updateCount = command.ExecuteNonQuery();
            }
            return updateCount;
        }

        /// <summary>
        /// DbCommandの生成
        /// </summary>
        /// <param name="connection">DB接続</param>
        /// <param name="sql">SQL</param>
        /// <param name="replaceToParamMark">パラメータ名からDB固有のSQLパラメータ名への変換処理</param>
        /// <param name="setParameter">パラメータ設定処理</param>
        /// <returns>生成したDbCommandインスタンス</returns>
        private static IDbCommand CreateCommand(
            IDbConnection connection, string sql, 
            Func<string, string> replaceToParamMark,
            Action<int, string, IDataParameter> setParameter) {

            var command = connection.CreateCommand();
            var sqlProp = _cachedSqls.GetOrAdd(sql, new SqlProp(sql, replaceToParamMark));

            command.CommandText = sqlProp.ActualSql;

            if(setParameter != null) {
                var parameterNames = sqlProp.ParameterNames;
                for(int i = 0; i < parameterNames.Count(); i++) {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = parameterNames[i].Mark;
                    setParameter(i, parameterNames[i].Name, parameter);
                    command.Parameters.Add(parameter);
                }
            }
            return command;
        }

        /// <summary>
        /// プロパティ名の取得
        /// </summary>
        /// <param name="fieldInfo">フィールド情報</param>
        /// <returns>検索結果とマッピングするプロパティ名</returns>
        private static string GetPropertyName(FieldInfo fieldInfo) {
            if(fieldInfo.IsDefined(typeof(ColumnAttribute))) {
                return fieldInfo.GetCustomAttribute<ColumnAttribute>().ColumnName;
            }
            return fieldInfo.Name;
        }
    }
}
