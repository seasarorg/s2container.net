#region Copyright
/*
 * Copyright 2005-2008 the Seasar Foundation and the Others.
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

using System.Collections.Generic;
using System;

namespace Seasar.Framework.Container.AutoRegister
{
    /// <summary>
    /// 自動登録用の抽象クラスです。
    /// </summary>
	public abstract class AbstractAutoRegister
	{
        private IS2Container container;
        private IList<ClassPattern> classPatterns = new List<ClassPattern>();
        private IList<ClassPattern> ignoreClassPatterns = new List<ClassPattern>();

        /// <summary>
        /// コンテナを取得・設定します。
        /// </summary>
        public IS2Container Container
        {
            set { container = value; }
            get { return container; }
        }

        /// <summary>
        /// 追加されている ClassPattern の数を取得します。
        /// </summary>
        public int ClassPatternSize
        {
            get { return classPatterns.Count; }
        }

        /// <summary>
        /// ClassPatternを取得します。
        /// </summary>
        /// <param name="index">取得するClassPatternのインデックス</param>
        /// <returns>ClassPattern</returns>
        public ClassPattern GetClassPattern(int index)
        {
            return (ClassPattern)classPatterns[index];
        }

        /// <summary>
        /// 自動登録で適用される ClassPattern を追加します。
        /// </summary>
        /// <param name="namespaceName">名前空間名</param>
        /// <param name="shortClassNames">クラス名のパターン</param>
        public void AddClassPattern(string namespaceName, string shortClassNames)
        {
            AddClassPattern(new ClassPattern(namespaceName, shortClassNames));
        }

        /// <summary>
        /// 自動登録で適用される ClassPattern を追加します。
        /// </summary>
        /// <param name="classPattern">ClassPattern</param>
        public void AddClassPattern(ClassPattern classPattern)
        {
            classPatterns.Add(classPattern);
        }

        /// <summary>
        /// 自動登録されない ClassPattern を追加します。
        /// </summary>
        /// <param name="namespaceName">名前空間名</param>
        /// <param name="shortClassNames">クラス名のパターン</param>
        public void AddIgnoreClassPattern(string namespaceName, string shortClassNames)
        {
            AddIgnoreClassPattern(new ClassPattern(namespaceName, shortClassNames));
        }

        /// <summary>
        /// 自動登録されない ClassPattern を追加します。
        /// </summary>
        /// <param name="classPattern">ClassPattern</param>
        public void AddIgnoreClassPattern(ClassPattern classPattern)
        {
            ignoreClassPatterns.Add(classPattern);
        }

        /// <summary>
        /// 自動登録を行います。
        /// </summary>
        /// <remarks>
        /// 属性による初期化メソッドの指定ができないのでJava版とは異なり
        /// このメソッドをdiconファイルで初期化メソッドとして呼び出す必要があります。
        /// </remarks>
        public abstract void RegisterAll();

        /// <summary>
        /// 無視するかどうかを返します。
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>無視するかどうか</returns>
        protected bool IsIgnore(Type type)
        {
            if (ignoreClassPatterns.Count == 0)
            {
                return false;
            }

            for (int i = 0; i < ignoreClassPatterns.Count; ++i)
            {
                ClassPattern cp = ignoreClassPatterns[i];

                if (!cp.IsAppliedNamespaceName(type.Namespace))
                {
                    continue;
                }

                if (cp.IsAppliedShortClassName(type.Name))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
