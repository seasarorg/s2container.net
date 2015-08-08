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

namespace Seasar.Framework.Container.Deployer
{
    public class SingletonComponentDeployer : AbstractComponentDeployer
    {
        private object _component;
        private bool _instantiating = false;

        public SingletonComponentDeployer(IComponentDef componentDef)
            : base(componentDef)
        {
        }

        public override object Deploy(Type receiveType)
        {
            lock (this)
            {
                if (_component == null)
                {
                    _Assemble();
                }
                var proxy = GetProxy(receiveType);
                return proxy ?? _component;
            }
        }

        public override void InjectDependency(object outerComponent)
        {
            throw new NotSupportedException("InjectDependency");
        }

        private void _Assemble()
        {
            if (_instantiating)
            {
                throw new CyclicReferenceRuntimeException(ComponentDef.ComponentType);
            }
            _instantiating = true;
            _component = ConstructorAssembler.Assemble();
            _instantiating = false;
            PropertyAssembler.Assemble(_component);
            InitMethodAssembler.Assemble(_component);
        }

        public override void Init()
        {
            Deploy(ComponentDef.ComponentType);
        }

        public override void Destroy()
        {
            if (_component == null)
            {
                return;
            }
            DestroyMethodAssembler.Assemble(_component);
            _component = null;
        }
    }
}
