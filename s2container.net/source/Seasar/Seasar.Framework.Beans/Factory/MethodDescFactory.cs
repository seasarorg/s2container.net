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
using System.Collections.Generic;
using System.Reflection;
using Seasar.Framework.Util;
using Seasar.Framework.Beans.Impl;

namespace Seasar.Framework.Beans.Factory
{
    /// <summary>
    /// Method情報記述クラスファクトリ
    /// </summary>
    /// <remarks>
    /// Genericを使って一つのファクトリクラスにまとめようとも
    /// 思いましたがコードが読みづらくなるだけと判断し、
    /// 各XxxDescクラスごとに作成しています。
    /// </remarks>
    public class MethodDescFactory
    {
        private const BindingFlags DEFAULT_BINDING_FLAG = (BindingFlags.Public | BindingFlags.Instance);
        private readonly Type _beanType;
        /// <summary>
        /// BindingFlags -> メソッド情報の検索時に指定した条件
        /// string -> メソッド名
        /// IMethodDesc -> メソッド情報
        /// </summary>
        private readonly IDictionary<BindingFlags, ArrayMap<string, IMethodDesc>> _methodDescCache
            = new Dictionary<BindingFlags, ArrayMap<string, IMethodDesc>>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="beanType">メソッド情報を取り出す型</param>
        public MethodDescFactory(Type beanType)
        {
            _beanType = beanType;
        }

        /// <summary>
        /// 指定した名前のメソッドが存在するか判定
        /// </summary>
        /// <remarks>
        /// overrideがあるのでメソッド名だけをキーにできないため
        /// 持っているコレクションを一つずつチェックしています。
        /// </remarks>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public virtual bool HasMethod(string methodName)
        {
            return HasMethod(methodName, DEFAULT_BINDING_FLAG);
        }

        /// <summary>
        /// 指定した名前のメソッドが存在するか判定
        /// </summary>
        /// <remarks>
        /// overrideがあるのでメソッド名だけをキーにできないため
        /// 持っているコレクションを一つずつチェックしています。
        /// </remarks>
        /// <param name="methodName"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public virtual bool HasMethod(string methodName, BindingFlags bindingFlags)
        {
            CreateMethodDescsIfNeed(bindingFlags);
            foreach (IMethodDesc desc in _methodDescCache[bindingFlags])
            {
                if (desc.Name.Equals(methodName))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 指定した名前のメソッドが存在するか判定
        /// </summary>
        /// <remarks>
        /// overrideがあるのでメソッド名だけをキーにできないため
        /// 一度キーを組み立てています。
        /// </remarks>
        /// <param name="methodName"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        public virtual bool HasMethod(string methodName, Type[] parameterTypes)
        {
            return HasMethod(methodName, parameterTypes, DEFAULT_BINDING_FLAG);
        }

        /// <summary>
        /// 指定した名前のメソッドが存在するか判定
        /// </summary>
        /// <remarks>
        /// overrideがあるのでメソッド名だけをキーにできないため
        /// 一度キーを組み立てています。
        /// </remarks>
        /// <param name="methodName"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public virtual bool HasMethod(string methodName, Type[] parameterTypes, BindingFlags bindingFlags)
        {
            CreateMethodDescsIfNeed(bindingFlags);
            string key = CreateKey(methodName, parameterTypes);
            return _methodDescCache[bindingFlags].ContainsKey(key);
        }

        /// <summary>
        /// メソッド情報の取得
        /// </summary>
        /// <remarks>
        /// overrideがあるのでメソッド名だけをキーにできないため
        /// 一度キーを組み立てています。
        /// </remarks>
        /// <param name="methodName"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        public virtual IMethodDesc GetMethodDesc(string methodName, Type[] parameterTypes)
        {
            return GetMethodDesc(methodName, parameterTypes, DEFAULT_BINDING_FLAG);
        }

        /// <summary>
        /// メソッド情報の取得
        /// </summary>
        /// <remarks>
        /// overrideがあるのでメソッド名だけをキーにできないため
        /// 一度キーを組み立てています。
        /// </remarks>
        /// <param name="methodName"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public virtual IMethodDesc GetMethodDesc(string methodName, Type[] parameterTypes, BindingFlags bindingFlags)
        {
            CreateMethodDescsIfNeed(bindingFlags);
            string key = CreateKey(methodName, parameterTypes);
            if (_methodDescCache[bindingFlags].ContainsKey(key) == false)
            {
                throw new MethodNotFoundRuntimeException(_beanType, methodName, parameterTypes);
            }
            return _methodDescCache[bindingFlags][key];
        }

        /// <summary>
        /// メソッド情報の取得
        /// </summary>
        /// <returns></returns>
        public virtual IMethodDesc[] GetMethodDescs()
        {
            return GetMethodDescs(DEFAULT_BINDING_FLAG);
        }

        /// <summary>
        /// メソッド情報の取得
        /// </summary>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public virtual IMethodDesc[] GetMethodDescs(BindingFlags bindingFlags)
        {
            CreateMethodDescsIfNeed(bindingFlags);
            return _methodDescCache[bindingFlags].Items;
        }

        /// <summary>
        /// メソッド情報の取得
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public virtual IMethodDesc[] GetMethodDescs(string methodName)
        {
            return GetMethodDescs(methodName, DEFAULT_BINDING_FLAG);
        }

        /// <summary>
        /// メソッド情報の取得
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public virtual IMethodDesc[] GetMethodDescs(string methodName, BindingFlags bindingFlags)
        {
            CreateMethodDescsIfNeed(bindingFlags);
            List<IMethodDesc> retList = new List<IMethodDesc>(_methodDescCache.Count);
            foreach (IMethodDesc desc in _methodDescCache[bindingFlags])
            {
                if (desc.Name.Equals(methodName))
                {
                    retList.Add(desc);
                }
            }
            return retList.ToArray();
        }

        /// <summary>
        /// 未登録のプロパティ検索条件の場合、プロパティ情報の新規キャッシュを行う
        /// </summary>
        /// <param name="bindingFlags"></param>
        protected virtual void CreateMethodDescsIfNeed(BindingFlags bindingFlags)
        {
            if (_methodDescCache.ContainsKey(bindingFlags))
            {
                return;
            }
            _methodDescCache[bindingFlags] = CreateMethodDescs(_beanType, bindingFlags);
        }

        /// <summary>
        /// メソッド情報の生成
        /// </summary>
        /// <param name="beanType"></param>
        /// <param name="bindingFlags">メソッド情報取り出し条件</param>
        /// <returns></returns>
        protected virtual ArrayMap<string, IMethodDesc> CreateMethodDescs(Type beanType, BindingFlags bindingFlags)
        {
            MethodInfo[] propertyInfos = beanType.GetMethods(bindingFlags);
            ArrayMap<string, IMethodDesc> methodDescCache = new ArrayMap<string, IMethodDesc>(propertyInfos.Length);
            foreach (MethodInfo info in propertyInfos)
            {
                IMethodDesc desc = CreateMethodDesc(info);
                string key = CreateKey(info);
                methodDescCache.Add(key, desc);
            }
            return methodDescCache;
        }

        /// <summary>
        /// メソッド情報の生成
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        protected virtual IMethodDesc CreateMethodDesc(MethodInfo methodInfo)
        {
            return NewMethodDesc(methodInfo);
        }

        /// <summary>
        /// メソッド情報を一意に識別するためのキーを取得する
        /// </summary>
        /// <param name="mi"></param>
        /// <returns></returns>
        protected virtual string CreateKey(MethodInfo mi)
        {
            Type[] parameterTypes = MethodUtil.GetParameterTypes(mi);
            return CreateKey(mi.Name, parameterTypes);
        }

        /// <summary>
        /// メソッド情報を一意に識別するためのキーを取得する
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        protected virtual string CreateKey(string name, Type[] parameterTypes)
        {
            if (HasParameter(parameterTypes) == false) { return name; }
            return MethodUtil.GetSignature(name, parameterTypes);
        }

        /// <summary>
        /// パラメータをもつかどうか判定
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected virtual bool HasParameter(Array parameters)
        {
            return (parameters != null && parameters.Length > 0);
        }

        /// <summary>
        /// メソッド情報の生成
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static IMethodDesc NewMethodDesc(MethodInfo methodInfo)
        {
            return new MethodDescImpl(methodInfo);
        }
    }
}
