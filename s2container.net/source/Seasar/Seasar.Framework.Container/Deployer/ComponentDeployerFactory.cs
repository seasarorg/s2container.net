#region Copyright
/*
 * Copyright 2005-2009 the Seasar Foundation and the Others.
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

using Seasar.Framework.Container.Util;

namespace Seasar.Framework.Container.Deployer
{
    public sealed class ComponentDeployerFactory
    {
        private ComponentDeployerFactory()
        {
        }

        public static IComponentDeployer Create(IComponentDef componentDef)
        {
            if (InstanceModeUtil.IsSingleton(componentDef.InstanceMode))
            {
                return new SingletonComponentDeployer(componentDef);
            }
            else if (InstanceModeUtil.IsPrototype(componentDef.InstanceMode))
            {
                return new PrototypeComponentDeployer(componentDef);
            }
            else if (InstanceModeUtil.IsRequest(componentDef.InstanceMode))
            {
                return new RequestComponentDeployer(componentDef);
            }
            else if (InstanceModeUtil.IsSession(componentDef.InstanceMode))
            {
                return new SessionComponentDeployer(componentDef);
            }
            else
            {
                return new OuterComponentDeployer(componentDef);
            }
        }
    }
}
