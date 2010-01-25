#region Copyright
/*
 * Copyright 2005-2010 the Seasar Foundation and the Others.
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
    public abstract class AbstractPropertyAssembler
        : AbstractAssembler, IPropertyAssembler
    {
        public AbstractPropertyAssembler(IComponentDef componentDef)
            : base(componentDef)
        {
        }

        protected object GetValue(IPropertyDef propertyDef, object component)
        {
            try
            {
                return propertyDef.Value;
            }
            catch (ComponentNotFoundRuntimeException cause)
            {
                throw new IllegalPropertyRuntimeException(
                    GetComponentType(component), propertyDef.PropertyName, cause);
            }
        }

        protected void SetValue(PropertyInfo propertyInfo, object component, object value)
        {
            if (value == null)
            {
                return;
            }
            try
            {
                propertyInfo.SetValue(component, value, null);
            }
            catch (Exception ex)
            {
                throw new IllegalPropertyRuntimeException(
                    ComponentDef.ComponentType, propertyInfo.Name, ex);
            }
        }

        #region PropertyAssembler ÉÅÉìÉo

        public virtual void Assemble(object component)
        {
        }

        #endregion
    }
}
