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

namespace Seasar.Framework.Beans
{
    /// <summary>
    /// リフレクション情報を扱うためのインターフェースです。
    /// </summary>
    public interface IBeanDesc
    {
        /// <summary>
        /// 保持しているリフレクション情報の対象型
        /// </summary>
        Type BeanType { get; }

        /// <summary>
        /// PropertyDescを持っているかどうかを返します。
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns>PropertyDescを持っているかどうか</returns>
        bool HasProperty(string propertyName);

        /// <summary>
        /// PropertyDescを持っているかどうかを返します。
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="bindingFlags"></param>
        /// <returns>PropertyDescを持っているかどうか</returns>
        bool HasProperty(string propertyName, BindingFlags bindingFlags);

        /// <summary>
        /// PropertyDescを返します。
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns>PropertyDesc}</returns>
        /// <exception cref="PropertyNotFoundRuntimeException">
        /// PropertyDescが見つからない場合
        /// </exception>
        IPropertyDesc GetPropertyDesc(string propertyName);

        /// <summary>
        /// PropertyDescを返します。
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="bindingFlags"></param>
        /// <returns>PropertyDesc}</returns>
        /// <exception cref="PropertyNotFoundRuntimeException">
        /// PropertyDescが見つからない場合
        /// </exception>
        IPropertyDesc GetPropertyDesc(string propertyName, BindingFlags bindingFlags);

        /// <summary>
        /// PropertyDescを返します。
        /// </summary>
        /// <param name="index"></param>
        /// <returns>PropertyDesc}</returns>
        IPropertyDesc GetPropertyDesc(int index);

        /// <summary>
        /// PropertyDescを返します。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="bindingFlags"></param>
        /// <returns>PropertyDesc}</returns>
        IPropertyDesc GetPropertyDesc(int index, BindingFlags bindingFlags);

        /// <summary>
        /// PropertyDescを返します。
        /// </summary>
        /// <returns>PropertyDesc}</returns>
        IPropertyDesc[] GetPropertyDescs();

        /// <summary>
        /// PropertyDescを返します。
        /// </summary>
        /// <param name="bindingFlags"></param>
        /// <returns>PropertyDesc}</returns>
        IPropertyDesc[] GetPropertyDescs(BindingFlags bindingFlags);

        /// <summary>
        /// フィールド情報を持っているかどうかを返します。
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns>PropertyInfoを持っているかどうか</returns>
        bool HasField(string fieldName);

        /// <summary>
        /// フィールド情報を持っているかどうかを返します。
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="bindingFlags"></param>
        /// <returns>PropertyInfoを持っているかどうか</returns>
        bool HasField(string fieldName, BindingFlags bindingFlags);

        /// <summary>
        /// FieldDescを返します。
        /// </summary>
        /// <param name="fieldName">フィールド名</param>
        /// <returns>フィールド情報</returns>
        /// <exception cref="FieldNotFoundRuntimeException"></exception>
        IFieldDesc GetFieldDesc(string fieldName);

        /// <summary>
        /// FieldDescを返します。
        /// </summary>
        /// <param name="fieldName">フィールド名</param>
        /// <param name="bindingFlags"></param>
        /// <returns>フィールド情報</returns>
        /// <exception cref="FieldNotFoundRuntimeException"></exception>
        IFieldDesc GetFieldDesc(string fieldName, BindingFlags bindingFlags);

        /// <summary>
        /// FieldDescを返します。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IFieldDesc GetFieldDesc(int index);

        /// <summary>
        /// FieldDescを返します。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        IFieldDesc GetFieldDesc(int index, BindingFlags bindingFlags);

        /// <summary>
        /// FieldDescを返します。
        /// </summary>
        /// <returns></returns>
        IFieldDesc[] GetFieldDescs();

        /// <summary>
        /// FieldDescを返します。
        /// </summary>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        IFieldDesc[] GetFieldDescs(BindingFlags bindingFlags);

        /// <summary>
        /// 型に応じたConstructorInfoを返します。
        /// </summary>
        /// <param name="paramTypes"></param>
        /// <remarks>
        /// コンストラクタ情報が使われるのは現時点ではQuillくらいのため、
        /// ConstructorDescのようなものは作らずに
        /// ConstructorInfoをそのまま返します。
        /// </remarks>
        /// <returns>型に応じたConstructorInfo}</returns>
        /// <exception cref="ConstructorNotFoundRuntimeException"></exception>
        ConstructorInfo GetConstructor(Type[] paramTypes);

        /// <summary>
        /// メソッド情報があるかどうか返します。（引数に関係なくメソッド名のみで判定します）
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns>MethodInfoがあるかどうか</returns>
        bool HasMethod(string methodName);

        /// <summary>
        /// メソッド情報があるかどうか返します。
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="parameterTypes"></param>
        /// <returns>MethodInfoがあるかどうか</returns>
        bool HasMethod(string methodName, Type[] parameterTypes);

        /// <summary>
        /// メソッド情報があるかどうか返します。（引数に関係なくメソッド名のみで判定します）
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="bindingFlags"></param>
        /// <returns>MethodInfoがあるかどうか</returns>
        bool HasMethod(string methodName, BindingFlags bindingFlags);

        /// <summary>
        /// メソッド情報があるかどうか返します。
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="bindingFlags"></param>
        /// <returns>MethodInfoがあるかどうか</returns>
        bool HasMethod(string methodName, Type[] parameterTypes, BindingFlags bindingFlags);

        /// <summary>
        /// メソッド情報を返します。引数なしのメソッドを探します。
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        /// <exception cref="MethodNotFoundRuntimeException">
        /// MethodInfoが見つからない場合。
        /// </exception>
        IMethodDesc GetMethodDesc(string methodName);

        /// <summary>
        /// メソッド情報を返します。引数なしのメソッドを探します。
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        /// <exception cref="MethodNotFoundRuntimeException">
        /// MethodInfoが見つからない場合。
        /// </exception>
        /// 
        IMethodDesc GetMethodDesc(string methodName, BindingFlags bindingFlags);

        /// <summary>
        /// メソッド情報を返します。
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="paramTypes"></param>
        /// <returns>MethodInfo}</returns>
        /// <exception cref="MethodNotFoundRuntimeException">
        /// MethodInfoが見つからない場合。
        /// </exception>
        IMethodDesc GetMethodDesc(string methodName, Type[] paramTypes);

        /// <summary>
        /// メソッド情報を返します。
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="paramTypes"></param>
        /// <param name="bindingFlags"></param>
        /// <returns>MethodInfo}</returns>
        /// <exception cref="MethodNotFoundRuntimeException">
        /// MethodInfoが見つからない場合。
        /// </exception>
        IMethodDesc GetMethodDesc(string methodName, Type[] paramTypes, BindingFlags bindingFlags);

        /// <summary>
        /// メソッド情報の配列を返します。
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MethodNotFoundRuntimeException">
        /// MethodInfoが見つからない場合。
        /// </exception>
        IMethodDesc[] GetMethodDescs();

        /// <summary>
        /// メソッド情報の配列を返します。
        /// </summary>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        /// <exception cref="MethodNotFoundRuntimeException">
        /// MethodInfoが見つからない場合。
        /// </exception>
        IMethodDesc[] GetMethodDescs(BindingFlags bindingFlags);

        /// <summary>
        /// メソッド情報の配列を返します。
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        /// <exception cref="MethodNotFoundRuntimeException">
        /// MethodInfoが見つからない場合。
        /// </exception>
        IMethodDesc[] GetMethodDescs(string methodName);

        /// <summary>
        /// メソッド情報の配列を返します。
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        /// <exception cref="MethodNotFoundRuntimeException">
        /// MethodInfoが見つからない場合。
        /// </exception>
        IMethodDesc[] GetMethodDescs(string methodName, BindingFlags bindingFlags);

        /// <summary>
        /// Nullable(.NET)型かどうか判定
        /// </summary>
        /// <returns></returns>
        bool IsNullable();

        /// <summary>
        /// typeを代入可能か判定
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsAssignableFrom(Type type);

        /// <summary>
        /// beanを代入可能か判定
        /// </summary>
        /// <param name="bean"></param>
        /// <returns></returns>
        bool IsAssignableFrom(IBeanDesc bean);
    }
}
