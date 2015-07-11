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
using System.Reflection;

namespace Seasar.Framework.Beans
{
    /// <summary>
    /// メソッド情報記述インターフェース
    /// </summary>
    public interface IMethodDesc
    {
        /// <summary>
        /// 元となるメソッド情報
        /// </summary>
        MethodInfo Method { get; }

        /// <summary>
        /// メソッド名
        /// </summary>
        string Name { get; }

        /// <summary>
        /// パラメータ情報
        /// </summary>
        /// <returns></returns>
        ParameterInfo[] GetParameterInfos();

        /// <summary>
        /// 戻り値の型を返す
        /// </summary>
        /// <returns>戻り値の型（voidならnull)</returns>
        Type GetReturnType();

        /// <summary>
        /// Overridできるかどうか判定する
        /// </summary>
        /// <returns></returns>
        bool CanOverride();

        /// <summary>
        /// メソッド呼び出し
        /// </summary>
        /// <param name="obj">メソッドをもつオブジェクト</param>
        /// <param name="parameters">メソッドに渡す引数</param>
        /// <returns>メソッドの戻り値</returns>
        object Invoke(object obj, params object[] parameters);
    }
}
