#region Copyright
/*
 * Copyright 2005-2009 the Seasar Foundation and the Others.
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

namespace Seasar.Quill.Dao.Interceptor
{
    /// <summary>
    /// データソース選択用Intarceptor
    /// </summary>
    public class DataSourceSelectInterceptor : AbstractInterceptor
    {
        private readonly Logger _logger = Logger.GetLogger(typeof(DataSourceSelectInterceptor));

        private IDictionary<MemberInfo, string> _daoDataSourceMap = new Dictionary<MemberInfo, string>();

        /// <summary>
        /// データソース名を指定するメンバとデータソース名のマッピング
        /// </summary>
        public IDictionary<MemberInfo, string> DaoDataSourceMap
        {
            set { _daoDataSourceMap = value; }
            get { return _daoDataSourceMap; }
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

        public override object Invoke(IMethodInvocation invocation)
        {
            if (DataSourceProxy == null)
            {
                throw new QuillApplicationException("EQLL0038"); 
            }

            IComponentDef def = GetComponentDef(invocation);
            if (def != null)
            {
                string dataSourceName = null;
                //  Daoクラス/メソッド用にデータソース名が設定されていれば
                //  そのデータソースを適用する
                MemberInfo daoClassType = def.ComponentType;
                if (_daoDataSourceMap.ContainsKey(daoClassType))
                {
                    dataSourceName = _daoDataSourceMap[daoClassType];
                }
                else
                {
                    //  クラスになければメソッドに定義されているか
                    MemberInfo daoMethod = invocation.Method;
                    if (_daoDataSourceMap.ContainsKey(daoMethod))
                    {
                        dataSourceName = _daoDataSourceMap[daoMethod];
                    }
                }

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
