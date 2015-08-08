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
using Seasar.Framework.Container.Impl;
using Seasar.Framework.Container.Util;
using Seasar.Framework.Util;
using Seasar.Framework.Xml;

namespace Seasar.Framework.Container.Factory
{
    public class ComponentTagHandler : TagHandler
    {
        public override void Start(TagHandlerContext context, IAttributes attributes)
        {
            var className = attributes["class"];
            Type componentType = null;
            if (className != null)
            {
                var asms = AppDomain.CurrentDomain.GetAssemblies();
                componentType = ClassUtil.ForName(className, asms);
                if (componentType == null)
                    throw new ClassNotFoundRuntimeException(className);
            }
            var name = attributes["name"];
            IComponentDef componentDef = new ComponentDefImpl(componentType, name);
            var instanceMode = attributes["instance"];
            if (instanceMode != null)
            {
                componentDef.InstanceMode = instanceMode;
            }
            var autoBindingMode = attributes["autoBinding"];
            if (autoBindingMode != null)
            {
                componentDef.AutoBindingMode = autoBindingMode;
            }
            context.Push(componentDef);
        }

        public override void End(TagHandlerContext context, string body)
        {
            var componentDef = (IComponentDef) context.Pop();
            string expression = null;
            if (body != null)
            {
                expression = body.Trim();
                if (!StringUtil.IsEmpty(expression))
                {
                    componentDef.Expression = expression;
                }
                else
                {
                    expression = null;
                }
            }
            if (componentDef.ComponentType == null
                && !InstanceModeUtil.IsOuter(componentDef.InstanceMode)
                && expression == null)
            {
                throw new TagAttributeNotDefinedRuntimeException("component", "class");
            }
            if (context.Peek() is IS2Container)
            {
                var container = (IS2Container) context.Peek();
                container.Register(componentDef);
            }
            else
            {
                var argDef = (IArgDef) context.Peek();
                argDef.ChildComponentDef = componentDef;
            }
        }
    }
}
