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
using System.Reflection;
using Seasar.Framework.Exceptions;

namespace Seasar.Framework.Util
{
    public static class ConstructorUtil
    {
        public static object NewInstance(ConstructorInfo constructor, object[] args)
        {
            try
            {
                if (args == null || args.Length == 0)
                    return ClassUtil.NewInstance(constructor, constructor.DeclaringType);
                else
                {
                    object ret;
                    switch (args.Length)
                    {
                        case 1:
                            ret = ClassArgumentsUtil<object, object>.NewInstance(constructor, args[0]);
                            break;
                        case 2:
                            ret = ClassArgumentsUtil<object, object, object>.NewInstance(constructor, args[0], args[1]);
                            break;
                        case 3:
                            ret = ClassArgumentsUtil<object, object, object, object>.NewInstance(constructor, args[0], args[1], args[2]);
                            break;
                        case 4:
                            ret = ClassArgumentsUtil<object, object, object, object, object>.NewInstance
                                (constructor, args[0], args[1], args[2], args[3]);
                            break;
                        case 5:
                            ret = ClassArgumentsUtil<object, object, object, object, object, object>.NewInstance
                                (constructor, args[0], args[1], args[2], args[3], args[4]);
                            break;
                        default:
                            ret = ClassArgumentsUtil<object, object, object, object, object, object, object>.NewInstance
                                (constructor, args[0], args[1], args[2], args[3], args[4], args[5]);
                            break;
                    }
                    return ret;
                }
            }
            catch (MethodAccessException ex)
            {
                throw new IllegalAccessRuntimeException(constructor.DeclaringType, ex);
            }
            catch (ArgumentException ex)
            {
                throw new IllegalAccessRuntimeException(constructor.DeclaringType, ex);
            }
            catch (TargetInvocationException ex)
            {
                throw new InvocationTargetRuntimeException(constructor.DeclaringType, ex);
            }
            catch (TargetParameterCountException ex)
            {
                throw new IllegalAccessRuntimeException(constructor.DeclaringType, ex);
            }
        }

        public static object NewInstance<T>(ConstructorInfo constructor, object[] args)
        {
            try
            {
                if (args == null || args.Length == 0)
                    return ClassUtil.NewInstance(constructor, constructor.DeclaringType);
                else
                {
                    object ret;
                    switch (args.Length)
                    {
                        case 1:
                            ret = ClassArgumentsUtil<T, object>.NewInstance(constructor, (T)args[0]);
                            break;
                        case 2:
                            ret = ClassArgumentsUtil<T, T, object>.NewInstance(constructor, (T)args[0], (T)args[1]);
                            break;
                        case 3:
                            ret = ClassArgumentsUtil<T, T, T, object>.NewInstance(constructor, (T)args[0], (T)args[1], (T)args[2]);
                            break;
                        case 4:
                            ret = ClassArgumentsUtil<T, T, T, T, object>.NewInstance
                                (constructor, (T)args[0], (T)args[1], (T)args[2], (T)args[3]);
                            break;
                        case 5:
                            ret = ClassArgumentsUtil<T, T, T, T, T, object>.NewInstance
                                (constructor, (T)args[0], (T)args[1], (T)args[2], (T)args[3], (T)args[4]);
                            break;
                        default:
                            ret = ClassArgumentsUtil<T, T, T, T, T, T, object>.NewInstance
                                (constructor, (T)args[0], (T)args[1], (T)args[2], (T)args[3], (T)args[4], (T)args[5]);
                            break;
                    }
                    return ret;
                }
            }
            catch (MethodAccessException ex)
            {
                throw new IllegalAccessRuntimeException(constructor.DeclaringType, ex);
            }
            catch (ArgumentException ex)
            {
                throw new IllegalAccessRuntimeException(constructor.DeclaringType, ex);
            }
            catch (TargetInvocationException ex)
            {
                throw new InvocationTargetRuntimeException(constructor.DeclaringType, ex);
            }
            catch (TargetParameterCountException ex)
            {
                throw new IllegalAccessRuntimeException(constructor.DeclaringType, ex);
            }
        }

        public static object NewInstance<T1, T2>(ConstructorInfo constructor, object[] args)
        {
            try
            {
                if (args == null || args.Length == 0)
                    return ClassUtil.NewInstance(constructor, constructor.DeclaringType);
                else
                {
                    object ret;
                    switch (args.Length)
                    {
                        case 1:
                            ret = ClassArgumentsUtil<T1, object>.NewInstance(constructor, (T1)args[0]);
                            break;
                        case 2:
                            ret = ClassArgumentsUtil<T1, T2, object>.NewInstance(constructor, (T1)args[0], (T2)args[1]);
                            break;
                        case 3:
                            ret = ClassArgumentsUtil<T1, T1, T2, object>.NewInstance(constructor, (T1)args[0], (T1)args[1], (T2)args[2]);
                            break;
                        case 4:
                            ret = ClassArgumentsUtil<T1, T1, T1, T2, object>.NewInstance
                                (constructor, (T1)args[0], (T1)args[1], (T1)args[2], (T2)args[3]);
                            break;
                        case 5:
                            ret = ClassArgumentsUtil<T1, T1, T1, T1, T2, object>.NewInstance
                                (constructor, (T1)args[0], (T1)args[1], (T1)args[2], (T1)args[3], (T2)args[4]);
                            break;
                        default:
                            ret = ClassArgumentsUtil<T1, T1, T1, T1, T1, T2, object>.NewInstance
                                (constructor, (T1)args[0], (T1)args[1], (T1)args[2], (T1)args[3], (T1)args[4], (T2)args[5]);
                            break;
                    }
                    return ret;
                }
            }
            catch (MethodAccessException ex)
            {
                throw new IllegalAccessRuntimeException(constructor.DeclaringType, ex);
            }
            catch (ArgumentException ex)
            {
                throw new IllegalAccessRuntimeException(constructor.DeclaringType, ex);
            }
            catch (TargetInvocationException ex)
            {
                throw new InvocationTargetRuntimeException(constructor.DeclaringType, ex);
            }
            catch (TargetParameterCountException ex)
            {
                throw new IllegalAccessRuntimeException(constructor.DeclaringType, ex);
            }
        }

        public static object NewInstance<T1, T2, T3>(ConstructorInfo constructor, object[] args)
        {
            try
            {
                if (args == null || args.Length == 0)
                    return ClassUtil.NewInstance(constructor, constructor.DeclaringType);
                else
                {
                    object ret;
                    switch (args.Length)
                    {
                        case 1:
                            ret = ClassArgumentsUtil<T1, object>.NewInstance(constructor, (T1)args[0]);
                            break;
                        case 2:
                            ret = ClassArgumentsUtil<T1, T2, object>.NewInstance(constructor, (T1)args[0], (T2)args[1]);
                            break;
                        case 3:
                            ret = ClassArgumentsUtil<T1, T2, T3, object>.NewInstance(constructor, (T1)args[0], (T2)args[1], (T3)args[2]);
                            break;
                        case 4:
                            ret = ClassArgumentsUtil<T1, T1, T2, T3, object>.NewInstance
                                (constructor, (T1)args[0], (T1)args[1], (T2)args[2], (T3)args[3]);
                            break;
                        case 5:
                            ret = ClassArgumentsUtil<T1, T1, T1, T2, T3, object>.NewInstance
                                (constructor, (T1)args[0], (T1)args[1], (T1)args[2], (T2)args[3], (T3)args[4]);
                            break;
                        default:
                            ret = ClassArgumentsUtil<T1, T1, T1, T1, T2, T3, object>.NewInstance
                                (constructor, (T1)args[0], (T1)args[1], (T1)args[2], (T1)args[3], (T2)args[4], (T3)args[5]);
                            break;
                    }
                    return ret;
                }
            }
            catch (MethodAccessException ex)
            {
                throw new IllegalAccessRuntimeException(constructor.DeclaringType, ex);
            }
            catch (ArgumentException ex)
            {
                throw new IllegalAccessRuntimeException(constructor.DeclaringType, ex);
            }
            catch (TargetInvocationException ex)
            {
                throw new InvocationTargetRuntimeException(constructor.DeclaringType, ex);
            }
            catch (TargetParameterCountException ex)
            {
                throw new IllegalAccessRuntimeException(constructor.DeclaringType, ex);
            }
        }

        public static object NewInstance<T1, T2, T3, T4>(ConstructorInfo constructor, object[] args)
        {
            try
            {
                if (args == null || args.Length == 0)
                    return ClassUtil.NewInstance(constructor, constructor.DeclaringType);
                else
                {
                    object ret;
                    switch (args.Length)
                    {
                        case 1:
                            ret = ClassArgumentsUtil<T1, object>.NewInstance(constructor, (T1)args[0]);
                            break;
                        case 2:
                            ret = ClassArgumentsUtil<T1, T2, object>.NewInstance(constructor, (T1)args[0], (T2)args[1]);
                            break;
                        case 3:
                            ret = ClassArgumentsUtil<T1, T2, T3, object>.NewInstance(constructor, (T1)args[0], (T2)args[1], (T3)args[2]);
                            break;
                        case 4:
                            ret = ClassArgumentsUtil<T1, T2, T3, T4, object>.NewInstance
                                (constructor, (T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3]);
                            break;
                        case 5:
                            ret = ClassArgumentsUtil<T1, T1, T2, T3, T4, object>.NewInstance
                                (constructor, (T1)args[0], (T1)args[1], (T2)args[2], (T3)args[3], (T4)args[4]);
                            break;
                        default:
                            ret = ClassArgumentsUtil<T1, T1, T1, T2, T3, T4, object>.NewInstance
                                (constructor, (T1)args[0], (T1)args[1], (T1)args[2], (T2)args[3], (T3)args[4], (T4)args[5]);
                            break;
                    }
                    return ret;
                }
            }
            catch (MethodAccessException ex)
            {
                throw new IllegalAccessRuntimeException(constructor.DeclaringType, ex);
            }
            catch (ArgumentException ex)
            {
                throw new IllegalAccessRuntimeException(constructor.DeclaringType, ex);
            }
            catch (TargetInvocationException ex)
            {
                throw new InvocationTargetRuntimeException(constructor.DeclaringType, ex);
            }
            catch (TargetParameterCountException ex)
            {
                throw new IllegalAccessRuntimeException(constructor.DeclaringType, ex);
            }
        }

        public static object NewInstance<T1, T2, T3, T4, T5>(ConstructorInfo constructor, object[] args)
        {
            try
            {
                if (args == null || args.Length == 0)
                    return ClassUtil.NewInstance(constructor, constructor.DeclaringType);
                else
                {
                    object ret;
                    switch (args.Length)
                    {
                        case 1:
                            ret = ClassArgumentsUtil<T1, object>.NewInstance(constructor, (T1)args[0]);
                            break;
                        case 2:
                            ret = ClassArgumentsUtil<T1, T2, object>.NewInstance(constructor, (T1)args[0], (T2)args[1]);
                            break;
                        case 3:
                            ret = ClassArgumentsUtil<T1, T2, T3, object>.NewInstance(constructor, (T1)args[0], (T2)args[1], (T3)args[2]);
                            break;
                        case 4:
                            ret = ClassArgumentsUtil<T1, T2, T3, T4, object>.NewInstance
                                (constructor, (T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3]);
                            break;
                        case 5:
                            ret = ClassArgumentsUtil<T1, T2, T3, T4, T5, object>.NewInstance
                                (constructor, (T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4]);
                            break;
                        default:
                            ret = ClassArgumentsUtil<T1, T1, T2, T3, T4, T5, object>.NewInstance
                                (constructor, (T1)args[0], (T1)args[1], (T2)args[2], (T3)args[3], (T4)args[4], (T5)args[5]);
                            break;
                    }
                    return ret;
                }
            }
            catch (MethodAccessException ex)
            {
                throw new IllegalAccessRuntimeException(constructor.DeclaringType, ex);
            }
            catch (ArgumentException ex)
            {
                throw new IllegalAccessRuntimeException(constructor.DeclaringType, ex);
            }
            catch (TargetInvocationException ex)
            {
                throw new InvocationTargetRuntimeException(constructor.DeclaringType, ex);
            }
            catch (TargetParameterCountException ex)
            {
                throw new IllegalAccessRuntimeException(constructor.DeclaringType, ex);
            }
        }

        public static object NewInstance<T1, T2, T3, T4, T5, T6>(ConstructorInfo constructor, object[] args)
        {
            try
            {
                if (args == null || args.Length == 0)
                    return ClassUtil.NewInstance(constructor, constructor.DeclaringType);
                else
                {
                    object ret;
                    switch (args.Length)
                    {
                        case 1:
                            ret = ClassArgumentsUtil<T1, object>.NewInstance(constructor, (T1)args[0]);
                            break;
                        case 2:
                            ret = ClassArgumentsUtil<T1, T2, object>.NewInstance(constructor, (T1)args[0], (T2)args[1]);
                            break;
                        case 3:
                            ret = ClassArgumentsUtil<T1, T2, T3, object>.NewInstance(constructor, (T1)args[0], (T2)args[1], (T3)args[2]);
                            break;
                        case 4:
                            ret = ClassArgumentsUtil<T1, T2, T3, T4, object>.NewInstance
                                (constructor, (T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3]);
                            break;
                        case 5:
                            ret = ClassArgumentsUtil<T1, T2, T3, T4, T5, object>.NewInstance
                                (constructor, (T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4]);
                            break;
                        default:
                            ret = ClassArgumentsUtil<T1, T2, T3, T4, T5, T6, object>.NewInstance
                                (constructor, (T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4], (T6)args[5]);
                            break;
                    }
                    return ret;
                }
            }
            catch (MethodAccessException ex)
            {
                throw new IllegalAccessRuntimeException(constructor.DeclaringType, ex);
            }
            catch (ArgumentException ex)
            {
                throw new IllegalAccessRuntimeException(constructor.DeclaringType, ex);
            }
            catch (TargetInvocationException ex)
            {
                throw new InvocationTargetRuntimeException(constructor.DeclaringType, ex);
            }
            catch (TargetParameterCountException ex)
            {
                throw new IllegalAccessRuntimeException(constructor.DeclaringType, ex);
            }
        }

    }
}
