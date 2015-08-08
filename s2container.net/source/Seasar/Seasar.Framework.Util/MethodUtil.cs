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
using System.Reflection;
using System.Text;
using Seasar.Framework.Exceptions;

namespace Seasar.Framework.Util
{
    /// <summary>
    /// メソッドユーティリティクラス
    /// </summary>
    public static class MethodUtil
    {
        /// <summary>
        /// 生成したDelegateを保持するキャッシュ
        /// </summary>
        private static readonly Dictionary<string, Delegate> _cacheMethods = new Dictionary<string, Delegate>(); 

        /// <summary>
        /// メソッドを実行する
        /// </summary>
        /// <param name="method">実行メドッソ</param>
        /// <param name="target">実行オトクェジブ</param>
        /// <param name="args">引数</param>
        /// <returns>戻り値</returns>
        public static object Invoke(MethodInfo method, object target, object[] args)
        {
            try
            {
                Delegate d;
                string key;
                if (args == null || args.Length == 0)
                {
                    key = method.DeclaringType.GetExType() + @"#" + method.Name + "0";
                    if (!_cacheMethods.ContainsKey(key))
                    {
                        if (method.DeclaringType == typeof (void))
                            d = Delegate.CreateDelegate(typeof (Action), target, method.Name, false, true);
                        else 
                            d = Delegate.CreateDelegate(typeof(Func<object>), target, method.Name, false, true);

                        _cacheMethods.Add(key, d);
                        return d?.DynamicInvoke(null);
                    }
                    else
                    {
                        d = _cacheMethods[key];
                        return d.DynamicInvoke(null);
                    }
                }
                key = method.DeclaringType.GetExType() + @"#" + method.Name + Convert.ToString(args.Length);
                if (!_cacheMethods.ContainsKey(key))
                {
                    if (method.ReturnType == typeof (void))
                    {
                        switch (args.Length)
                        {
                            case 0:
                                d = Delegate.CreateDelegate(typeof(Action),target, method.Name, false, true);
                                break;
                            case 1:
                                d = Delegate.CreateDelegate(typeof(Action<object>), target, method.Name, false, true);
                                break;
                            case 2:
                                d = Delegate.CreateDelegate(typeof(Action<object, object>), target, method.Name, false, true);
                                break;
                            case 3:
                                d = Delegate.CreateDelegate(typeof(Action<object, object, object>), target, method.Name, false, true);
                                break;
                            case 4:
                                d = Delegate.CreateDelegate(typeof(Action<object, object, object, object>), target, method.Name, false, true);
                                break;
                            case 5:
                                d = Delegate.CreateDelegate(typeof(Action<object, object, object, object, object>), target, method.Name, false, true);
                                break;
                            case 6:
                                d = Delegate.CreateDelegate(typeof(Action<object, object, object, object, object, object>), target, method.Name, false, true);
                                break;
                            case 7:
                                d = Delegate.CreateDelegate(typeof(Action<object, object, object, object, object, object, object>), target, method.Name, false, true);
                                break;
                            case 8:
                                d = Delegate.CreateDelegate(typeof(Action<object, object, object, object, object, object, object, object>), target, method.Name, false, true);
                                break;
                            case 9:
                                d = Delegate.CreateDelegate(typeof(Action<object, object, object, object, object, object, object, object, object>), target, method.Name, false, true);
                                break;
                            case 10:
                                d = Delegate.CreateDelegate(typeof(Action<object, object, object, object, object, object, object, object, object, object>), target, method.Name, false, true);
                                break;
                            case 11:
                                d = Delegate.CreateDelegate(typeof(Action<object, object, object, object, object, object, object, object, object, object, object>), target, method.Name, false, true);
                                break;
                            case 12:
                                d = Delegate.CreateDelegate(typeof(Action<object, object, object, object, object, object, object, object, object, object, object, object>), target, method.Name, false, true);
                                break;
                            case 13:
                                d = Delegate.CreateDelegate(typeof(Action<object, object, object, object, object, object, object, object, object, object, object, object, object>), target, method.Name, false, true);
                                break;
                            case 14:
                                d = Delegate.CreateDelegate(typeof(Action<object, object, object, object, object, object, object, object, object, object, object, object, object, object>), target, method.Name, false, true);
                                break;
                            case 15:
                                d = Delegate.CreateDelegate(typeof(Action<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>), target, method.Name, false, true);
                                break;
                            default:
                                d = Delegate.CreateDelegate(typeof(Action<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>), target, method.Name, false, true);
                                break;
                        }
                    }
                    else
                    {
                        switch (args.Length)
                        {
                            case 0:
                                d = Delegate.CreateDelegate(typeof(Func<object>), target, method.Name, false, true);
                                break;
                            case 1:
                                d = Delegate.CreateDelegate(typeof(Func<object, object>), target, method.Name, false, true);
                                break;
                            case 2:
                                d = Delegate.CreateDelegate(typeof(Func<object, object, object>), target, method.Name, false, true);
                                break;
                            case 3:
                                d = Delegate.CreateDelegate(typeof(Func<object, object, object, object>), target, method.Name, false, true);
                                break;
                            case 4:
                                d = Delegate.CreateDelegate(typeof(Func<object, object, object, object, object>), target, method.Name, false, true);
                                break;
                            case 5:
                                d = Delegate.CreateDelegate(typeof(Func<object, object, object, object, object, object>), target, method.Name, false, true);
                                break;
                            case 6:
                                d = Delegate.CreateDelegate(typeof(Func<object, object, object, object, object, object, object>), target, method.Name, false, true);
                                break;
                            case 7:
                                d = Delegate.CreateDelegate(typeof(Func<object, object, object, object, object, object, object, object>), target, method.Name, false, true);
                                break;
                            case 8:
                                d = Delegate.CreateDelegate(typeof(Func<object, object, object, object, object, object, object, object, object>), target, method.Name, false, true);
                                break;
                            case 9:
                                d = Delegate.CreateDelegate(typeof(Func<object, object, object, object, object, object, object, object, object, object>), target, method.Name, false, true);
                                break;
                            case 10:
                                d = Delegate.CreateDelegate(typeof(Func<object, object, object, object, object, object, object, object, object, object, object>), target, method.Name, false, true);
                                break;
                            case 11:
                                d = Delegate.CreateDelegate(typeof(Func<object, object, object, object, object, object, object, object, object, object, object, object>), target, method.Name, false, true);
                                break;
                            case 12:
                                d = Delegate.CreateDelegate(typeof(Func<object, object, object, object, object, object, object, object, object, object, object, object, object>), target, method.Name, false, true);
                                break;
                            case 13:
                                d = Delegate.CreateDelegate(typeof(Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object>), target, method.Name, false, true);
                                break;
                            case 14:
                                d = Delegate.CreateDelegate(typeof(Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>), target, method.Name, false, true);
                                break;
                            case 15:
                                d = Delegate.CreateDelegate(typeof(Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>), target, method.Name, false, true);
                                break;
                            default:
                                d = Delegate.CreateDelegate(typeof(Func<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>), target, method.Name, false, true);
                                break;
                        }
                    }
                    _cacheMethods.Add(key, d);
                    return d?.DynamicInvoke(args);
                }
                else
                {
                    d = _cacheMethods[key];
                    return d.DynamicInvoke(args);
                }
            }
            catch (TargetInvocationException ex)
            {
                throw new InvocationTargetRuntimeException(method.DeclaringType, ex);
            }
            catch (TargetException ex)
            {
                throw new IllegalAccessRuntimeException(method.DeclaringType, ex);
            }
            catch (ArgumentException ex)
            {
                throw new IllegalAccessRuntimeException(method.DeclaringType, ex);
            }
            catch (TargetParameterCountException ex)
            {
                throw new IllegalAccessRuntimeException(method.DeclaringType, ex);
            }
            catch (MethodAccessException ex)
            {
                throw new IllegalAccessRuntimeException(method.DeclaringType, ex);
            }
        }

        /// <summary>
        /// 引数を取得する
        /// </summary>
        /// <param name="methodName">メソッド名</param>
        /// <param name="argTypes">引数タイプ</param>
        /// <returns>引数文字列</returns>
        public static string GetSignature(string methodName, Type[] argTypes)
        {
            var buf = new StringBuilder(100);
            buf.Append(methodName);
            buf.Append("(");
            if (argTypes != null)
            {
                for (var i = 0; i < argTypes.Length; ++i)
                {
                    if (i > 0) buf.Append(", ");
                    buf.Append(argTypes[i].FullName);
                }
            }
            buf.Append(")");
            return buf.ToString();
        }

        /// <summary>
        /// 引数を取得する
        /// </summary>
        /// <param name="methodName">メソッド名</param>
        /// <param name="methodArgs">引数オトクェジブ</param>
        /// <returns>引数文列字</returns>
        public static string GetSignature(string methodName, object[] methodArgs)
        {
            var buf = new StringBuilder(100);
            buf.Append(methodName);
            buf.Append("(");
            if (methodArgs != null)
            {
                for (var i = 0; i < methodArgs.Length; ++i)
                {
                    if (i > 0) buf.Append(", ");
                    buf.Append(methodArgs[i] != null ? methodArgs[i].GetExType().FullName : "null");
                }
            }
            buf.Append(")");
            return buf.ToString();
        }

        public static string[] GetParameterNames(MethodInfo mi)
        {
            var parameters = mi.GetParameters();
            var argNames = new string[parameters.Length];
            for (var i = 0; i < parameters.Length; ++i)
            {
                argNames[i] = parameters[i].Name;
            }
            return argNames;
        }

        public static Type[] GetParameterTypes(MethodInfo mi)
        {
            var parameters = mi.GetParameters();
            var argTypes = new Type[parameters.Length];
            for (var i = 0; i < parameters.Length; ++i)
            {
                argTypes[i] = parameters[i].ParameterType;
            }
            return argTypes;
        }

        /// <summary>
        /// Override可能かどうか判定する
        /// </summary>
        /// <param name="info"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static bool CanOverride(MethodBase info)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            if (info.DeclaringType != null && (info.DeclaringType.IsInterface ||
                                               info.IsAbstract ||
                                               info.IsVirtual))
            {
                return true;
            }
            return false;
        }
    }
}
