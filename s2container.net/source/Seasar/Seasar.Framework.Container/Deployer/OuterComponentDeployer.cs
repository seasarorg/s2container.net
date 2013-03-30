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
using Seasar.Framework.Exceptions;

namespace Seasar.Framework.Container.Deployer
{
    public class OuterComponentDeployer : AbstractComponentDeployer
    {
        public OuterComponentDeployer(IComponentDef componentDef)
            : base(componentDef)
        {
        }

        public override object Deploy(Type receiveType)
        {
            throw new NotSupportedException("Deploy");
        }

        public override void InjectDependency(object outerComponent)
        {
            CheckComponentType(outerComponent);
            PropertyAssembler.Assemble(outerComponent);
            InitMethodAssembler.Assemble(outerComponent);
        }

        private void CheckComponentType(object outerComponent)
        {
            Type componentType = ComponentDef.ComponentType;
            if (componentType == null)
            {
                return;
            }
            if (!componentType.IsAssignableFrom(outerComponent.GetType()))
            {
                throw new TypeUnmatchRuntimeException(componentType, outerComponent.GetType());
            }
        }
    }
}
