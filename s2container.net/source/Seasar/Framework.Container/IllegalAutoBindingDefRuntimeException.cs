#region Copyright
/*
 * Copyright 2005-2007 the Seasar Foundation and the Others.
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
using Seasar.Framework.Exceptions;

namespace Seasar.Framework.Container
{
    /// <summary>
    /// 不正な自動バインディング定義が指定された場合にスローされます。
    /// </summary>
    /// <seealso cref="IAutoBindingDef"/>
    /// <seealso cref="Seasar.Framework.Container.Assembler.AutoBindingDefFactory.GetAutoBindingDef"/>
    [Serializable]
    public class IllegalAutoBindingDefRuntimeException : SRuntimeException
    {
        private string autoBindingName;

        /// <summary>
        /// <code>IllegalAutoBindingDefRuntimeException</code>を構築します。
        /// </summary>
        /// <param name="autoBindingName">指定された不正な自動バインディング定義名</param>
        public IllegalAutoBindingDefRuntimeException(string autoBindingName)
            : base ("ESSR0077", new object[] { autoBindingName })
        {
            this.autoBindingName = autoBindingName;
        }

        /// <summary>
        /// 例外の原因となった不正な自動バインディング定義名を返します。
        /// </summary>
        /// <value>自動バインディング定義名</value>
        public string AutoBindingName
        {
            get { return autoBindingName; }
        }
    }
}
