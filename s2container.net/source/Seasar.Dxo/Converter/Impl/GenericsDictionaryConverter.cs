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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Seasar.Dxo.Exception;
using Seasar.Framework.Util;

namespace Seasar.Dxo.Converter.Impl
{
    /// <summary>
    /// オブジェクトをIDictionary<typeparam name="TK"/><typeparam name="TV"/>へ変換するコンバータ実装クラス
    /// </summary>
    public class GenericsDictionaryConverter<TK, TV> : AbstractPropertyConverter 
        where TK: class
        where TV: class
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
            Debug.Assert(typeof(IDictionary<TK, TV>).IsAssignableFrom(expectType)
                         , string.Format(DxoMessages.EDXO1003, "expectType", typeof(TK).Name, typeof(TV).Name));
//            Debug.Assert(typeof(IDictionary<K, V>).IsAssignableFrom(expectType)
//                         , string.Format("expectTypeはIDictionary<{0}{1}>と互換性がなくてはならない"
//                         , typeof(K).Name, typeof(V).Name));

            if (dest == null)
            {
                if (expectType.IsClass && !expectType.IsAbstract)
                    dest = ClassUtil.NewInstance(expectType);
//                    dest = Activator.CreateInstance(expectType);
                else
                    throw new DxoException(String.Format(DxoMessages.EDXO0001, "expectType"));
//                throw new DxoException("expectTypeは具象クラスではないので実体化することができない");
            }
            var result = dest as IDictionary<TK, TV>;
            if (result != null)
            {
                result.Clear();

                var vs = source as IDictionary<TK, TV>;
                if (vs != null)
                {
                    foreach (var pair in vs)
                    {
                        result.Add(pair.Key, pair.Value);
                    }
                    return true;
                }
                else
                {
                    var dictionary = source as IDictionary;
                    if (dictionary != null)
                    {
                        foreach ( var key in dictionary.Keys)
                        {
                            var value = dictionary[key];
                            if (typeof (TK).IsAssignableFrom(key.GetExType())
                                && typeof (TV).IsAssignableFrom(value.GetExType()))
                                result.Add((TK) key, (TV) value);
                            else
                                return false;
                        }
                        return true;
                    }
                }
            }
            return false;
        }
    }

}
