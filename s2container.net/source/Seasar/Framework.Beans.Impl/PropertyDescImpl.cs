#region Copyright
/*
 * Copyright 2005-2007 the Seasar Foundation and the Others.
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
using Seasar.Framework.Util;
using System.Text;

namespace Seasar.Framework.Beans.Impl
{
    public class PropertyDescImpl : IPropertyDesc
    {
        private PropertyInfo property;
        private IBeanDesc beanDesc;

        public PropertyDescImpl(PropertyInfo property, IBeanDesc beanDesc)
        {
            this.property = property;
            this.beanDesc = beanDesc;
        }

        #region IPropertyDesc ÉÅÉìÉo

        public string PropertyName
        {
            get { return property.Name; }
        }

        public Type propertyType
        {
            get { return property.PropertyType; }
        }

        public MethodInfo ReadMethod
        {
            get { return property.GetGetMethod(); }
        }

        public bool HasReadMethod
        {
            get { return property.CanRead; }
        }

        public MethodInfo WriteMethod
        {
            get {return property.GetSetMethod(); }
        }

        public bool HasWriteMethod
        {
            get { return property.CanWrite; }
        }

        public object GetValue(object target)
        {
            return property.GetValue(target, null);
        }

        public void SetValue(object target, object value)
        {
            try
            {
                property.SetValue(target, value, null);
            }
            catch (Exception ex)
            {
                throw new IllegalPropertyRuntimeException(
                    beanDesc.BeanType, property.Name, ex);
            }
        }

        #endregion

        public IBeanDesc BeanDesc
        {
            get { return beanDesc; }
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();

            buf.Append("propertyName=");
            buf.Append(property.Name);
            buf.Append(",propertyType=");
            buf.Append(property.PropertyType.FullName);
            buf.Append(",readMethod=");
            buf.Append(property.CanRead ? property.GetGetMethod().Name : "null");
            buf.Append(",writeMethod=");
            buf.Append(property.CanWrite ? property.GetSetMethod().Name : "null");

            return buf.ToString();
        }
    }
}
