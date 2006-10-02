using System;
using System.Collections;
using System.Text;
using Seasar.Framework.Exceptions;

namespace Seasar.Framework.Util
{
    /// <summary>
    /// Utility of array
    /// </summary>
	public class ArrayUtil
	{
        private ArrayUtil() {
        }

        /// <summary>
        /// Add an item to array
        /// </summary>
        /// <param name="array"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Object[] Add(Object[] array, Object obj){
            if( array == null ){
                throw new EmptyRuntimeException("array");
            }
            Object[] newArray = (Object[])Array.CreateInstance(
                array.GetType().GetElementType(), array.Length + 1);
            Array.Copy(array, 0, newArray, 0, array.Length);
            newArray[array.Length] = (Seasar.Framework.Aop.IMethodInterceptor)obj;
            return newArray;
        }

        /// <summary>
        /// Add an array to another array
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Object[] Add(Object[] a, Object[] b) {
            if (a != null && b != null) {
                if (a.Length != 0 && b.Length != 0) {
                    Object[] array = (Object[])Array.CreateInstance(
                        a.GetType().GetElementType(), a.Length + b.Length);
                    Array.Copy(a, 0, array, 0, a.Length);
                    Array.Copy(b, 0, array, a.Length, b.Length);
                    return array;
                } else if (b.Length == 0) {
                    return a;
                } else {
                    return b;
                }
            } else if (b == null) {
                return a;
            } else {
                return b;
            }
        }

        /// <summary>
        /// Index of array which is object
        /// </summary>
        /// <param name="array"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int IndexOf(Object[] array, Object obj) {
            if (array != null) {
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
        public static int IndexOf(char[] array, char ch) {
            if (array != null) {
                for (int i = 0; i < array.Length; ++i) {
                    char c = array[i];
                    if (ch == c) {
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
        public static Object[] Remove(Object[] array, Object obj) {
            int index = IndexOf(array, obj);
            if (index < 0) {
                return array;
            }
            Object[] newArray = (Object[])Array.CreateInstance(
                array.GetType(), array.Length - 1);
            if (index > 0) {
                Array.Copy(array, 0, newArray, 0, index);
            }
            if (index < array.Length - 1) {
                Array.Copy(array, index + 1, newArray, index,
                    newArray.Length - index);
            }
            return newArray;
        }

        public static bool IsEmpty(Object[] arrays) {
            return (arrays == null || arrays.Length == 0);
        }

        public static bool Contains(Object[] array, Object obj) {
            return (-1 < IndexOf(array, obj));
        }

        public static bool Contains(char[] array, char ch) {
            return (-1 < IndexOf(array, ch));
        }
	}
}