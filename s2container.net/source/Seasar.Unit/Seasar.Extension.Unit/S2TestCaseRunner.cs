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

using System.Collections;
using Seasar.Extension.ADO;
using Seasar.Extension.Tx;
using Seasar.Framework.Container;
using Seasar.Framework.Util;

#if NET_4_0
using System;
using System.Reflection;
using Seasar.Framework.Container.Factory;
using Seasar.Unit.Core;
#else
#region NET2.0
using MbUnit.Core.Invokers;
using Seasar.Extension.Tx.Impl;
using Seasar.Framework.Unit;
#endregion
#endif

namespace Seasar.Extension.Unit
{
    /// <summary>
    /// S2Containerを使用したテスト実行クラス
    /// </summary>
#if NET_4_0
    public class S2TestCaseRunner : S2TestCaseRunnerBase
#else
#region NET2.0
    public class S2TestCaseRunner : S2FrameworkTestCaseRunner
#endregion
#endif
    {
        private static readonly string DATASOURCE_NAME = string.Format("Ado{0}DataSource", ContainerConstants.NS_SEP);

#if NET_4_0
        private IList _bindedFields;

        protected IS2Container Container { get; set; }

        public S2TestCaseRunner(Tx txTreatment)
            : base(txTreatment)
        {
        }

        protected override void SetUpContainer(object fixtureInstance)
        {
            SingletonS2ContainerFactory.Init();
            Container = SingletonS2ContainerFactory.Container;

            if (typeof(S2TestCase).IsAssignableFrom(fixtureInstance.GetExType()))
            {
                ((S2TestCase)fixtureInstance).Container = Container;
            }
        }

        protected override void TearDownContainer(object fixtureInstance)
        {
            try
            {
                SingletonS2ContainerFactory.Destroy();
            }
            finally
            {
                Container = null;
            }
        }

        protected override ITransactionContext GetTransactionContext()
        {
            return (ITransactionContext)Container.GetComponent(typeof(ITransactionContext));
        }

        protected override void SetUpAfterContainerInit(object fixtureInstance)
        {
            Container.Init();
            BindFields(fixtureInstance);
            SetUpAfterBindFields();
            SetUpDataSource(fixtureInstance);
        }

        protected override void TearDownBeforeContainerDestroy(object fixtureInstance)
        {
            try
            {
                TearDownBeforeUnbindFields();                
            }
            finally
            {
                try
                {
                    UnbindFields(fixtureInstance);
                }
                finally
                {
                    TearDownDataSource(fixtureInstance);
                }
            }
            
        }

        protected override IDataSource GetDataSource(object fixtureInstance)
        {
            if (Container.HasComponentDef(DATASOURCE_NAME))
            {
                return Container.GetComponent(DATASOURCE_NAME) as IDataSource;
            }

            if (Container.HasComponentDef(typeof(IDataSource)))
            {
                return Container.GetComponent(typeof(IDataSource)) as IDataSource;
            }
            return null;
        }

        protected virtual void SetUpAfterBindFields()
        {
        }

        protected virtual void TearDownBeforeUnbindFields()
        {
        }

        protected void BindFields(object fixtureInstance)
        {
            _bindedFields = new ArrayList();
            for (var type = fixtureInstance.GetExType();
                (type != typeof(S2TestCase) && type != null);
                type = type.BaseType)
            {
                var fields = type.GetFields(
                            BindingFlags.DeclaredOnly |
                            BindingFlags.Public |
                            BindingFlags.NonPublic |
                            BindingFlags.Instance |
                            BindingFlags.Static);

                for (var i = 0; i < fields.Length; ++i)
                {
                    BindField(fixtureInstance, fields[i]);
                }
            }
        }

        protected void BindField(object fixtureInstance, FieldInfo fieldInfo)
        {
            if (IsAutoBindable(fieldInfo))
            {
                if (fieldInfo.FieldType.ToString() == "System.DateTime")
                {
                    var dateValue = (DateTime)fieldInfo.GetValue(fixtureInstance);
                    if (DateTime.MinValue != dateValue)
                    {
                        return;
                    }
                }
                else if (fieldInfo.GetValue(fixtureInstance) != null)
                {
                    return;
                }
                var name = S2TestUtils.NormalizeName(fieldInfo.Name);
                object component = null;
                if (Container.HasComponentDef(name))
                {
                    var componentType = Container.GetComponentDef(name).ComponentType;
                    if (componentType == null)
                    {
                        component = Container.GetComponent(name);
                        if (component != null)
                        {
                            componentType = component.GetExType();
                        }
                    }

                    if (componentType != null
                                && fieldInfo.FieldType.IsAssignableFrom(componentType))
                    {
                        if (component == null)
                        {
                            component = Container.GetComponent(name);
                        }
                    }
                    else
                    {
                        component = null;
                    }
                }

                if (component == null
                    && Container.HasComponentDef(fieldInfo.FieldType))
                {
                    component = Container.GetComponent(fieldInfo.FieldType);
                }

                if (component != null)
                {
                    fieldInfo.SetValue(fixtureInstance, component);
                    _bindedFields.Add(fieldInfo);
                }
            }
        }



        protected bool IsAutoBindable(FieldInfo fieldInfo)
        {
            return !fieldInfo.IsStatic && !fieldInfo.IsLiteral
                        && !fieldInfo.IsInitOnly; // && !fieldInfo.FieldType.IsValueType;
        }

        protected void UnbindFields(object fixtureInstance)
        {
            for (var i = 0; i < _bindedFields.Count; ++i)
            {
                var fieldInfo = (FieldInfo)_bindedFields[i];
                try
                {
                    if (!fieldInfo.FieldType.IsValueType)
                    {
                        fieldInfo.SetValue(fixtureInstance, null);
                    }
                }
                catch (ArgumentException e)
                {
                    Console.Error.WriteLine(e);
                }
                catch (FieldAccessException e)
                {
                    Console.Error.WriteLine(e);
                }
            }
        }
#else
#region NET2.0
        protected Tx _tx;
        private ITransactionContext _tc;
        private IDataSource _dataSource;

        public object Run(IRunInvoker invoker, object o, IList args, Tx tx)
        {
            _tx = tx;
            _fixture = o as S2TestCase;
            return Run(invoker, o, args);
        }

        protected override void BeginTransactionContext()
        {
            if (Tx.NotSupported != _tx)
            {
                _tc = (ITransactionContext)container.GetComponent(typeof(ITransactionContext));
                _tc.Begin();
            }
        }

        protected override void EndTransactionContext()
        {
            if (_tc != null)
            {
                if (Tx.Commit == _tx)
                {
                    _tc.Commit();
                }
                if (Tx.Rollback == _tx)
                {
                    _tc.Rollback();
                }
            }
        }

        protected override void SetUpAfterContainerInit()
        {
            base.SetUpAfterContainerInit();
            SetupDataSource();
        }

        protected override void TearDownBeforeContainerDestroy()
        {
            TearDownDataSource();
            base.TearDownBeforeContainerDestroy();
        }

        protected virtual void SetupDataSource()
        {
            if (container.HasComponentDef(DATASOURCE_NAME))
            {
                _dataSource = container.GetComponent(DATASOURCE_NAME) as IDataSource;
            }
            else if (container.HasComponentDef(typeof(IDataSource)))
            {
                _dataSource = container.GetComponent(typeof(IDataSource)) as IDataSource;
            }
            if (_fixture != null && _dataSource != null)
            {
                ((S2TestCase)_fixture).SetDataSource(_dataSource);
            }
        }

        protected virtual void TearDownDataSource()
        {
            TxDataSource txDataSource = _dataSource as TxDataSource;
            if (txDataSource != null)
            {
                if (txDataSource.Context.Connection != null)
                {
                    txDataSource.CloseConnection(txDataSource.Context.Connection);
                }
            }

            S2TestCase fixture = _fixture as S2TestCase;
            if (fixture != null)
            {
                if (fixture.HasConnection)
                {
                    ConnectionUtil.Close(fixture.Connection);
                    fixture.SetConnection(null);
                }
                fixture.SetDataSource(null);
            }
            _dataSource = null;
        }
#endregion
#endif
    }
}
