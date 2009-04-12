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
 * governing permissions and limitations under the LicensSe.
 */

#endregion

using System;
using System.Globalization;

namespace Seasar.Dxo.Converter.Impl
{
    /// <summary>
    /// DateTime型変換するためのコンバータクラス
    /// </summary>
    public class DateTimeConverter : AbstractPropertyConverter
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
            if (source is string)
            {
                DateTime target;
                CultureInfo format = new CultureInfo("ja-JP", false);
                target = DateTime.ParseExact(source as string, this.Format, format);
                dest = target;

                return true;
            }
            else if (source is bool)
            {
                if ((bool) source)
                    dest = new DateTime(1);
                else
                    dest = new DateTime(0);

                return true;
            }
            else
            {
                try
                {
                    string convertedSource = source.ToString();
                    long val = Int64.Parse(convertedSource);
                    dest = new DateTime(val);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}