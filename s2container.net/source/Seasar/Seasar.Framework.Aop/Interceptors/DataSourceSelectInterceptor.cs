#region Copyright
/*
 * Copyright 2005-2013 the Seasar Foundation and the Others.
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

namespace Seasar.Framework.Aop.Interceptors
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

                if (_dataSourceProxy != null && string.IsNullOrEmpty(dataSourceName) == false)
                {
                    _dataSourceProxy.SetDataSourceName(dataSourceName);
                }
            }

            if (_logger.IsDebugEnabled)
            {
                //  デバッグで動いているのならどのデータソースが指定されているか返す
                string ds = _dataSourceProxy.GetDataSourceName();
                _logger.Debug(string.Format("SetDataSourceName={0}", _dataSourceProxy.GetDataSourceName()));
            }

            return invocation.Proceed();
        }
    }
}
