#region Copyright
/*
 * Copyright 2005-2009 the Seasar Foundation and the Others.
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
using Seasar.Framework.Exceptions;

namespace Seasar.Framework.Util
{
    /// <summary>
    /// Utility of array
    /// </summary>
    public sealed class ArrayUtil
    {
        private ArrayUtil()
        {
        }

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
            object[] newArray = (object[]) Array.CreateInstance(
                array.GetType().GetElementType(), array.Length + 1);
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
                    object[] array = (object[]) Array.CreateInstance(
                        a.GetType().GetElementType(), a.Length + b.Length);
                    Array.Copy(a, 0, array, 0, a.Length);
                    Array.Copy(b, 0, array, a.Length, b.Length);
                    return array;
                }
                else if (b.Length == 0)
                {
                    return a;
                }
                else
                {
                    return b;
                }
            }
            else if (b == null)
            {
                return a;
            }
            else
            {
                return b;
            }
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
                for (int i = 0; i < array.Length; ++i)
                {
                    char c = array[i];
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
            int index = IndexOf(array, obj);
            if (index < 0)
            {
                return array;
            }
            object[] newArray = (object[]) Array.CreateInstance(
                array.GetType(), array.Length - 1);
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

        public static bool IsEmpty(object[] arrays)
        {
            return (arrays == null || arrays.Length == 0);
        }

        public static bool Contains(object[] array, object obj)
        {
            return (-1 < IndexOf(array, obj));
        }

        public static bool Contains(char[] array, char ch)
        {
            return (-1 < IndexOf(array, ch));
        }
    }
}