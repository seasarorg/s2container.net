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
using System.Collections;
using System.Collections.Generic;

namespace Seasar.Dxo.Converter.Impl
{
    /// <summary>
    /// オブジェクトをT[]な配列に変換するコンバータ実装クラス
    /// </summary>
    public class ArrayConverter<T> : AbstractPropertyConverter
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
            if (dest == null)
            {
                //配列型は、nullがセットされていることがあるのでインスタンスを生成してやる
                dest = new T[0];
            }
            T[] result = (dest as T[]);
            if (result != null)
                Array.Clear(result, 0, result.Length);
            else
                throw new ArgumentNullException("dest");

            if (source is ICollection<T>)
            {
                ICollection<T> sourceCollection = source as ICollection<T>;
                //配列のReNew
                if (sourceCollection.Count > result.Length)
                {
                    result = new T[sourceCollection.Count];
                    dest = result; //配列を作り直したので転記が必要
                }
                //ジェネリックも直接コピーできる
                sourceCollection.CopyTo(result, 0);
                return true;
            }
                //コレクションが対象
            else if (source is ICollection)
            {
                ICollection sourceCollection = source as ICollection;
                //配列のReNew
                if (sourceCollection.Count > result.Length)
                {
                    result = new T[sourceCollection.Count];
                    dest = result; //配列を作り直したので転記が必要
                }
                if (source.GetType().IsArray)
                {
                    if (source is T[])
                    {
                        //型一致の配列ならば直接コピーできる
                        (source as T[]).CopyTo(result, 0);
                        return true;
                    }
                    else
                    {
                        //要素の型に互換性があるか調べてからバルクコピー
                        Type elementType = source.GetType().GetElementType();
                        if (typeof (T).IsAssignableFrom(elementType))
                        {
                            if (source is Array)
                            {
                                (source as Array).CopyTo(result, 0);
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    //ヘテロジニアスなコレクションの場合を考えて1アイテム毎に型チェックが必要
                    int i = 0;
                    foreach (object item in sourceCollection)
                    {
                        //要素の型に互換性があるか調べてコピー
                        if (item.GetType().IsAssignableFrom(typeof (T)))
                        {
                            result.SetValue(item, i);
                            i++;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
