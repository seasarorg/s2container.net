#region Copyright
/*
 * Copyright 2005-2012 the Seasar Foundation and the Others.
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

namespace Seasar.Dxo.Converter
{
    /// <summary>
    /// プロパティをある型に変換するためのコンバータインタフェース
    /// </summary>
    public interface IPropertyConverter
    {
        /// <summary>
        /// オブジェクトのプロパティを任意の型に変換します
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        /// <param name="source">変換元のオブジェクト</param>
        /// <param name="dest">変換先のオブジェクト</param>
        /// <param name="expectType">変換先のオブジェクトに期待されている型</param>
        void Convert(string propertyName, object source, ref object dest, Type expectType);

        /// <summary>
        /// 書式
        /// </summary>
        string Format { set; get; }

        /// <summary>
        /// プロパティのコンバート直前に発生するイベント
        /// </summary>
        event EventHandler<ConvertEventArgs> PrepareConvert;

        /// <summary>
        /// コンバートが完了した際に発生するイベント
        /// </summary>
        event EventHandler<ConvertEventArgs> ConvertCompleted;

        /// <summary>
        /// コンバートが失敗した際に発生するイベント
        /// </summary>
        event EventHandler<ConvertEventArgs> ConvertFail;
    }
}
