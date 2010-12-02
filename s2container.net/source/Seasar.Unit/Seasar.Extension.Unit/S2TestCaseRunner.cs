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
using System.Collections;
using System.Reflection;
using Seasar.Extension.ADO;
using Seasar.Extension.Tx;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;
using Seasar.Unit.Core;

namespace Seasar.Extension.Unit
{
    /// <summary>
    /// S2Containerを使用したテスト実行クラス
    /// </summary>
    public class S2TestCaseRunner : S2TestCaseRunnerBase
    {
        private static readonly string DATASOURCE_NAME = string.Format("Ado{0}DataSource", ContainerConstants.NS_SEP);

        private IS2Container _container;
        private IList _bindedFields;

        protected IS2Container Container
        {
            get { return _container; }
            set { _container = value; }
        }

        public S2TestCaseRunner(Seasar.Extension.Unit.Tx txTreatment)
            : base(txTreatment)
        {
        }

        protected override void SetUpContainer(object fixtureInstance)
        {
            SingletonS2ContainerFactory.Init();
            _container = SingletonS2ContainerFactory.Container;

            if (typeof(S2TestCase).IsAssignableFrom(fixtureInstance.GetType()))
            {
                ((S2TestCase)fixtureInstance).Container = _container;
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
                _container = null;
            }
        }

        protected override ITransactionContext GetTransactionContext()
        {
            return (ITransactionContext)Container.GetComponent(typeof(ITransactionContext));
        }

        protected override void SetUpAfterContainerInit(object fixtureInstance)
        {
            _container.Init();
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
            for (var type = fixtureInstance.GetType();
                (type != typeof(S2TestCase) && type != null);
                type = type.BaseType)
            {
                var fields = type.GetFields(
                            BindingFlags.DeclaredOnly |
                            BindingFlags.Public |
                            BindingFlags.NonPublic |
                            BindingFlags.Instance |
                            BindingFlags.Static);

                for (int i = 0; i < fields.Length; ++i)
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
                if (_container.HasComponentDef(name))
                {
                    var componentType = _container.GetComponentDef(name).ComponentType;
                    if (componentType == null)
                    {
                        component = _container.GetComponent(name);
                        if (component != null)
                        {
                            componentType = component.GetType();
                        }
                    }

                    if (componentType != null
                                && fieldInfo.FieldType.IsAssignableFrom(componentType))
                    {
                        if (component == null)
                        {
                            component = _container.GetComponent(name);
                        }
                    }
                    else
                    {
                        component = null;
                    }
                }

                if (component == null
                    && _container.HasComponentDef(fieldInfo.FieldType))
                {
                    component = _container.GetComponent(fieldInfo.FieldType);
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
            for (int i = 0; i < _bindedFields.Count; ++i)
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
    }
}
