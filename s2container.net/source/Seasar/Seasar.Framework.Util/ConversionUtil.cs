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
    /// <summary>
    /// 型の変換を行うクラス
    /// </summary>
    public sealed class ConversionUtil
    {
        private ConversionUtil()
        {
        }

        /// <summary>
        /// 渡されたオブジェクトを目的のTypeに変換する
        /// </summary>
        /// <param name="o">変換するオブジェクト</param>
        /// <param name="targetType">目的のType</param>
        /// <returns>変換されたオブジェクト</returns>
        public static object ConvertTargetType(object o, Type targetType)
        {
            object ret;

            if (typeof(IConvertible).IsAssignableFrom(targetType))
            {
                ret = ConvertConvertible(o, targetType);
            }
#if !NET_1_1
            else if (targetType.IsGenericType &&
                targetType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                ret = ConvertNullable(o, targetType);
            }
#endif
            else if (typeof(INullable).IsAssignableFrom(targetType))
            {
                ret = ConvertSqlTypesNullable(o, targetType);
            }
#if NHIBERNATE_NULLABLES
            else if (typeof(INullableType).IsAssignableFrom(targetType))
            {
                ret = ConvertNHibernateNullable(o, targetType);
            }
#endif
            else if (o == DBNull.Value)
            {
                ret = null;
            }
            else
            {
                ret = o;
            }

            return ret;
        }

        /// <summary>
        /// オブジェクトをINullableを実装するTypeに変換する
        /// </summary>
        /// <param name="o">変換するオブジェクト</param>
        /// <param name="targetType">INullableを実装するType</param>
        /// <returns>変換されたオブジェクト</returns>
        public static object ConvertSqlTypesNullable(object o, Type targetType)
        {
            INullable ret;

            if (o == null || o == DBNull.Value)
            {
                ret = (INullable) targetType.InvokeMember("Null", BindingFlags.GetField,
                    null, null, null);
            }
            else
            {
                Type paramType = GetValueType(targetType);
                ret = (INullable) Activator.CreateInstance(targetType,
                    new object[] { Convert.ChangeType(o, paramType) });
            }

            return ret;
        }

#if NHIBERNATE_NULLABLES

        /// <summary>
        /// オブジェクトをINullableTypeを実装するTypeに変換する
        /// </summary>
        /// <param name="o">変換するオブジェクト</param>
        /// <param name="targetType">INullableTypeを実装するType</param>
        /// <returns>変換されたオブジェクト</returns>
        public static object ConvertNHibernateNullable(object o, Type targetType)
        {
            INullableType ret;

            if (o == null || o == DBNull.Value)
            {
                ret = (INullableType) targetType.InvokeMember("Default", BindingFlags.GetField,
                    null, null, null);
            }
            else
            {
                Type paramType = GetValueType(targetType);
                ret = (INullableType) Activator.CreateInstance(targetType,
                    new object[] { Convert.ChangeType(o, paramType) });
            }

            return ret;
        }
#endif

#if !NET_1_1
        /// <summary>
        /// オブジェクトをNullableジェネリック構造体に変換する
        /// </summary>
        /// <param name="o">変換するオブジェクト</param>
        /// <param name="targetType">Nullable ジェネリック構造体のType</param>
        /// <returns>変換されたオブジェクト</returns>
        public static object ConvertNullable(object o, Type targetType)
        {
            object ret;

            if (o == null || o == DBNull.Value)
            {
                ret = null;
            }
            else
            {
                Type paramType = GetValueType(targetType);
                ret = Activator.CreateInstance(targetType,
                    new object[] { Convert.ChangeType(o, paramType) });
            }

            return ret;
        }
#endif

        /// <summary>
        /// オブジェクトをIConvertibleを実装するTypeに変換する
        /// </summary>
        /// <param name="o">変換するオブジェクト</param>
        /// <param name="targetType">IConvertibleを実装するType</param>
        /// <returns>変換されたオブジェクト</returns>
        public static object ConvertConvertible(object o, Type targetType)
        {
            object ret = null;

            if (o == null || o == DBNull.Value)
            {
                if (!targetType.Equals(typeof(string)))
                {
                    ret = Convert.ChangeType(decimal.Zero, targetType);
                }
            }
            else
            {
                ret = Convert.ChangeType(o, targetType);
            }

            return ret;
        }

        /// <summary>
        /// プロパティ名が"Value"であるプロパティのTypeを返す
        /// </summary>
        /// <param name="targetType">Type</param>
        /// <returns></returns>
        private static Type GetValueType(Type targetType)
        {
            PropertyInfo pi = targetType.GetProperty("Value");
            return pi.PropertyType;
        }
    }
}
