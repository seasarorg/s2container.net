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

using System.Text.RegularExpressions;
using Seasar.Framework.Util;

namespace Seasar.Framework.Container.AutoRegister
{
    /// <summary>
    /// 自動登録の対象、非対象となるクラス名のパターンを保持します。
    /// </summary>
	public class ClassPattern
	{
        private string namespaceName;
        private Regex[] shortClassNamePatterns;

        /// <summary>
        /// デフォルトのコンストラクタです。
        /// </summary>
        public ClassPattern()
        {
        }

        /// <summary>
        /// 名前空間名とクラス名のパターンを受け取るコンストラクタです。
        /// </summary>
        /// <param name="namespaceName">名前空間名</param>
        /// <param name="shortClassNames">クラス名のパターン</param>
        public ClassPattern(string namespaceName, string shortClassNames)
        {
            NamespaceName = namespaceName;
            ShortClassNames = shortClassNames;
        }

        /// <summary>
        /// 名前空間名を取得・設定します。
        /// </summary>
        public string NamespaceName
        {
            set { namespaceName = value; }
            get { return namespaceName; }
        }

        /// <summary>
        /// （名前空間を含まない）クラス名のパターンを設定します。
        /// </summary>
        /// <remarks>
        /// 複数のパターンを設定する場合、','で区切ります。
        /// </remarks>
        public string ShortClassNames
        {
            set
            {
                string[] classNames = value.Split(',');
                shortClassNamePatterns = new Regex[classNames.Length];

                for (int i = 0; i < classNames.Length; ++i)
                {
                    string className = classNames[i].Trim();
                    shortClassNamePatterns[i] = new Regex(className, RegexOptions.Compiled);
                }
            }
        }

        /// <summary>
        /// （名前空間を含まない）クラス名がパターンに一致しているかどうかを返します。
        /// </summary>
        /// <param name="shortClassName">クラス名</param>
        /// <returns>一致している場合はtrue, 一致していない場合はfalse</returns>
        public bool IsAppliedShortClassName(string shortClassName)
        {
            if (shortClassNamePatterns == null)
            {
                return true;
            }

            for (int i = 0; i < shortClassNamePatterns.Length; ++i)
            {
                if (shortClassNamePatterns[i].IsMatch(shortClassName))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 名前空間名がパターンに一致しているかどうかを返します。
        /// </summary>
        /// <param name="namespaceName">名前空間名</param>
        /// <returns>一致している場合はtrue, 一致していない場合はfalse</returns>
        public bool IsAppliedNamespaceName(string namespaceName)
        {
            if (!StringUtil.IsEmpty(namespaceName)
                && !StringUtil.IsEmpty(this.namespaceName))
            {
                return AppendDelimiter(namespaceName).StartsWith(
                    AppendDelimiter(this.namespaceName));
            }

            if (StringUtil.IsEmpty(namespaceName)
                && StringUtil.IsEmpty(this.namespaceName))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// デリミタを追加します。
        /// </summary>
        /// <param name="name">名前空間名</param>
        /// <returns>名前空間名に後ろにデリミタ('.')を追加したもの</returns>
        protected static string AppendDelimiter(string name)
        {
            return name.EndsWith(".") ? name : name + ".";
        }
    }
}
