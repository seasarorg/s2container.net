#region Copyright
/*
 * Copyright 2005-2010 the Seasar Foundation and the Others.
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

namespace Seasar.Windows.Attr
{
    /// <summary>
    /// Windowの表示モードタイプ
    /// </summary>
    public enum ModalType
    {
        Modal,
        Modaless
    }

    /// <summary>
    /// TargetForm属性
    /// </summary>
    /// <remarks>表示するWindowsFormを指定する</remarks>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TargetFormAttribute : Attribute
    {
        /// <summary>
        /// WindowsFormクラス
        /// </summary>
        private readonly Type _type;

        /// <summary>
        /// Window表示モードタイプ
        /// </summary>
        private readonly ModalType _mode = ModalType.Modaless;


        /// <summary>
        /// 返り値用プロパティ名
        /// </summary>
        private string _returnPropertyName;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">WindowsFormクラス</param>
        /// <param name="mode">Window表示モード</param>
        /// <param name="returnPropetyName">返り値用プロパティ名</param>
        public TargetFormAttribute(Type type, ModalType mode, string returnPropetyName)
        {
            _type = type;
            _mode = mode;
            _returnPropertyName = returnPropetyName;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">WindowsFormクラス</param>
        /// <param name="mode">Window表示モード</param>
        public TargetFormAttribute(Type type, ModalType mode)
        {
            _type = type;
            _mode = mode;
            _returnPropertyName = string.Empty;
        }

        /// <summary>
        /// WindowsFormクラス
        /// </summary>
        public Type FormType
        {
            get { return _type; }
        }

        /// <summary>
        /// Window表示モード
        /// </summary>
        public ModalType Mode
        {
            get { return _mode; }
        }

        /// <summary>
        /// 返り値用プロパティ名
        /// </summary>
        public string ReturnPropertyName
        {
            get { return _returnPropertyName; }
            set { _returnPropertyName = value; }
        }
    }
}
