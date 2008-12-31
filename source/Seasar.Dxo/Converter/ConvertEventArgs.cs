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
using System.Text;

namespace Seasar.Dxo.Converter
{
    /// <summary>
    /// DXOプロパティコンバータのコンバートイベントパラメタクラス
    /// </summary>
    public class ConvertEventArgs : EventArgs
    {
        private string propertyName;
        private object source;
        private object dest;
        private Type expectedType;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="propertyName">対象プロパティ名</param>
        /// <param name="source">コンバート元のオブジェクト</param>
        /// <param name="dest">コンバート先、上書き用のオブジェクト</param>
        /// <param name="expectedType">コンバート先として期待されている型</param>
        public ConvertEventArgs (string propertyName, object source, ref object dest, Type expectedType)
        {
            this.propertyName = propertyName;
            this.source = source;
            this.dest = dest;
            this.expectedType = expectedType;
        }

        /// <summary>
        /// 対象プロパティ名
        /// </summary>
        public string PropertyName
        {
            get { return this.propertyName; }
            set { this.propertyName = value; }
        }

        /// <summary>
        /// コンバート元のオブジェクト
        /// </summary>
        public object Source
        {
            get { return this.source; }
            set { this.source = value; }
        }

        /// <summary>
        /// コンバート先、上書き用のオブジェクト
        /// </summary>
        public object Destiny
        {
            get { return this.dest; }
            set { this.dest = value; }
        }


        /// <summary>
        /// コンバート先として期待されている型
        /// </summary>
        public Type ExpectedType
        {
            get { return this.expectedType; }
            set { this.expectedType = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(this.GetType().Name).Append(" [");
            sb.Append(" propertyName = ").Append(this.propertyName).Append(", ");
            sb.Append(" source = ").Append(this.source).Append(", ");
            sb.Append(" dest = ").Append(this.dest).Append(", ");
            sb.Append(" expectedType = ").Append(this.expectedType);
            sb.Append(" ]");

            return sb.ToString();
        }
    }
}