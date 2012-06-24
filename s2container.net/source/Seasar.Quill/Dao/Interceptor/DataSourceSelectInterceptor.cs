#region Copyright
/*
 * Copyright 2005-2012 the Seasar Foundation and the Others.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
 * either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 */
#endregion

using System.Collections.Generic;
using System.Reflection;
using Seasar.Extension.ADO.Impl;
using Seasar.Framework.Container;
using Seasar.Framework.Log;
using Seasar.Framework.Aop.Interceptors;
using Seasar.Framework.Aop;
using Seasar.Quill.Exception;
using Seasar.Quill.Util;
using System;

namespace Seasar.Quill.Dao.Interceptor
{
    /// <summary>
    /// データソース選択用Intarceptor
    /// （一つのMemberで複数のデータソース切り替えが設定されている
    /// 可能性があるのでこのInterceptorはQuillContainerで管理せずに
    /// データソース名ごとにインスタンスを生成します）
    /// </summary>
    public class DataSourceSelectInterceptor : AbstractInterceptor
    {
        private static readonly Logger _logger = Logger.GetLogger(typeof(DataSourceSelectInterceptor));

        /// <summary>
        /// データソース名ごとにインスタンスを生成するためのHashMap
        /// [Key=データソース名, Value=Interceptorインスタンス]
        /// </summary>
        private static readonly IDictionary<string, DataSourceSelectInterceptor> _interceptorMap
            = new Dictionary<string, DataSourceSelectInterceptor>();

        #region Property
        [Obsolete("データソース名ごとにInterceptorのインスタンスを生成する方針に切替のため削除予定")]
        private IDictionary<MemberInfo, string> _daoDataSourceMap = new Dictionary<MemberInfo, string>();

        /// <summary>
        /// データソース名を指定するメンバとデータソース名のマッピング
        /// </summary>
        [Obsolete("データソース名ごとにInterceptorのインスタンスを生成する方針に切替のため削除予定")]
        public IDictionary<MemberInfo, string> DaoDataSourceMap
        {
            set { _daoDataSourceMap = value; }
            get { return _daoDataSourceMap; }
        }

        protected readonly string _dataSourceName;
        /// <summary>
        /// 切り替え後のデータソース名
        /// </summary>
        public virtual string DataSourceName
        {
            get { return _dataSourceName; }
        }


        private AbstractSelectableDataSourceProxy _dataSourceProxy;

        /// <summary>
        /// データソース
        /// </summary>
        public AbstractSelectableDataSourceProxy DataSourceProxy
        {
            set { _dataSourceProxy = value; }
            get { return _dataSourceProxy; }
        }
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataSourceName">(Not Null or Empty</param>
        protected DataSourceSelectInterceptor(string dataSourceName)
        {
            _dataSourceName = dataSourceName;
        }

        #region static
        /// <summary>
        /// １データソース名＝１インスタンスを生成する
        /// </summary>
        /// <param name="dataSourceName"></param>
        /// <returns></returns>
        public static DataSourceSelectInterceptor GetInstance(string dataSourceName)
        {
            if (string.IsNullOrEmpty(dataSourceName))
            {
                throw new ArgumentNullException("dataSourceName");
            }
   
            if(_interceptorMap.ContainsKey(dataSourceName))
            {
                return _interceptorMap[dataSourceName];
            }

            lock(typeof(DataSourceSelectInterceptor))
            {
                if(_interceptorMap.ContainsKey(dataSourceName))
                {
                    return _interceptorMap[dataSourceName];
                }

                _interceptorMap[dataSourceName] = new DataSourceSelectInterceptor(dataSourceName);
            }
            return _interceptorMap[dataSourceName];
        }

        #endregion

        public override object Invoke(IMethodInvocation invocation)
        {
            if (DataSourceProxy == null)
            {
                throw new QuillApplicationException("EQLL0038"); 
            }

            IComponentDef def = GetComponentDef(invocation);
            if (def != null)
            {
                string dataSourceName = DataSourceName;

                if (DataSourceProxy != null && string.IsNullOrEmpty(dataSourceName) == false)
                {
                    DataSourceProxy.SetDataSourceName(dataSourceName);
                }
            }

            if (_logger.IsDebugEnabled)
            {
                //  デバッグで動いているのならどのデータソースが指定されているか返す
                _logger.Debug(MessageUtil.GetSimpleMessage("IQLL0012",
                    new object[] {  (DataSourceProxy.GetDataSourceName() ?? "null") }));
            }

            return invocation.Proceed();
        }
    }
}
