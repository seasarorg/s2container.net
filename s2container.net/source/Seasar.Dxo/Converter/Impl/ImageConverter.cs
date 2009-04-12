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
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace Seasar.Dxo.Converter.Impl
{
    /// <summary>
    /// オブジェクトをImageに変換するコンバータ
    /// </summary>
    public class ImageConverter : AbstractPropertyConverter
    {
        /// <summary>
        /// オブジェクトのプロパティを任意の型に変換します
        /// (抽象メソッドは派生クラスで必ずオーバライドされます)
        /// </summary>
        /// <param name="source">変換元のオブジェクト</param>
        /// <param name="dest">変換先のオブジェクト</param>
        /// <param name="expectType">変換先のオブジェクトに期待されている型</param>
        /// <returns>bool 変換が成功した場合にはtrue</returns>
        protected override bool DoConvert(object source, ref object dest, Type expectType)
        {
            Debug.Assert(typeof (Image).IsAssignableFrom(expectType)
                         , String.Format(DxoMessages.EDXO1002, "expectType", "Image"));
            //            Debug.Assert(typeof(Image).IsAssignableFrom(expectType)
//                         , "expectTypeはImageと互換性がなくてはならない");

            if (source is Image)
            {
                dest = (source as Image).Clone();
                return true;
            }
            else if (source is string)
            {
                string resource = source as string;
                if (File.Exists(resource))
                {
                    dest = Image.FromFile(resource);
                    return true;
                }
            }
            return false;
        }
    }
}