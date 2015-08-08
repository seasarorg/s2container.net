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
using System.Collections.Generic;
using System.Linq.Expressions;
using Seasar.Framework.Exceptions;

namespace Seasar.Framework.Util
{
    /// <summary>
    /// Utility of array
    /// </summary>
    public static class ArrayUtil
    {
        // 式木のキャッシュ
        private static readonly Dictionary<Type, Func<Array>> _arrayCache = new Dictionary<Type, Func<Array>>();

        /// <summary>
        /// Add an item to array
        /// </summary>
        /// <param name="array"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object[] Add(object[] array, object obj)
        {
            if (array == null)
            {
                throw new EmptyRuntimeException("array");
            }
//            var newArray = (object[]) Array.CreateInstance(array.GetExType().GetElementType(), array.Length + 1);
            var newArray = (object[]) NewInstance(array.GetExType().GetElementType(), array.Length + 1);
            Array.Copy(array, 0, newArray, 0, array.Length);
            newArray[array.Length] = obj;
            return newArray;
        }

        /// <summary>
        /// Add an array to another array
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static object[] Add(object[] a, object[] b)
        {
            if (a != null && b != null)
            {
                if (a.Length != 0 && b.Length != 0)
                {
//                    var array = (object[]) Array.CreateInstance(a.GetExType().GetElementType(), a.Length + b.Length);
                    var array = (object[])NewInstance(a.GetExType().GetElementType(), a.Length + b.Length);
                    Array.Copy(a, 0, array, 0, a.Length);
                    Array.Copy(b, 0, array, a.Length, b.Length);
                    return array;
                }
                return b.Length == 0 ? a : b;
            }
            return b ?? a;
        }

        /// <summary>
        /// Index of array which is object
        /// </summary>
        /// <param name="array"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int IndexOf(object[] array, object obj)
        {
            if (array != null)
            {
                return Array.IndexOf(array, obj);
            }
            return -1;
        }

        /// <summary>
        /// Index of array which is char
        /// </summary>
        /// <param name="array"></param>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static int IndexOf(char[] array, char ch)
        {
            if (array != null)
            {
                for (var i = 0; i < array.Length; ++i)
                {
                    var c = array[i];
                    if (ch == c)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// Remove an object from array
        /// </summary>
        /// <param name="array"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object[] Remove(object[] array, object obj)
        {
            var index = IndexOf(array, obj);
            if (index < 0)
            {
                return array;
            }
            var newArray = (object[])NewInstance(array.GetExType().GetElementType(), array.Length - 1);
//            var newArray = (object[]) Array.CreateInstance(array.GetExType(), array.Length - 1);
            if (index > 0)
            {
                Array.Copy(array, 0, newArray, 0, index);
            }
            if (index < array.Length - 1)
            {
                Array.Copy(array, index + 1, newArray, index,
                    newArray.Length - index);
            }
            return newArray;
        }

        public static bool IsEmpty(object[] arrays) => (arrays == null || arrays.Length == 0);

        public static bool Contains(object[] array, object obj) => (-1 < IndexOf(array, obj));

        public static bool Contains(char[] array, char ch) => (-1 < IndexOf(array, ch));

        /// <summary>
        /// 配列インスタンスを生成する
        /// </summary>
        /// <param name="type">生成する型</param>
        /// <param name="length">配列の長さ</param>
        /// <returns>配列インスタンス</returns>
        public static Array NewInstance(Type type, params long[] length)
        {
            Func<Array> lambda;
            if (!_arrayCache.ContainsKey(type))
            {
                lambda = _CreateExpression(type);
                // 生成した式木はキャッシュに保存
                _arrayCache.Add(type, lambda);
            }
            else
            {
                lambda = _arrayCache[type];
            }
            return (Array)lambda.DynamicInvoke(null);
//            return Array.CreateInstance(type, length);
        }

        /// <summary>
        /// インスタンス化する式木(Expression)を作成する
        /// </summary>
        /// <param name="type">インスタンス化する型</param>
        /// <param name="length">配列の長さ</param>
        /// <returns>コンパイルした式木</returns>
        private static Func<Array> _CreateExpression(Type type, params long[] length)
        {
            var expressions = new Expression[length.Length];
            for (var i = 0; i < length.Length; i++)
            {
                expressions[i] = Expression.Constant(length[i]);
            }

            return Expression.Lambda<Func<Array>>(Expression.NewArrayBounds(type, expressions)).Compile();
        }
    }
}
