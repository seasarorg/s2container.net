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
using System.Text;
using Seasar.Framework.Util;

namespace Seasar.Dxo.Converter
{
    /// <summary>
    /// DXOプロパティコンバータのコンバートイベントパラメタクラス
    /// </summary>
    public class ConvertEventArgs : EventArgs
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="propertyName">対象プロパティ名</param>
        /// <param name="source">コンバート元のオブジェクト</param>
        /// <param name="dest">コンバート先、上書き用のオブジェクト</param>
        /// <param name="expectedType">コンバート先として期待されている型</param>
        public ConvertEventArgs (string propertyName, object source, ref object dest, Type expectedType)
        {
            PropertyName = propertyName;
            Source = source;
            Destiny = dest;
            ExpectedType = expectedType;
        }

        /// <summary>
        /// 対象プロパティ名
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// コンバート元のオブジェクト
        /// </summary>
        public object Source { get; set; }

        /// <summary>
        /// コンバート先、上書き用のオブジェクト
        /// </summary>
        public object Destiny { get; set; }


        /// <summary>
        /// コンバート先として期待されている型
        /// </summary>
        public Type ExpectedType { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(this.GetExType().Name).Append(" [");
            sb.Append(" propertyName = ").Append(PropertyName).Append(", ");
            sb.Append(" source = ").Append(Source).Append(", ");
            sb.Append(" dest = ").Append(Destiny).Append(", ");
            sb.Append(" expectedType = ").Append(ExpectedType);
            sb.Append(" ]");

            return sb.ToString();
        }
    }
}
