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
using System.Collections;
using Seasar.Framework.Util;
using Seasar.Framework.Exceptions;

namespace Seasar.Framework.Beans.Impl
{
    public class BeanDesc
    {
        private Type beanType;
        private ConstructorInfo[] constructors;
        private IDictionary propertyDescCache = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
        private IDictionary methodCache = new Hashtable();
        private SortedList fieldCache = new SortedList();
        [NonSerialized]
        private HashSet invalidPropertyNames = new HashSet();
        private IDictionary constructorParameterNamesCache;
        private IDictionary methodParameterNamesCache;

        public BeanDesc(Type beanType)
        {
            if (beanType == null)
            {
                throw new EmptyRuntimeException("beanType");
            }

            this.beanType = beanType;
            constructors = beanType.GetConstructors();
            
            // TODO ‘±‚«
        }
    }
}
