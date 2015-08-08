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
using System.Diagnostics;
using Seasar.Framework.Util;

namespace Seasar.Dxo.Converter
{
    /// <summary>
    /// モデルを相互変換するためのコンバータ抽象クラス
    /// </summary>
    public abstract class AbstractPropertyConverter : IPropertyConverter
    {
        public string Format { get; set; }

        /// <summary>
        /// プロパティのコンバート直前に発生するイベント
        /// </summary>
        public event EventHandler<ConvertEventArgs> PrepareConvert;

        /// <summary>
        /// コンバートが完了した際に発生するイベント
        /// </summary>
        public event EventHandler<ConvertEventArgs> ConvertCompleted;

        /// <summary>
        /// コンバートが失敗した際に発生するイベント
        /// </summary>
        public event EventHandler<ConvertEventArgs> ConvertFail;

        /// <summary>
        /// オブジェクトのプロパティを任意の型に変換します
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        /// <param name="source">変換元のオブジェクト</param>
        /// <param name="dest">変換先のオブジェクト</param>
        /// <param name="expectType">変換先のオブジェクトに期待されている型</param>
        public void Convert(string propertyName, object source, ref object dest, Type expectType)
        {
            PrepareConvert?.Invoke(this, new ConvertEventArgs(propertyName, source, ref dest, expectType));

            if (DoConvert(source, ref dest, expectType))
            {
                //コンバート成功
                ConvertCompleted?.Invoke(this, new ConvertEventArgs(propertyName, source, ref dest, expectType));
            }
            else
            {
                Debug.WriteLine("### Property Conversion fail!");
                Debug.WriteLine("         property PropertyConverter:" + this.GetExType().Name);
                Debug.WriteLine("         property Name     :" + propertyName);
                Debug.WriteLine("             source Type   :" + (source?.GetExType().Name ?? "null"));
                Debug.WriteLine("             source Value  :" + (source?.ToString() ?? "null"));
                Debug.WriteLine("            expected Type  :" + expectType.Name);
                //コンバート失敗
                ConvertFail?.Invoke(this, new ConvertEventArgs(propertyName, source, ref dest, expectType));
            }
        }


        /// <summary>
        /// オブジェクトのプロパティを任意の型に変換します
        /// (抽象メソッドは派生クラスで必ずオーバライドされます)
        /// </summary>
        /// <param name="source">変換元のオブジェクト</param>
        /// <param name="dest">変換先のオブジェクト</param>
        /// <param name="expectType">変換先のオブジェクトに期待されている型</param>
        /// <returns>bool 変換が成功した場合にはtrue</returns>
        protected abstract bool DoConvert(object source, ref object  dest, Type expectType);
    }
}
