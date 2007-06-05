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
using System.Collections.Generic;
using Seasar.Framework.Aop;
using Seasar.Framework.Aop.Proxy;

namespace Seasar.Quill
{
    /// <summary>
    /// コンポーネントを格納するコンテナクラス
    /// </summary>
    /// <remarks>
    /// <para>
    /// 格納するコンポーネントのインスタンスは1度生成されると
    /// 同じものが使用される(singleton)</para>
    /// </remarks>
    public class QuillContainer
    {
        // 作成済みにコンポーネントを格納する
        protected IDictionary<Type, QuillComponent> components =
            new Dictionary<Type, QuillComponent>();

        // Aspectを構築するBuilder
        protected AspectBuilder aspectBuilder;

        /// <summary>
        /// QuillContainerの初期化を行うコンストラクタ
        /// </summary>
        public QuillContainer()
        {
            // QuillContainer内で使用するAspectBuilderを作成する
            aspectBuilder = new AspectBuilder(this);
        }

        /// <summary>
        /// Quillコンポーネントを取得する
        /// </summary>
        /// <remarks>
        /// <para>インスタンスの受け側のTypeとインスタンスのTypeが同じ場合の
        /// QuillComponentを取得する。</para>
        /// <para>Quillコンポーネントが生成済みの場合は生成済みの
        /// Quillコンポーネントを返す。</para>        
        /// </remarks>
        /// <param name="type">インスタンスの受け側のType</param>
        /// <returns>Quillコンポーネント</returns>
        public virtual QuillComponent GetComponent(Type type)
        {
            // Quillコンポーネントを取得して返す
            return GetComponent(type, type);
        }

        /// <summary>
        /// Quillコンポーネントを取得する
        /// </summary>
        /// <remarks>
        /// Quillコンポーネントが生成済みの場合は生成済みの
        /// Quillコンポーネントを返す。
        /// </remarks>
        /// <param name="type">インスタンスの受け側のType</param>
        /// <param name="implType">インスタンスのType</param>
        /// <returns>Quillコンポーネント</returns>
        public virtual QuillComponent GetComponent(Type type, Type implType)
        {
            lock (components)
            {
                // 既に作成済みのインスタンスであるか確認する
                if (components.ContainsKey(type))
                {
                    // 既に作成済みであれば作成済みのインスタンスを返す
                    return components[type];
                }

                // Aspectを作成する（Aspect属性が指定されていなければサイズ0となる)
                IAspect[] aspects = aspectBuilder.CreateAspects(implType);

                if (implType.IsInterface && aspects.Length == 0)
                {
                    // InterfaceでAspectが定義されていない場合は例外をスローする
                    throw new QuillApplicationException("EQLL0008",
                        new string[] { implType.FullName });
                }

                // Quillコンポーネントを作成する
                QuillComponent component = new QuillComponent(implType, type, aspects);

                // 作成済みのQuillコンポーネントを保存する
                components[implType] = component;

                // Quillコンポーネントを返す
                return component;
            }
        }
    }
}
