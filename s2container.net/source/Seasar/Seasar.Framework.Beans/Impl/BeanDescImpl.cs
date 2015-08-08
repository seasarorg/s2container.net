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
using Seasar.Framework.Beans.Factory;
using Seasar.Framework.Util;

namespace Seasar.Framework.Beans.Impl
{
    /// <summary>
    /// BeanDesc実装クラス
    /// </summary>
    public class BeanDescImpl : IBeanDesc
    {
        /// <summary>
        /// Nullable型判定結果キャッシュ
        /// </summary>
        private bool? _isNullable;

        /// <summary>
        /// プロパティ情報キャッシュ
        /// </summary>
        private PropertyDescFactory _propertyCache;

        /// <summary>
        /// メソッド情報キャッシュ
        /// </summary>
        private MethodDescFactory _methodCache;

        /// <summary>
        /// フィールド情報キャッシュ
        /// </summary>
        private FieldDescFactory _fieldCache;

        /// <summary>
        /// 保持しているリフレクション情報の対象型
        /// </summary>
        public Type BeanType { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="beanType">元となる型</param>
        public BeanDescImpl(Type beanType)
        {
            BeanType = beanType;
        }

        /// <summary>
        /// PropertyDescを持っているかどうかを返します。
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns>PropertyDescを持っているかどうか</returns>
        public virtual bool HasProperty(string propertyName)
        {
            return GetOrCreatePropertyDescCache().HasProperty(propertyName);
        }

        /// <summary>
        /// PropertyDescを持っているかどうかを返します。
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="bindingFlags"></param>
        /// <returns>PropertyDescを持っているかどうか</returns>
        public virtual bool HasProperty(string propertyName, BindingFlags bindingFlags)
        {
            return GetOrCreatePropertyDescCache().HasProperty(propertyName, bindingFlags);
        }

        /// <summary>
        /// PropertyDescを返します。
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns>PropertyDesc}</returns>
        /// <exception cref="PropertyNotFoundRuntimeException">
        /// PropertyDescが見つからない場合
        /// </exception>
        public virtual IPropertyDesc GetPropertyDesc(string propertyName)
        {
            return GetOrCreatePropertyDescCache().GetProperty(propertyName);
        }

        /// <summary>
        /// PropertyDescを返します。
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="bindingFlags"></param>
        /// <returns>PropertyDesc}</returns>
        /// <exception cref="PropertyNotFoundRuntimeException">
        /// PropertyDescが見つからない場合
        /// </exception>
        public virtual IPropertyDesc GetPropertyDesc(string propertyName, BindingFlags bindingFlags)
        {
            return GetOrCreatePropertyDescCache().GetProperty(propertyName, bindingFlags);
        }

        /// <summary>
        /// PropertyDescを返します。
        /// </summary>
        /// <param name="index"></param>
        /// <returns>PropertyDesc}</returns>
        public virtual IPropertyDesc GetPropertyDesc(int index)
        {
            return GetOrCreatePropertyDescCache().GetPropertyDescs()[index];
        }

        /// <summary>
        /// PropertyDescを返します。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="bindingFlags"></param>
        /// <returns>PropertyDesc}</returns>
        public virtual IPropertyDesc GetPropertyDesc(int index, BindingFlags bindingFlags)
        {
            return GetOrCreatePropertyDescCache().GetPropertyDescs(bindingFlags)[index];
        }

        /// <summary>
        /// PropertyDescを返します。
        /// </summary>
        /// <returns>PropertyDesc}</returns>
        public virtual IPropertyDesc[] GetPropertyDescs()
        {
            return GetOrCreatePropertyDescCache().GetPropertyDescs();
        }

        /// <summary>
        /// PropertyDescを返します。
        /// </summary>
        /// <param name="bindingFlags"></param>
        /// <returns>PropertyDesc}</returns>
        public virtual IPropertyDesc[] GetPropertyDescs(BindingFlags bindingFlags)
        {
            return GetOrCreatePropertyDescCache().GetPropertyDescs(bindingFlags);
        }

        /// <summary>
        /// フィールド情報を持っているかどうかを返します。
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns>PropertyInfoを持っているかどうか</returns>
        public virtual bool HasField(string fieldName)
        {
            return GetOrCreateFieldDescCache().HasField(fieldName);
        }

        /// <summary>
        /// フィールド情報を持っているかどうかを返します。
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="bindingFlags"></param>
        /// <returns>PropertyInfoを持っているかどうか</returns>
        public virtual bool HasField(string fieldName, BindingFlags bindingFlags)
        {
            return GetOrCreateFieldDescCache().HasField(fieldName, bindingFlags);
        }

        /// <summary>
        /// FieldDescを返します。
        /// </summary>
        /// <param name="fieldName">フィールド名</param>
        /// <returns>フィールド情報</returns>
        /// <exception cref="FieldNotFoundRuntimeException"></exception>
        public virtual IFieldDesc GetFieldDesc(string fieldName)
        {
            return GetOrCreateFieldDescCache().GetFieldDesc(fieldName);
        }

        /// <summary>
        /// FieldDescを返します。
        /// </summary>
        /// <param name="fieldName">フィールド名</param>
        /// <param name="bindingFlags"></param>
        /// <returns>フィールド情報</returns>
        /// <exception cref="FieldNotFoundRuntimeException"></exception>
        public virtual IFieldDesc GetFieldDesc(string fieldName, BindingFlags bindingFlags)
        {
            return GetOrCreateFieldDescCache().GetFieldDesc(fieldName, bindingFlags);
        }

        /// <summary>
        /// FieldDescを返します。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual IFieldDesc GetFieldDesc(int index)
        {
            return GetOrCreateFieldDescCache().GetFieldDescs()[index];
        }

        /// <summary>
        /// FieldDescを返します。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public virtual IFieldDesc GetFieldDesc(int index, BindingFlags bindingFlags)
        {
            return GetOrCreateFieldDescCache().GetFieldDescs(bindingFlags)[index];
        }

        /// <summary>
        /// FieldDescを返します。
        /// </summary>
        /// <returns></returns>
        public virtual IFieldDesc[] GetFieldDescs()
        {
            return GetOrCreateFieldDescCache().GetFieldDescs();
        }

        /// <summary>
        /// FieldDescを返します。
        /// </summary>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public virtual IFieldDesc[] GetFieldDescs(BindingFlags bindingFlags)
        {
            return GetOrCreateFieldDescCache().GetFieldDescs(bindingFlags);
        }

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
        public virtual ConstructorInfo GetConstructor(Type[] paramTypes)
        {
            var ci = BeanType.GetConstructor(paramTypes);
            if (ci == null)
            {
                throw new ConstructorNotFoundRuntimeException(BeanType, paramTypes);
            }
            return ci;
        }

        /// <summary>
        /// メソッド情報があるかどうか返します。（引数に関係なくメソッド名のみで判定します）
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns>MethodInfoがあるかどうか</returns>
        public virtual bool HasMethod(string methodName)
        {
            return GetOrCreateMethodDescCache().HasMethod(methodName);
        }

        /// <summary>
        /// メソッド情報があるかどうか返します。
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="parameterTypes"></param>
        /// <returns>MethodInfoがあるかどうか</returns>
        public virtual bool HasMethod(string methodName, Type[] parameterTypes)
        {
            return GetOrCreateMethodDescCache().HasMethod(methodName, parameterTypes);
        }

        /// <summary>
        /// メソッド情報があるかどうか返します。（引数に関係なくメソッド名のみで判定します）
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="bindingFlags"></param>
        /// <returns>MethodInfoがあるかどうか</returns>
        public virtual bool HasMethod(string methodName, BindingFlags bindingFlags)
        {
            return GetOrCreateMethodDescCache().HasMethod(methodName, bindingFlags);
        }

        /// <summary>
        /// メソッド情報があるかどうか返します。
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="bindingFlags"></param>
        /// <returns>MethodInfoがあるかどうか</returns>
        public virtual bool HasMethod(string methodName, Type[] parameterTypes, BindingFlags bindingFlags)
        {
            return GetOrCreateMethodDescCache().HasMethod(methodName, parameterTypes, bindingFlags);
        }

        /// <summary>
        /// メソッド情報を返します。引数なしのメソッドを探します。
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        /// <exception cref="MethodNotFoundRuntimeException">
        /// MethodInfoが見つからない場合。
        /// </exception>
        public virtual IMethodDesc GetMethodDesc(string methodName)
        {
            //  途中でnullが返ってくることはない（例外は発生する）ためまとめて呼び出し
            return GetOrCreateMethodDescCache().GetMethodDesc(methodName, null);
        }

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
        public virtual IMethodDesc GetMethodDesc(string methodName, BindingFlags bindingFlags)
        {
            //  途中でnullが返ってくることはない（例外は発生する）ためまとめて呼び出し
            return GetOrCreateMethodDescCache().GetMethodDesc(methodName, null, bindingFlags);
        }

        /// <summary>
        /// メソッド情報を返します。
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="paramTypes"></param>
        /// <returns>MethodInfo}</returns>
        /// <exception cref="MethodNotFoundRuntimeException">
        /// MethodInfoが見つからない場合。
        /// </exception>
        public virtual IMethodDesc GetMethodDesc(string methodName, Type[] paramTypes)
        {
            return GetOrCreateMethodDescCache().GetMethodDesc(methodName, paramTypes);
        }

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
        public virtual IMethodDesc GetMethodDesc(string methodName, Type[] paramTypes, BindingFlags bindingFlags)
        {
            return GetOrCreateMethodDescCache().GetMethodDesc(methodName, paramTypes, bindingFlags);
        }

        /// <summary>
        /// メソッド情報の配列を返します。
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MethodNotFoundRuntimeException">
        /// MethodInfoが見つからない場合。
        /// </exception>
        public virtual IMethodDesc[] GetMethodDescs()
        {
            return GetOrCreateMethodDescCache().GetMethodDescs();
        }

        /// <summary>
        /// メソッド情報の配列を返します。
        /// </summary>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        /// <exception cref="MethodNotFoundRuntimeException">
        /// MethodInfoが見つからない場合。
        /// </exception>
        public virtual IMethodDesc[] GetMethodDescs(BindingFlags bindingFlags)
        {
            return GetOrCreateMethodDescCache().GetMethodDescs(bindingFlags);
        }


        /// <summary>
        /// メソッド情報の配列を返します。
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        /// <exception cref="MethodNotFoundRuntimeException">
        /// MethodInfoが見つからない場合。
        /// </exception>
        public virtual IMethodDesc[] GetMethodDescs(string methodName)
        {
            return GetOrCreateMethodDescCache().GetMethodDescs(methodName);
        }

        /// <summary>
        /// メソッド情報の配列を返します。
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        /// <exception cref="MethodNotFoundRuntimeException">
        /// MethodInfoが見つからない場合。
        /// </exception>
        public virtual IMethodDesc[] GetMethodDescs(string methodName, BindingFlags bindingFlags)
        {
            return GetOrCreateMethodDescCache().GetMethodDescs(methodName, bindingFlags);
        }

        /// <summary>
        /// Nullable(.NET)型かどうか判定
        /// </summary>
        /// <returns></returns>
        public virtual bool IsNullable()
        {
            if (_isNullable.HasValue) { return _isNullable.Value; }
            _isNullable = AssignTypeUtil.IsNullable(BeanType);
            return _isNullable.Value;
        }

        /// <summary>
        /// typeを代入可能か判定
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual bool IsAssignableFrom(Type type)
        {
            return BeanType.IsAssignableFrom(type);
        }

        /// <summary>
        /// beanを代入可能か判定
        /// </summary>
        /// <param name="bean"></param>
        /// <returns></returns>
        public virtual bool IsAssignableFrom(IBeanDesc bean)
        {
            return IsAssignableFrom(bean.BeanType);
        }

        /// <summary>
        /// プロパティ情報キャッシュの取得
        /// </summary>
        /// <returns></returns>
        protected virtual PropertyDescFactory GetOrCreatePropertyDescCache()
        {
            return _propertyCache ?? (_propertyCache = new PropertyDescFactory(BeanType));
        }

        /// <summary>
        /// メソッド情報キャッシュの取得
        /// </summary>
        /// <returns></returns>
        protected virtual MethodDescFactory GetOrCreateMethodDescCache()
        {
            return _methodCache ?? (_methodCache = new MethodDescFactory(BeanType));
        }

        /// <summary>
        /// フィールド情報キャッシュの取得
        /// </summary>
        /// <returns></returns>
        protected virtual FieldDescFactory GetOrCreateFieldDescCache()
        {
            return _fieldCache ?? (_fieldCache = new FieldDescFactory(BeanType));
        }
    }
}
