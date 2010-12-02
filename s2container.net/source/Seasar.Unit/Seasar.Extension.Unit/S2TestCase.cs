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
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;
using Seasar.Unit.Core;

namespace Seasar.Extension.Unit
{
    /// <summary>
    /// S2Container用テスト補助クラス
    /// </summary>
    /// <remarks>
    /// 継承して使用
    /// S2Containerへの登録やコンポーネントの取得等をショートカット
    /// </remarks>
    public class S2TestCase : S2TestCaseBase
    {
        private ICommandFactory _commandFactory;
        private IS2Container _container;
        public IS2Container Container
        {
            get { return _container; }
            set { _container = value; }
        }

        protected override ICommandFactory CommandFactory
        {
            get
            {
                if (_commandFactory == null && Container.HasComponentDef(typeof(ICommandFactory)))
                {
                    _commandFactory = Container.GetComponent(typeof(ICommandFactory)) as ICommandFactory;
                }
                if (_commandFactory == null)
                {
                    _commandFactory = BasicCommandFactory.INSTANCE;
                }
                return _commandFactory;
            }
        }

        public object GetComponent(string componentName)
        {
            return _container.GetComponent(componentName);
        }

        public object GetComponent(Type componentClass)
        {
            return _container.GetComponent(componentClass);
        }

        public IComponentDef GetComponentDef(string componentName)
        {
            return _container.GetComponentDef(componentName);
        }

        public IComponentDef GetComponentDef(Type componentClass)
        {
            return _container.GetComponentDef(componentClass);
        }

        public void Register(Type componentClass)
        {
            _container.Register(componentClass);
        }

        public void Register(Type componentClass, string componentName)
        {
            _container.Register(componentClass, componentName);
        }

        public void Register(object component)
        {
            _container.Register(component);
        }

        public void Register(object component, string componentName)
        {
            _container.Register(component, componentName);
        }

        public void Register(IComponentDef componentDef)
        {
            _container.Register(componentDef);
        }

        public void Include(string path)
        {
            S2ContainerFactory.Include(Container, S2TestUtils.ConvertPath(GetType(), path));
        }
    }
}
