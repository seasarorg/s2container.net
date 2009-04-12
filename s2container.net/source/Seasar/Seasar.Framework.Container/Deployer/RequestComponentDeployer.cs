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

using System;
using System.Reflection;
using System.Web;
using Seasar.Framework.Exceptions;
using Seasar.Framework.Log;
using Seasar.Framework.Util;

namespace Seasar.Framework.Container.Deployer
{
    public class RequestComponentDeployer : AbstractComponentDeployer
    {
        private static readonly Logger _logger = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public RequestComponentDeployer(IComponentDef componentDef)
            : base(componentDef)
        {
        }

        public override object Deploy(Type receiveType)
        {
            IComponentDef cd = ComponentDef;
            HttpContext context = cd.Container.Root.HttpContext;
            if (context == null)
            {
                ApplicationException ae = new EmptyRuntimeException("HttpContext");
                _logger.Log(ae);
                throw ae;
            }
            string componentName = cd.ComponentName;
            if (componentName == null)
            {
                componentName = cd.ComponentType.Name;
                componentName = StringUtil.Decapitalize(componentName);
            }
            object component = context.Items[componentName];

            if (component != null)
            {
                return component;
            }

            component = ConstructorAssembler.Assemble();

            object proxy = GetProxy(receiveType);

            if (proxy == null)
            {
                context.Items[componentName] = component;
            }
            else
            {
                context.Items[componentName] = proxy;
            }

            PropertyAssembler.Assemble(component);
            InitMethodAssembler.Assemble(component);

            if (proxy == null)
            {
                return component;
            }
            else
            {
                return proxy;
            }
        }

        public override void InjectDependency(object component)
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
