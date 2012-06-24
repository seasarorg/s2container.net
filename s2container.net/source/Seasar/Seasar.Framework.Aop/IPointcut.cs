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

using System.Reflection;

namespace Seasar.Framework.Aop
{
    /// <summary>
    /// Advice(Interceptor)を挿入するか定義します
    /// </summary>
    /// <remarks>
    /// <p>このインターフェイスは一部AOPアライアンス準拠。<p>
    /// <p>※リフレクションを用いたIsAppliedメソッドはAOPアライアンスに非準拠です。</p>
    /// </remarks>
    /// <seealso href="http://aopalliance.sourceforge.net/doc/index.html">AOP Alliance</seealso>
    public interface IPointcut
    {
        /// <summary>
        /// 引数で渡されたmethodにAdviceを挿入するか確認します
        /// </summary>
        /// <param name="method">MethodBase メソッドとコンストラクタに関する情報を持っています</param>
        /// <returns>TrueならAdviceを挿入する、FalseならAdviceは挿入されない</returns>
        bool IsApplied(MethodBase method);

        /// <summary>
        /// 引数で渡されたメソッド名にAdviceを挿入するか確認します
        /// </summary>
        /// <param name="methodName">メソッド名</param>
        /// <returns>TrueならAdviceを挿入する、FalseならAdviceは挿入されない</returns>
        bool IsApplied(string methodName);
    }
}
