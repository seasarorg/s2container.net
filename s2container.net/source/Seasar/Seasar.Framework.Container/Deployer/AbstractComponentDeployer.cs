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
using Seasar.Framework.Container.Assembler;
using Seasar.Framework.Container.Util;

namespace Seasar.Framework.Container.Deployer
{
    public class AbstractComponentDeployer : IComponentDeployer
    {
        private IConstructorAssembler _constructorAssembler;
        private IPropertyAssembler _propertyAssembler;
        private IMethodAssembler _initMethodAssembler;
        private IMethodAssembler _destroyMethodAssembler;

        public AbstractComponentDeployer(IComponentDef componentDef)
        {
            ComponentDef = componentDef;
            _SetupAssembler();
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
            return receiveType == null ? null : ComponentDef.GetProxy(receiveType);
        }

        protected IComponentDef ComponentDef { get; }

        protected IConstructorAssembler ConstructorAssembler => _constructorAssembler;

        protected IPropertyAssembler PropertyAssembler => _propertyAssembler;

        protected IMethodAssembler InitMethodAssembler => _initMethodAssembler;

        protected IMethodAssembler DestroyMethodAssembler => _destroyMethodAssembler;

        private void _SetupAssembler()
        {
            var autoBindingMode = ComponentDef.AutoBindingMode;
            if (AutoBindingUtil.IsAuto(autoBindingMode))
            {
                _SetupAssemblerForAuto();
            }
            else if (AutoBindingUtil.IsConstructor(autoBindingMode))
            {
                _SetupAssemblerForConstructor();
            }
            else if (AutoBindingUtil.IsProperty(autoBindingMode))
            {
                _SetupAssemblerForProperty();
            }
            else if (AutoBindingUtil.IsNone(autoBindingMode))
            {
                _SetupAssemblerForNone();
            }
            else
            {
                throw new ArgumentException(autoBindingMode);
            }
            _initMethodAssembler = new DefaultInitMethodAssembler(ComponentDef);
            _destroyMethodAssembler = new DefaultDestroyMethodAssembler(ComponentDef);
        }

        private void _SetupAssemblerForAuto()
        {
            _SetupConstructorAssemblerForAuto();
            _propertyAssembler = new AutoPropertyAssembler(ComponentDef);
        }

        private void _SetupConstructorAssemblerForAuto()
        {
            if (ComponentDef.ArgDefSize > 0)
            {
                _constructorAssembler = new ManualConstructorAssembler(ComponentDef);
            }
            else if (ComponentDef.Expression != null)
            {
                _constructorAssembler = new ExpressionConstructorAssembler(ComponentDef);
            }
            else
            {
                _constructorAssembler = new AutoConstructorAssembler(ComponentDef);
            }
        }

        private void _SetupAssemblerForConstructor()
        {
            _SetupConstructorAssemblerForAuto();
            _propertyAssembler = new ManualPropertyAssembler(ComponentDef);
        }

        private void _SetupAssemblerForProperty()
        {
            if (ComponentDef.Expression != null)
            {
                _constructorAssembler =
                    new ExpressionConstructorAssembler(ComponentDef);
            }
            else
            {
                _constructorAssembler = new ManualConstructorAssembler(ComponentDef);
            }
            _propertyAssembler = new AutoPropertyAssembler(ComponentDef);
        }

        private void _SetupAssemblerForNone()
        {
            if (ComponentDef.ArgDefSize > 0)
            {
                _constructorAssembler = new ManualConstructorAssembler(ComponentDef);
            }
            else if (ComponentDef.Expression != null)
            {
                _constructorAssembler = new ExpressionConstructorAssembler(ComponentDef);
            }
            else
            {
                _constructorAssembler = new DefaultConstructorAssembler(ComponentDef);
            }
            if (ComponentDef.PropertyDefSize > 0)
            {
                _propertyAssembler = new ManualPropertyAssembler(ComponentDef);
            }
            else
            {
                _propertyAssembler = new DefaultPropertyAssembler(ComponentDef);
            }
        }
    }
}
