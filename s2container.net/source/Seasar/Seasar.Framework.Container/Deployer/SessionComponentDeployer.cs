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
using Seasar.Framework.Exceptions;

namespace Seasar.Framework.Container.Deployer
{
    public class SessionComponentDeployer : AbstractComponentDeployer
    {
        public SessionComponentDeployer(IComponentDef componentDef)
            : base(componentDef)
        {
        }

        public override object Deploy(Type receiveType)
        {
            var cd = ComponentDef;
            var session = cd.Container.Root.Session;
            if (session == null)
            {
                throw new EmptyRuntimeException("session");
            }
            var componentName = cd.ComponentName;
            if (componentName == null)
            {
                throw new EmptyRuntimeException("componentName");
            }

            var component = session[componentName];

            if (component != null)
            {
                return component;
            }

            component = ConstructorAssembler.Assemble();

            var proxy = GetProxy(receiveType);

            if (proxy == null)
            {
                session[componentName] = component;
            }
            else
            {
                session[componentName] = proxy;
            }

            PropertyAssembler.Assemble(component);
            InitMethodAssembler.Assemble(component);

            return proxy ?? component;
        }

        public override void InjectDependency(object outerComponent)
        {
            throw new NotSupportedException("InjectDependency");
        }

        public override void Init()
        {
        }

        public override void Destroy()
        {
        }
    }
}
