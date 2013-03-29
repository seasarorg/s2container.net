#region Copyright
/*
 * Copyright 2005-2013 the Seasar Foundation and the Others.
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
using Seasar.Framework.Beans.Impl;
using Seasar.Framework.Util;

namespace Seasar.Framework.Beans.Factory
{
    /// <summary>
    /// フィールド情報記述クラスファクトリ
    /// </summary>
    public class FieldDescFactory
    {
        private const BindingFlags DEFAULT_BINDING_FLAG = (BindingFlags.Public | BindingFlags.Instance);
        private readonly Type _beanType;

        /// <summary>
        /// BindingFlags -> メソッド情報の検索時に指定した条件
        /// string -> メソッド名
        /// IFieldDesc -> フィールド情報
        /// </summary>
        private readonly IDictionary<BindingFlags, ArrayMap<string, IFieldDesc>> _fieldDescCache
            = new Dictionary<BindingFlags, ArrayMap<string, IFieldDesc>>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="beanType">プロパティ情報を取り出す型</param>
        public FieldDescFactory(Type beanType)
        {
            _beanType = beanType;
        }

        /// <summary>
        /// 指定した名前のプロパティが存在するか判定
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public virtual bool HasField(string fieldName)
        {
            return HasField(fieldName, DEFAULT_BINDING_FLAG);
        }

        /// <summary>
        /// 指定した名前のプロパティが存在するか判定
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public virtual bool HasField(string fieldName, BindingFlags bindingFlags)
        {
            CreateFieldDescsIfNeed(bindingFlags);

            ArrayMap<string, IFieldDesc> cache = _fieldDescCache[bindingFlags];
            if (cache.ContainsKey(fieldName)) { return true; }

            return false;
        }

        /// <summary>
        /// プロパティ情報の取得(NotNull)
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        /// <exception cref="FieldNotFoundRuntimeException"></exception>
        public virtual IFieldDesc GetFieldDesc(string fieldName)
        {
            return GetFieldDesc(fieldName, DEFAULT_BINDING_FLAG);
        }

        /// <summary>
        /// プロパティ情報の取得(NotNull)
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        /// <exception cref="FieldNotFoundRuntimeException"></exception>
        public virtual IFieldDesc GetFieldDesc(string fieldName, BindingFlags bindingFlags)
        {
            CreateFieldDescsIfNeed(bindingFlags);
            if (HasField(fieldName, bindingFlags) == false)
            {
                throw new FieldNotFoundRuntimeException(_beanType, fieldName);
            }
            return _fieldDescCache[bindingFlags][fieldName];
        }

        /// <summary>
        /// プロパティ情報一覧の取得
        /// </summary>
        /// <returns></returns>
        public virtual IFieldDesc[] GetFieldDescs()
        {
            return GetFieldDescs(DEFAULT_BINDING_FLAG);
        }

        /// <summary>
        /// プロパティ情報一覧の取得
        /// </summary>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public virtual IFieldDesc[] GetFieldDescs(BindingFlags bindingFlags)
        {
            CreateFieldDescsIfNeed(bindingFlags);
            return _fieldDescCache[bindingFlags].Items;
        }

        /// <summary>
        /// 未登録のプロパティ検索条件の場合、プロパティ情報の新規キャッシュを行う
        /// </summary>
        /// <param name="bindingFlags"></param>
        protected virtual void CreateFieldDescsIfNeed(BindingFlags bindingFlags)
        {
            if (_fieldDescCache.ContainsKey(bindingFlags))
            {
                return;
            }
            _fieldDescCache[bindingFlags] = CreatePropertyDescs(_beanType, bindingFlags);
        }

        /// <summary>
        /// プロパティ情報の生成
        /// </summary>
        /// <param name="beanType"></param>
        /// <param name="bindingFlags">プロパティの取り出し条件</param>
        /// <returns></returns>
        protected virtual ArrayMap<string, IFieldDesc> CreatePropertyDescs(Type beanType, BindingFlags bindingFlags)
        {
            FieldInfo[] fieldInfos = beanType.GetFields(bindingFlags);
            ArrayMap<string, IFieldDesc> fieldDescs = new ArrayMap<string, IFieldDesc>(fieldInfos.Length);
            foreach (FieldInfo info in fieldInfos)
            {
                IFieldDesc desc = CreateFieldDesc(info);
                fieldDescs.Add(info.Name, desc);
            }
            return fieldDescs;
        }

        /// <summary>
        /// フィールド情報記述クラスインスタンスの生成
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <returns></returns>
        protected virtual IFieldDesc CreateFieldDesc(FieldInfo fieldInfo)
        {
            return NewFieldDesc(fieldInfo);
        }

        /// <summary>
        /// フィールド情報記述クラスインスタンスの生成
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <returns></returns>
        public static IFieldDesc NewFieldDesc(FieldInfo fieldInfo)
        {
            return new FieldDescImpl(fieldInfo);
        }
    }
}
