﻿#region Copyright
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
using Seasar.Framework.Beans.Impl;

namespace Seasar.Framework.Beans.Factory
{
    /// <summary>
    /// BeanDescを生成するクラスです。
    /// </summary>
    public class BeanDescFactory
    {
        private static readonly IDictionary<Type, IBeanDesc> _beanDescCache;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static BeanDescFactory()
        {
            _beanDescCache = new Dictionary<Type, IBeanDesc>();
        }

        /// <summary>
        /// BeanDescを返します。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IBeanDesc GetBeanDesc(Type type)
        {
            if (_beanDescCache.ContainsKey(type) == false)
            {
                var newBeanDesc = _CreateBeanDesc(type);
                _beanDescCache.Add(type, newBeanDesc);
            }
            return _beanDescCache[type];
        }

        /// <summary>
        /// キャッシュをクリアします
        /// </summary>
        public static void Clear()
        {
            _beanDescCache.Clear();
            //  Property,Method,FieldDescFactoryで保持している値については
            //  staticで参照しているわけではないので
            //  明示的なクリアは行わない
        }

        /// <summary>
        /// BeanDescインスタンスを生成します。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IBeanDesc _CreateBeanDesc(Type type) => new BeanDescImpl(type);
    }
}
