﻿#region Copyright
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
using Seasar.Framework.Container;

namespace Seasar.Quill
{
    /// <summary>
    /// インターフェースとその実装型の対応情報（Singleton）
    /// </summary>
    public class InjectionMap
    {
        protected static readonly InjectionMap _instance = new InjectionMap();

        /// <summary>
        /// Singletonなインスタンスを取得
        /// InjectionMapは基本的にアプリ起動時の設定を推奨します。
        /// （Mockとの使い分けを行いやすくするため）
        /// </summary>
        /// <returns></returns>
        public static InjectionMap GetInstance()
        {
            return _instance;
        }

        protected readonly IDictionary<Type, Type> _injectionMap;

        /// <summary>
        /// Singletonとするためのコンストラクタ
        /// </summary>
        protected InjectionMap()
        {
            _injectionMap = new Dictionary<Type, Type>();
        }

        /// <summary>
        /// インターフェースとその実装型の登録
        /// </summary>
        /// <remarks>Implementation属性(引数あり)とほぼ同じ働き</remarks>
        /// <param name="interfaceType"></param>
        /// <param name="implType"></param>
        public virtual void Add(Type interfaceType, Type implType)
        {
            if(_injectionMap.ContainsKey(interfaceType))
            {
                throw new TooManyRegistrationRuntimeException(
                    interfaceType,
                    new Type[] { interfaceType, _injectionMap[interfaceType] });
            }
            _injectionMap.Add(interfaceType, implType);
        }

        /// <summary>
        /// インターフェースとその実装型の登録
        /// </summary>
        /// <remarks>Implementation属性(引数あり)とほぼ同じ働き</remarks>
        /// <param name="map"></param>
        public virtual void Add(IDictionary<Type, Type> map)
        {
            foreach (KeyValuePair<Type, Type> pair in map)
            {
                Add(pair.Key, pair.Value);
            }
        }

        /// <summary>
        /// インターフェースを指定しない実装型の登録
        /// </summary>
        /// <remarks>Implementation属性(引数なし)とほぼ同じ働き</remarks>
        /// <param name="componentType"></param>
        public virtual void Add(Type componentType)
        {
            if(_injectionMap.ContainsKey(componentType))
            {
                throw new TooManyRegistrationRuntimeException(
                    componentType, 
                    new Type[] {componentType, _injectionMap[componentType]});
            }
            _injectionMap.Add(componentType, componentType);
        }

        /// <summary>
        /// インターフェースを指定しない実装型の登録
        /// </summary>
        /// <remarks>Implementation属性(引数なし)とほぼ同じ働き</remarks>
        /// <param name="componentTypeList"></param>
        public virtual void Add(IList<Type> componentTypeList)
        {
            foreach (Type type in componentTypeList)
            {
                Add(type);
            }
        }

        /// <summary>
        /// 実装型の取得
        /// </summary>
        /// <param name="registedType">インターフェース型</param>
        /// <returns></returns>
        public virtual Type GetComponentType(Type registedType)
        {
            if(_injectionMap.ContainsKey(registedType))
            {
                return _injectionMap[registedType];
            }
            throw new ComponentNotFoundRuntimeException(registedType);
        }

        /// <summary>
        /// 実装型が登録されているか判定
        /// </summary>
        /// <param name="type"></param>
        /// <returns>true=登録されている、false=登録されていない</returns>
        public virtual bool HasComponentType(Type type)
        {
            return _injectionMap.ContainsKey(type);
        }

        /// <summary>
        /// インターフェースとその実装型の対応情報をクリア
        /// </summary>
        public virtual void Clear()
        {
            _injectionMap.Clear();
        }
    }
}
