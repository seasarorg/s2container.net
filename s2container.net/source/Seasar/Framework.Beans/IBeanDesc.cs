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

namespace Seasar.Framework.Beans
{
    public interface IBeanDesc
    {
        Type BeanType { get; }

        bool HasPropertyDesc(string propertyName);

        IPropertyDesc GetPropertyDesc(string propertyName);

        IPropertyDesc GetPropertyDesc(int index);

        int PropertyDescSize { get; }

        bool HasField(string fieldName);

        FieldInfo GetField(string fieldName);

        FieldInfo GetField(int index);

        object GetFieldValue(string fieldName, object target);

        int FieldSize { get; }

        object newInstance(object[] args);

        ConstructorInfo GetSuitableConstructor(object args);

        ConstructorInfo GetConstructor(Type[] paramTypes);

        string[] GetConstructorParameterNames(Type[] paramTypes);

        string[] GetConstructorParameterNames(ConstructorInfo constructor);

        object Invoke(object target, string methodName, object[] args);

        MethodInfo GetMethod(string methodName);

        MethodInfo GetMethod(string methodName, Type[] paramTypes);

        MethodInfo GetMethodNoException(string methodName);

        MethodInfo GetMethodNoException(string methodName, Type[] paramTypes);

        MethodInfo[] GetMethods(string methodName);

        bool HasMethod(string methodName);

        string[] MethodNames { get; }

        string[] GetMethodParameterNames(string methodName, Type[] paramTypes);

        string[] GetMethodParameterNamesNoException(
            string methodName, Type[] paramTypes);

        string[] GetMethodParameterNames(MethodInfo method);

        string[] GetMethodParameterNamesNoException(MethodInfo method);
    }
}
