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
using System.Data.SqlTypes;
using System.Reflection;

#if NHIBERNATE_NULLABLES
using Nullables;
#endif

namespace Seasar.Framework.Util
{
    public sealed class PropertyUtil
    {
        private PropertyUtil()
        {
        }

        public static Type GetPrimitiveType(Type type)
        {
            if (type.GetInterface(typeof(INullable).Name) != null)
            {
                PropertyInfo npi = type.GetProperty("Value");
                return npi.PropertyType;
            }
#if NHIBERNATE_NULLABLES
            else if (type.GetInterface(typeof(INullableType).Name) != null)
            {
                ConstructorInfo[] constructorInfos = type.GetConstructors();
                ParameterInfo[] parameterInfos = constructorInfos[0].GetParameters();
                type = parameterInfos[0].ParameterType;
            }
#endif
#if !NET_1_1
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = Nullable.GetUnderlyingType(type);
            }
#endif

            return type;
        }

        public static object GetPrimitiveValue(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return DBNull.Value;
            }

            if (value is INullable)
            {
                INullable nullable = (INullable) value;
                if (!nullable.IsNull)
                {
                    PropertyInfo npi = value.GetType().GetProperty("Value");
                    return npi.GetValue(value, null);
                }
                else
                {
                    return DBNull.Value;
                }
            }
#if NHIBERNATE_NULLABLES
            else if (value is INullableType)
            {
                INullableType nullableType = (INullableType)value;
                if (nullableType.HasValue)
                {
                    return nullableType.Value;
                }
                else
                {
                    return DBNull.Value;
                }
            }
#endif

            return value;
        }
    }
}
