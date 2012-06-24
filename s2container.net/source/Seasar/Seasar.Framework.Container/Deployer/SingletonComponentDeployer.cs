#region Copyright
/*
 * Copyright 2005-2012 the Seasar Foundation and the Others.
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
        private object component;
        private bool instantiating = false;

        public SingletonComponentDeployer(IComponentDef componentDef)
            : base(componentDef)
        {
        }

        public override object Deploy(Type receiveType)
        {
            lock (this)
            {
                if (component == null)
                {
                    Assemble();
                }
                object proxy = GetProxy(receiveType);
                return proxy == null ? component : proxy;
            }
        }

        public override void InjectDependency(object outerComponent)
        {
            throw new NotSupportedException("InjectDependency");
        }

        private void Assemble()
        {
            if (instantiating)
            {
                throw new CyclicReferenceRuntimeException(ComponentDef.ComponentType);
            }
            instantiating = true;
            component = ConstructorAssembler.Assemble();
            instantiating = false;
            PropertyAssembler.Assemble(component);
            InitMethodAssembler.Assemble(component);
        }

        public override void Init()
        {
            Deploy(ComponentDef.ComponentType);
        }

        public override void Destroy()
        {
            if (component == null)
            {
                return;
            }
            DestroyMethodAssembler.Assemble(component);
            component = null;
        }
    }
}
