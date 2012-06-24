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
using System.Reflection;
using System.Runtime.Remoting;
using Seasar.Framework.Aop.Proxy;
using Seasar.Framework.Log;
using Seasar.Framework.Container.Util;

namespace Seasar.Framework.Container.Assembler
{
    public class AutoPropertyAssembler : AbstractPropertyAssembler
    {
        private static readonly Logger _logger = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public AutoPropertyAssembler(IComponentDef componentDef)
            : base(componentDef)
        {
        }

        public override void Assemble(object component)
        {
            Type type = component.GetType();
            if (RemotingServices.IsTransparentProxy(component))
            {
                AopProxy aopProxy = RemotingServices.GetRealProxy(component) as AopProxy;
                if (aopProxy != null)
                {
                    type = aopProxy.TargetType;
                }
            }

            IS2Container container = ComponentDef.Container;
            foreach (PropertyInfo property in type.GetProperties())
            {
                object value;
                string propName = property.Name;
                if (ComponentDef.HasPropertyDef(propName))
                {
                    IPropertyDef propDef = ComponentDef.GetPropertyDef(propName);

                    value = GetComponentByReceiveType(property.PropertyType, propDef.Expression);

                    if (value == null) value = GetValue(propDef, component);

                    SetValue(property, component, value);
                }
                else if (property.CanWrite
                    && AutoBindingUtil.IsSuitable(property.PropertyType, component, propName))
                {
                    if (container.HasComponentDef(property.PropertyType))
                    {
                        value = container.GetComponent(property.PropertyType);
                    }
                    else
                    {
                        if (property.CanRead
                            && property.GetValue(component, null) != null)
                        {
                            continue;
                        }
                        _logger.Log("WSSR0008",
                            new object[] { GetComponentType(component).FullName, propName });
                        continue;
                    }
                    SetValue(property, component, value);
                }
            }
        }
    }
}
