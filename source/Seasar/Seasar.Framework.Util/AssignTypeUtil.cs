#region Copyright
/*
 * Copyright 2005-2008 the Seasar Foundation and the Others.
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
using System.Collections;
using System.Globalization;
using System.Collections.Generic;

namespace Seasar.Framework.Util
{
	public class AssignTypeUtil
	{
        /// <summary>
        /// 単体の値として利用できる型か判定する
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsSimpleType(Type type)
        {
            if (type == null)
            {
                throw new NullReferenceException("type");
            }

            if (type == typeof(string) || type.IsPrimitive
                || type == typeof(bool) || type ==  typeof(char)
                || type.IsValueType || type == typeof(DateTime)
                || type == typeof(Calendar) || type == typeof(byte[]))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Genericを除くIList型実装クラスかどうか判定する
        /// </summary>
        /// <param name="type"></param>
        /// <returns>true=IList実装,false=その他</returns>
        public static bool IsList(Type type)
        {
            if (!type.IsArray && !type.IsGenericType && typeof(IList).IsAssignableFrom(type))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Generic型かどうか判定
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsGenericList(Type type)
        {
            if (type.IsGenericType &&
                (type.GetGenericTypeDefinition().Equals(typeof(IList<>))
                || type.GetGenericTypeDefinition().Equals(typeof(List<>))))
            {
                return true;
            }
            return false;
        }

	}
}
