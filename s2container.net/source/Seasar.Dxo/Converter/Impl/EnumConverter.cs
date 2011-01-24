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
using System.Diagnostics;

namespace Seasar.Dxo.Converter.Impl
{
    /// <summary>
    /// オブジェクトをEnumに変換するためのコンバータクラス
    /// </summary>
    public class EnumConverter : AbstractPropertyConverter
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
            Debug.Assert(expectType.IsEnum, String.Format(DxoMessages.EDXO1004, "expectType", "Enum"));
//            Debug.Assert(expectType.IsEnum, "expectTypeはEnumであるはず");

            if (source.GetType().IsEnum)
            {
                if (Enum.IsDefined(expectType, source))
                {
                    dest = source;
                    return true;
                }
            }
            else if (source is string)
            {
                //Enumの類推を行う   
                dest = _GetEnumValue(source as string, expectType);
                return true;
            }
            else if (source is IConvertible)
            {
                if (Enum.IsDefined(expectType, source))
                {
                    dest = Enum.ToObject(expectType, source);
                    return true;
                }
            }
            return false;
        }

        private static object _GetEnumValue(string strvalue, Type enumType)
        {
            string[] enumElements =
                strvalue.Split(new char[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
            if (enumElements.Length > 1)
            {
                return Enum.Parse(enumType, enumElements[1], true);
            }
            return Enum.Parse(enumType, strvalue, true);
        }
    }
}
