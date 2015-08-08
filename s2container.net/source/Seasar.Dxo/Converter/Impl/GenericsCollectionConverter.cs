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
    /// オブジェクトをICollection<typeparam name="T"/>へ変換するコンバータ実装クラス
    /// </summary>
    public class GenericsCollectionConverter<T> : AbstractPropertyConverter
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
            Debug.Assert(typeof(ICollection<T>).IsAssignableFrom(expectType)
                         , String.Format(DxoMessages.EDXO1002, "expectType", "ICollection<" + typeof(T).Name + ">"));
//            Debug.Assert(typeof(ICollection<T>).IsAssignableFrom(expectType)
//                         , "expectTypeはICollection<" + typeof(T).Name + ">と互換性がなくてはならない");

            if (dest == null)
            {
                if (expectType.IsClass && !expectType.IsAbstract)
                    dest = ClassUtil.NewInstance(expectType);
//                    dest = Activator.CreateInstance(expectType);
                else
                    throw new DxoException(String.Format(DxoMessages.EDXO0001, "expectType"));
//                throw new DxoException("expectTypeは具象クラスではないので実体化することができない");
            }
            var result = dest as ICollection<T>;
            if (result != null)
            {
                result.Clear();

                if (source is IEnumerable)
                {
                    if (source is ICollection<T>)
                    {
                        foreach (T item in (source as ICollection<T>))
                        {
                            result.Add(item);
                        }
                        return true;
                    }
                    else if (source.GetExType().IsArray)
                    {
                        //要素の型に互換性があるか
                        var elementType = source.GetExType().GetElementType();
                        if (typeof(T).IsAssignableFrom(elementType))
                        {
                            foreach (T item in source as T[])
                            {
                                result.Add(item);
                            }
                            return true;
                        }
                    }
                    else
                    {
                        foreach (var item in source as IEnumerable)
                        {
                            //ヘテロジニアスなコレクションの可能性があるので、アイテム毎に型チェックが必要
                            if (typeof(T).IsAssignableFrom(item.GetExType()))
                            {
                                result.Add((T)item);
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
