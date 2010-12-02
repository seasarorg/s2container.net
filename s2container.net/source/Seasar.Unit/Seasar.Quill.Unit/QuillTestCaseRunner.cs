#region Copyright
/*
 * Copyright 2005-2010 the Seasar Foundation and the Others.
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
using Seasar.Extension.Tx;
using Seasar.Extension.Tx.Impl;
using Seasar.Extension.Unit;
using Seasar.Quill.Dao;
using Seasar.Quill.Database.DataSource.Impl;
using Seasar.Quill.Database.Tx;
using Seasar.Quill.Util;
using Seasar.Unit.Core;

namespace Seasar.Quill.Unit
{
    /// <summary>
    /// Quill用テスト実行クラス
    /// </summary>
    public class QuillTestCaseRunner : S2TestCaseRunnerBase
    {
        private Type _daoSettingType;
        private Type _transactionSettingType;

        public QuillTestCaseRunner(Tx txTreatment)
            : base(txTreatment)
        {
        }

        protected override void SetUpContainer(object fixtureInstance)
        {
            QuillConfig.ConfigPath = null;
            var config = QuillConfig.GetInstance();

            //  Quillの設定ファイルがあればS2Dao、Transaction設定を使用する
            if (config.HasQuillConfig())
            {
                _daoSettingType = config.GetDaoSettingType();
                _transactionSettingType = config.GetTransationSettingType();
            }
            else
            {
                _daoSettingType = SettingUtil.GetDefaultDaoSettingType();
                _transactionSettingType = SettingUtil.GetDefaultTransactionType();
            }

            //  必要なコンポーネントを作成した上でインジェクション実行
            QuillInjector.GetInstance().Inject(fixtureInstance);
        }

        protected override void SetUpAfterContainerInit(object fixtureInstance)
        {
            SetUpDataSource(fixtureInstance);
        }

        protected override void TearDownBeforeContainerDestroy(object fixtureInstance)
        {
            TearDownDataSource(fixtureInstance);
        }

        protected override void TearDownContainer(object fixtureInstance)
        {
            QuillInjector.GetInstance().Destroy();
        }

        protected override ITransactionContext GetTransactionContext()
        {
            var container = QuillInjector.GetInstance().Container;
            var txSetting = (ITransactionSetting)ComponentUtil.GetComponent(
                container, _transactionSettingType);

            //  SetupInjectionが呼ばれていればTransactionContextは設定済み
            return txSetting.TransactionContext;
        }

        protected override Extension.ADO.IDataSource GetDataSource(object fixtureInstance)
        {
            var container = QuillInjector.GetInstance().Container;
            var dataSource = (SelectableDataSourceProxyWithDictionary)ComponentUtil.GetComponent(
                container, typeof(SelectableDataSourceProxyWithDictionary));
            dataSource.SetDataSourceName(null);

            //  DaoとTransaction設定は予めやっておく
            //  （テストするクラスと同じDataSource,TransactionContextを使用するため)
            var daoSetting = (IDaoSetting)ComponentUtil.GetComponent(
                container, _daoSettingType);
            if (daoSetting.IsNeedSetup())
            {
                daoSetting.Setup(dataSource);
            }

            if (_txTreatment != Tx.NotSupported)
            {
                var txSetting = (ITransactionSetting)ComponentUtil.GetComponent(
                    container, _transactionSettingType);
                if (txSetting.IsNeedSetup())
                {
                    txSetting.Setup(dataSource);
                }
            }
            return dataSource;
        }

        protected override void TearDownDataSource(object fixtureInstance)
        {
            if (_dataSource == null || 
                ! typeof(SelectableDataSourceProxyWithDictionary).IsAssignableFrom(_dataSource.GetType()))
            {
                return;
            }

            var ds = (SelectableDataSourceProxyWithDictionary)_dataSource;
            foreach (var dataSourceName in ds.DataSourceCollection.Keys)
            {
                var currentDataSource = ds.GetDataSource(dataSourceName);
                var txDataSource = currentDataSource as TxDataSource;
                if (txDataSource != null)
                {
                    if (txDataSource.Context != null && txDataSource.Context.Connection != null)
                    {
                        txDataSource.CloseConnection(txDataSource.Context.Connection);
                    }
                }
            }
            ds.SetDataSourceName(null);
            base.TearDownDataSource(fixtureInstance);
        }
    }
}
