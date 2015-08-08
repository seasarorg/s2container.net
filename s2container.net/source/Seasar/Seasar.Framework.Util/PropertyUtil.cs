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
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;

#if NHIBERNATE_NULLABLES
using Nullables;
#endif

namespace Seasar.Framework.Util
{
    public  static class PropertyUtil
    {
        public static Type GetPrimitiveType(Type type)
        {
            if (type.GetInterface(typeof(INullable).Name) != null)
            {
                var npi = type.GetProperty("Value");
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
                var nullable = (INullable) value;
                if (!nullable.IsNull)
                {
                    var npi = value.GetExType().GetProperty("Value");
//                    return npi.GetValue(value, null);
                    return GetValue(value, value.GetExType(), npi.Name);
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

        /// <summary>
        /// オブジェクトのプロパティに値をセットする
        /// </summary>
        /// <param name="target">対象オブジェクト</param>
        /// <param name="targetType">対象オブジェクト型</param>
        /// <param name="propertyName">プロパティ名</param>
        /// <param name="value">セットする値</param>
        /// <param name="propType">プロパティ型</param>
        public static void SetValue(object target, Type targetType, string propertyName, Type propType, object value)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (String.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            const CSharpBinderFlags binderFlags = CSharpBinderFlags.None;
            const CSharpArgumentInfoFlags argumentFlags = CSharpArgumentInfoFlags.None;

            var binder = Binder.SetMember(binderFlags, propertyName, targetType,
                new[]
                {
                    CSharpArgumentInfo.Create(argumentFlags, null),
                    CSharpArgumentInfo.Create(argumentFlags, null),
                });

            var callsite = CallSite<Func<CallSite, object, object, object>>.Create(binder);
            callsite.Target(callsite, target, value);
        }

        /// <summary>
        /// オブジェクトから値を取得する
        /// </summary>
        /// <param name="target">対象オブジェクト</param>
        /// <param name="targetType">対象オブジェクト型</param>
        /// <param name="propertyName">プロパティ名</param>
        /// <returns>取得した値</returns>
        public static object GetValue(object target, Type targetType, string propertyName)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (String.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            const CSharpBinderFlags binderFlags = CSharpBinderFlags.None;
            const CSharpArgumentInfoFlags argumentFlags = CSharpArgumentInfoFlags.None;

            var binder = Binder.GetMember(binderFlags, propertyName, targetType,
                new[]
                {
                    CSharpArgumentInfo.Create(argumentFlags, null)
                });

            var callsite = CallSite<Func<CallSite, object, object>>.Create(binder);
            return callsite.Target(callsite, target);
        }
    }
}
