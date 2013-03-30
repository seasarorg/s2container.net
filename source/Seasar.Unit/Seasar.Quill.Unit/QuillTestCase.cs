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

using System;

using Seasar.Extension.ADO;
using Seasar.Extension.Unit;
using Seasar.Quill.Database.DataSource.Impl;
using Seasar.Extension.ADO.Impl;

namespace Seasar.Quill.Unit
{
	public class QuillTestCase : S2TestCase
	{
        private QuillContainer _qContainer = null;
        private ICommandFactory _commandFactory = null;

        public QuillContainer QContainer
        {
            get { return _qContainer; }
            set { _qContainer = value; }
        }

        private QuillInjector _injector = null;

        public QuillInjector Injector
        {
            get { return _injector; }
            set { _injector = value; }
        }

        public override ICommandFactory CommandFactory
        {
            get
            {
                //  テストデータの書き込みにカスタムしたCommandFactoryが
                //  必要になることはないと思われるため
                //  現状では実質BasicCommandFactory固定としています。
                if (_commandFactory == null)
                {
                    _commandFactory = BasicCommandFactory.INSTANCE;
                }
                return _commandFactory;
            }
        }

        public object GetQuillComponent(Type componentClass)
        {
            QuillComponent component = _qContainer.GetComponent(componentClass);
            return component.GetComponentObject(componentClass);
        }

        public object GetQuillComponent(Type interfaceType, Type implType)
        {
            QuillComponent component = _qContainer.GetComponent(interfaceType, implType);
            return component.GetComponentObject(component.ReceiptType);
        }

        public virtual void Inject(object target)
        {
            _injector.Inject(target);
        }

        public virtual void SetDataSourceName(string dataSourceName)
        {
            SelectableDataSourceProxyWithDictionary dataSourceProxy 
                = DataSource as SelectableDataSourceProxyWithDictionary;
            if (dataSourceProxy != null)
            {
                dataSourceProxy.SetDataSourceName(dataSourceName);
            }
        }

        
	}
}
