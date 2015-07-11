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
using System.Diagnostics;
using Seasar.Dxo.Exception;

namespace Seasar.Dxo.Converter.Impl
{
    /// <summary>
    /// オブジェクトをIListインタフェースに変換するコンバータ。
    /// </summary>
    public class ListConverter : AbstractPropertyConverter
    {
        protected override bool DoConvert(object source, ref object dest, Type expectType)
        {
            Debug.Assert(typeof (IList).IsAssignableFrom(expectType)
                         , String.Format(DxoMessages.EDXO1002, "expectType", "IList"));
//            Debug.Assert(typeof(IList).IsAssignableFrom(expectType)
//                         , "expectTypeはIListと互換性がなくてはならない");

            if (dest == null)
            {
                if (expectType.IsClass && !expectType.IsAbstract)
                {
                    dest = Activator.CreateInstance(expectType);
                }
                else
                {
                    throw new DxoException(
                        String.Format(DxoMessages.EDXO0001, expectType.Name));
//                    throw new DxoException(
//                        expectType.Name + "は具象クラスではないので実体化することができない");
                }
            }
            IList target = dest as IList;
            if (target != null)
            {
                target.Clear();
                if (source is IEnumerable)
                {
                    foreach (object item in (source as IEnumerable))
                    {
                        target.Add(item);
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
