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
using System.Reflection;
using Seasar.Framework.Beans;

namespace Seasar.Framework.Container.Assembler
{
    public class ManualPropertyAssembler : AbstractPropertyAssembler
    {
        public ManualPropertyAssembler(IComponentDef componentDef)
            : base(componentDef)
        {
        }

        public override void Assemble(object component)
        {
            Type type = component.GetType();
            int size = ComponentDef.PropertyDefSize;
            for (int i = 0; i < size; ++i)
            {
                IPropertyDef propDef = ComponentDef.GetPropertyDef(i);
                PropertyInfo propInfo = type.GetProperty(propDef.PropertyName);
                if (propInfo == null)
                {
                    throw new PropertyNotFoundRuntimeException(component.GetType(),
                        propDef.PropertyName);
                }
                propDef.ArgType = propInfo.PropertyType;

                object value = GetComponentByReceiveType(propInfo.PropertyType, propDef.Expression);

                if (value == null)
                {
                    value = GetValue(propDef, component);
                }

                SetValue(propInfo, component, value);
            }
        }
    }
}
