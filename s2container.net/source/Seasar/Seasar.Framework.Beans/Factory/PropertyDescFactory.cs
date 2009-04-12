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
using Seasar.Framework.Util;
using System.Reflection;
using Seasar.Framework.Beans.Impl;

namespace Seasar.Framework.Beans.Factory
{
    /// <summary>
    /// Property情報記述クラスファクトリ
    /// </summary>
    /// <remarks>
    /// Genericを使って一つのファクトリクラスにまとめようとも
    /// 思いましたがコードが読みづらくなるだけと判断し、
    /// 各XxxDescクラスごとに作成しています。
    /// </remarks>
    public class PropertyDescFactory
    {
        private const BindingFlags DEFAULT_BINDING_FLAG = (BindingFlags.Public | BindingFlags.Instance);
        private readonly Type _beanType;
        /// <summary>
        /// BindingFlags -> メソッド情報の検索時に指定した条件
        /// string -> メソッド名
        /// IPropertyDesc -> プロパティ情報
        /// </summary>
        private readonly IDictionary<BindingFlags, ArrayMap<string, IPropertyDesc>> _propertyDescCache
            = new Dictionary<BindingFlags, ArrayMap<string, IPropertyDesc>>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="beanType">プロパティ情報を取り出す型</param>
        public PropertyDescFactory(Type beanType)
        {
            _beanType = beanType;
        }

        /// <summary>
        /// 指定した名前のプロパティが存在するか判定
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public virtual bool HasProperty(string propertyName)
        {
            return HasProperty(propertyName, DEFAULT_BINDING_FLAG);
        }

        /// <summary>
        /// 指定した名前のプロパティが存在するか判定
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public virtual bool HasProperty(string propertyName, BindingFlags bindingFlags)
        {
            CreatePropertyDescsIfNeed(bindingFlags);

            ArrayMap<string, IPropertyDesc> cache = _propertyDescCache[bindingFlags];
            if (cache.ContainsKey(propertyName)) { return true; }

            return false;
        }

        /// <summary>
        /// プロパティ情報の取得(NotNull)
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        /// <exception cref="PropertyNotFoundRuntimeException"></exception>
        public virtual IPropertyDesc GetProperty(string propertyName)
        {
            return GetProperty(propertyName, DEFAULT_BINDING_FLAG);
        }

        /// <summary>
        /// プロパティ情報の取得(NotNull)
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        /// <exception cref="PropertyNotFoundRuntimeException"></exception>
        public virtual IPropertyDesc GetProperty(string propertyName, BindingFlags bindingFlags)
        {
            CreatePropertyDescsIfNeed(bindingFlags);
            if (HasProperty(propertyName, bindingFlags) == false)
            {
                throw new PropertyNotFoundRuntimeException(_beanType, propertyName);
            }
            return _propertyDescCache[bindingFlags][propertyName];
        }

        /// <summary>
        /// プロパティ情報一覧の取得
        /// </summary>
        /// <returns></returns>
        public virtual IPropertyDesc[] GetPropertyDescs()
        {
            return GetPropertyDescs(DEFAULT_BINDING_FLAG);
        }

        /// <summary>
        /// プロパティ情報一覧の取得
        /// </summary>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public virtual IPropertyDesc[] GetPropertyDescs(BindingFlags bindingFlags)
        {
            CreatePropertyDescsIfNeed(bindingFlags);
            return _propertyDescCache[bindingFlags].Items;
        }

        /// <summary>
        /// 未登録のプロパティ検索条件の場合、プロパティ情報の新規キャッシュを行う
        /// </summary>
        /// <param name="bindingFlags"></param>
        protected virtual void CreatePropertyDescsIfNeed(BindingFlags bindingFlags)
        {
            if (_propertyDescCache.ContainsKey(bindingFlags))
            {
                return;
            }
            _propertyDescCache[bindingFlags] = CreatePropertyDescs(_beanType, bindingFlags);
        }

        /// <summary>
        /// プロパティ情報の生成
        /// </summary>
        /// <param name="beanType"></param>
        /// <param name="bindingFlags">プロパティの取り出し条件</param>
        /// <returns></returns>
        protected virtual ArrayMap<string, IPropertyDesc> CreatePropertyDescs(Type beanType, BindingFlags bindingFlags)
        {
            PropertyInfo[] propertyInfos = beanType.GetProperties(bindingFlags);
            ArrayMap<string, IPropertyDesc> propertyDescCache = new ArrayMap<string, IPropertyDesc>(propertyInfos.Length);
            foreach (PropertyInfo info in propertyInfos)
            {
                IPropertyDesc desc = CreatePropertyDesc(info);
                propertyDescCache.Add(info.Name, desc);
            }
            return propertyDescCache;
        }

        /// <summary>
        /// プロパティ情報インスタンスの生成
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        protected virtual IPropertyDesc CreatePropertyDesc(PropertyInfo propertyInfo)
        {
            return NewPropertyDesc(propertyInfo);
        }

        /// <summary>
        /// プロパティ情報インスタンスの生成
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static IPropertyDesc NewPropertyDesc(PropertyInfo propertyInfo)
        {
            return new PropertyDescImpl(propertyInfo);
        }
    }
}
