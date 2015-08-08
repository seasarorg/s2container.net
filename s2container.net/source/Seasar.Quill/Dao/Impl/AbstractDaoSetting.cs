#region Copyright
/*
 * Copyright 2005-2015 the Seasar Foundation and the Others.
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

using System;
using Seasar.Dao;
using Seasar.Extension.ADO;
using Seasar.Framework.Aop;

namespace Seasar.Quill.Dao.Impl
{
    /// <summary>
    /// Dao設定抽象クラス
    /// </summary>
    public abstract class AbstractDaoSetting : IDaoSetting
    {
        protected bool isNeedSetup = true;

        protected IDaoMetaDataFactory daoMetaDataFactory = null;

        protected IMethodInterceptor daoInterceptor = null;

        protected string dataSourceName = null;

        #region IDaoSetting メンバ

        [Obsolete("内部で使用していないため、削除予定です。")]
        public virtual IDaoMetaDataFactory DaoMetaDataFactory => daoMetaDataFactory;

        public virtual IMethodInterceptor DaoInterceptor => daoInterceptor;

        public virtual string DataSourceName => dataSourceName;

        public void Setup(IDataSource dataSource)
        {
            SetupDao(dataSource);
            isNeedSetup = false;
        }

        public bool IsNeedSetup() => isNeedSetup;

        #endregion

        /// <summary>
        /// Dao関係の設定を行う
        /// </summary>
        /// <param name="dataSource"></param>
        protected abstract void SetupDao(IDataSource dataSource);
    }
}
