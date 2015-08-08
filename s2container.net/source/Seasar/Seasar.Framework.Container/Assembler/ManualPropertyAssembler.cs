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

using Seasar.Framework.Beans;
using Seasar.Framework.Util;

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
            var type = component.GetExType();
            var size = ComponentDef.PropertyDefSize;
            for (var i = 0; i < size; ++i)
            {
                var propDef = ComponentDef.GetPropertyDef(i);
                var propInfo = type.GetProperty(propDef.PropertyName);
                if (propInfo == null)
                {
                    throw new PropertyNotFoundRuntimeException(component.GetExType(),
                        propDef.PropertyName);
                }
                propDef.ArgType = propInfo.PropertyType;

                var value = GetComponentByReceiveType(propInfo.PropertyType, propDef.Expression);

                if (value == null)
                {
                    value = GetValue(propDef, component);
                }

                SetValue(propInfo, component, value);
            }
        }
    }
}
