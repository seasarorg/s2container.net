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
using System.Linq.Expressions;
using System.Reflection;
using Seasar.Framework.Exceptions;

namespace Seasar.Framework.Util
{
    public static class ClassUtil
    {
        // 式木のキャッシュ
        private static readonly Dictionary<Type, Func<object>> _classCache = new Dictionary<Type, Func<object>>();

        public static ConstructorInfo GetConstructorInfo(Type type, Type[] argTypes)
        {
            var types = argTypes ?? Type.EmptyTypes;
            var constructor = type.GetConstructor(types);
            if (constructor == null)
            {
                throw new NoSuchConstructorRuntimeException(type, argTypes);
            }
            return constructor;
        }

        public static Type ForName(string className, Assembly[] assemblys)
        {
            var type = Type.GetType(className);
            if (type != null)
            {
                return type;
            }
            foreach (var assembly in assemblys)
            {
                type = assembly.GetType(className);
                if (type != null)
                {
                    return type;
                }
            }
            return null;
        }

        /// <summary>
        /// 現在使用可能なアセンブリの中から、
        /// クラス名を使って型を取得する
        /// </summary>
        /// <param name="className">名前空間を含むクラス名</param>
        /// <returns>該当する型</returns>
        public static Type ForName(string className) => ForName(className, AppDomain.CurrentDomain.GetAssemblies());

        /// <summary>
        /// インスタンスを生成する
        /// </summary>
        /// <param name="type">生成する型</param>
        /// <param name="nonPublic">パブリックの既定コンストラクター。パブリックでない既定コンストラクターを一致させる場合は、true。</param>
        /// <returns>インスタンス</returns>
        public static object NewInstance(Type type, bool nonPublic = false)
        {
            Func<object> lambda;
            if (!_classCache.ContainsKey(type))
            {
                lambda = _CreateExpression(type, nonPublic);
                // 生成した式木はキャッシュに保存
                _classCache.Add(type, lambda);
            }
            else
            {
                lambda = _classCache[type];
            }
            return lambda.DynamicInvoke(null);
//            return Activator.CreateInstance(type);
        }

        /// <summary>
        /// インスタンスを生成する
        /// </summary>
        /// <param name="info">コンストラクタ情報</param>
        /// <param name="type">生成する型</param>
        /// <returns>インスタンス</returns>
        public static object NewInstance(ConstructorInfo info, Type type)
        {
            Func<object> lambda;
            if (!_classCache.ContainsKey(type))
            {
                lambda = Expression.Lambda<Func<object>>(Expression.New(info)).Compile();
                // 生成した式木はキャッシュに保存
                _classCache.Add(type, lambda);
            }
            else
            {
                lambda = _classCache[type];
            }
            return lambda.DynamicInvoke(null);
        }

        /// <summary>
        /// インスタンスを生成する
        /// </summary>
        /// <param name="className">名前空間を含むクラス名</param>
        /// <param name="assemblyName">アセンブリ名</param>
        /// <returns>インスタンス</returns>
        public static object NewInstance(string className, string assemblyName)
        {
            Assembly[] asms = {Assembly.LoadFrom(assemblyName)};
            return NewInstance(ForName(className, asms));
        }

        /// <summary>
        /// インスタンス化する式木(Expression)を作成する
        /// </summary>
        /// <param name="type">インスタンス化する型</param>
        /// <param name="nonPublic">パブリックの既定コンストラクター。パブリックでない既定コンストラクターを一致させる場合は、true。</param>
        /// <returns>コンパイルした式木</returns>
        private static Func<object> _CreateExpression(Type type, bool nonPublic)
        {
            ConstructorInfo info;
            if (nonPublic)
            {
                info = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0], null);
            }
            else
            {
                info = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                    null, new Type[0], null);
            }
            return Expression.Lambda<Func<object>>(Expression.New(info)).Compile();
        }
    }
}