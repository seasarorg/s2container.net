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

using System.Reflection;
using Seasar.Framework.Container.Util;
using Seasar.Framework.Util;

namespace Seasar.Framework.Container.Assembler
{
    public class AutoConstructorAssembler : AbstractConstructorAssembler
    {
        public AutoConstructorAssembler(IComponentDef componentDef)
            : base(componentDef)
        {
        }

        public override object Assemble()
        {
            var constructor = _GetSuitableConstructor();
            var args = new object[0];

            if (constructor == null)
            {
                if (!ComponentDef.ComponentType.IsInterface)
                {
                    return AssembleDefault();
                }
            }
            else
            {
                args = GetArgs(ParameterUtil.GetParameterTypes(constructor.GetParameters()));
            }
            return AopProxyUtil.WeaveAspect(ComponentDef, constructor, args);
        }

        private ConstructorInfo _GetSuitableConstructor()
        {
            var argSize = -1;
            ConstructorInfo constructor = null;
            var constructors = ComponentDef.ComponentType.GetConstructors();
            for (var i = 0; i < constructors.Length; ++i)
            {
                var tempArgSize = constructors[i].GetParameters().Length;
                if (tempArgSize == 0) return null;
                if (tempArgSize > argSize
                    && AutoBindingUtil.IsSuitable(constructors[i].GetParameters()))
                {
                    constructor = constructors[i];
                    argSize = tempArgSize;
                }
            }
            return constructor;
        }
    }
}
