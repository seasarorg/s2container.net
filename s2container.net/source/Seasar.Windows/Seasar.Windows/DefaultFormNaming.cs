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

using System;
using Seasar.Framework.Container.AutoRegister;

namespace Seasar.Windows
{
    /// <summary>
    /// 起動WindowsForm指定用クラス
    /// </summary>
    public class DefaultFormNaming : IAutoNaming
    {
        private string _mainFormName;
        private string _label = "MainForm";

        /// <summary>
        /// 起動WindowsForm
        /// </summary>
        public string MainFormName
        {
            get { return _mainFormName; }
            set { _mainFormName = value; }
        }

        /// <summary>
        /// WindowsForm指定ラベル
        /// </summary>
        public string Label
        {
            get { return _label; }
            set { _label = value; }
        }

        #region IAutoNaming Members

        /// <summary>
        /// コンポーネント名を定義します。
        /// </summary>
        /// <param name="type">コンポーネント名を定義したいType</param>
        /// <returns>コンポーネント名</returns>
        public string DefineName(Type type)
        {
            string name = type.Name;
            if (name == _mainFormName)
            {
                if (!String.IsNullOrEmpty(_label))
                    return _label;
                else
                    return name;
            }
            else
            {
                return name;
            }
        }

        #endregion
    }
}