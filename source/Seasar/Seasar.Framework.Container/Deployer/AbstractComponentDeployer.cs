#region Copyright
/*
 * Copyright 2005-2008 the Seasar Foundation and the Others.
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
using Seasar.Framework.Container.Assembler;
using Seasar.Framework.Container.Util;

namespace Seasar.Framework.Container.Deployer
{
    public class AbstractComponentDeployer : IComponentDeployer
    {
        private readonly IComponentDef _componentDef;
        private IConstructorAssembler _constructorAssembler;
        private IPropertyAssembler _propertyAssembler;
        private IMethodAssembler _initMethodAssembler;
        private IMethodAssembler _destroyMethodAssembler;

        public AbstractComponentDeployer(IComponentDef componentDef)
        {
            _componentDef = componentDef;
            SetupAssembler();
        }

        #region ComponentDeployer ƒƒ“ƒo

        public virtual object Deploy(Type receiveType)
        {
            return null;
        }

        public virtual void InjectDependency(object outerComponent)
        {
        }

        public virtual void Init()
        {
        }

        #endregion

        public virtual void Destroy()
        {
        }

        protected object GetProxy(Type receiveType)
        {
            if (receiveType == null)
            {
                return null;
            }
            return ComponentDef.GetProxy(receiveType);
        }

        protected IComponentDef ComponentDef
        {
            get { return _componentDef; }
        }

        protected IConstructorAssembler ConstructorAssembler
        {
            get { return _constructorAssembler; }
        }

        protected IPropertyAssembler PropertyAssembler
        {
            get { return _propertyAssembler; }
        }

        protected IMethodAssembler InitMethodAssembler
        {
            get { return _initMethodAssembler; }
        }

        protected IMethodAssembler DestroyMethodAssembler
        {
            get { return _destroyMethodAssembler; }
        }

        private void SetupAssembler()
        {
            string autoBindingMode = _componentDef.AutoBindingMode;
            if (AutoBindingUtil.IsAuto(autoBindingMode))
            {
                SetupAssemblerForAuto();
            }
            else if (AutoBindingUtil.IsConstructor(autoBindingMode))
            {
                SetupAssemblerForConstructor();
            }
            else if (AutoBindingUtil.IsProperty(autoBindingMode))
            {
                SetupAssemblerForProperty();
            }
            else if (AutoBindingUtil.IsNone(autoBindingMode))
            {
                SetupAssemblerForNone();
            }
            else
            {
                throw new ArgumentException(autoBindingMode);
            }
            _initMethodAssembler = new DefaultInitMethodAssembler(_componentDef);
            _destroyMethodAssembler = new DefaultDestroyMethodAssembler(_componentDef);
        }

        private void SetupAssemblerForAuto()
        {
            SetupConstructorAssemblerForAuto();
            _propertyAssembler = new AutoPropertyAssembler(_componentDef);
        }

        private void SetupConstructorAssemblerForAuto()
        {
            if (_componentDef.ArgDefSize > 0)
            {
                _constructorAssembler = new ManualConstructorAssembler(_componentDef);
            }
            else if (_componentDef.Expression != null)
            {
                _constructorAssembler = new ExpressionConstructorAssembler(_componentDef);
            }
            else
            {
                _constructorAssembler = new AutoConstructorAssembler(_componentDef);
            }
        }

        private void SetupAssemblerForConstructor()
        {
            SetupConstructorAssemblerForAuto();
            _propertyAssembler = new ManualPropertyAssembler(_componentDef);
        }

        private void SetupAssemblerForProperty()
        {
            if (_componentDef.Expression != null)
            {
                _constructorAssembler =
                    new ExpressionConstructorAssembler(_componentDef);
            }
            else
            {
                _constructorAssembler = new ManualConstructorAssembler(_componentDef);
            }
            _propertyAssembler = new AutoPropertyAssembler(_componentDef);
        }

        private void SetupAssemblerForNone()
        {
            if (_componentDef.ArgDefSize > 0)
            {
                _constructorAssembler = new ManualConstructorAssembler(_componentDef);
            }
            else if (_componentDef.Expression != null)
            {
                _constructorAssembler = new ExpressionConstructorAssembler(_componentDef);
            }
            else
            {
                _constructorAssembler = new DefaultConstructorAssembler(_componentDef);
            }
            if (_componentDef.PropertyDefSize > 0)
            {
                _propertyAssembler = new ManualPropertyAssembler(_componentDef);
            }
            else
            {
                _propertyAssembler = new DefaultPropertyAssembler(_componentDef);
            }
        }
    }
}
