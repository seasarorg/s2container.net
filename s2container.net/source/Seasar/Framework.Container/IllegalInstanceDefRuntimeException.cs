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
    /// 不正なコンポーネントインスタンス定義が指定された場合にスローされます。
    /// </summary>
    /// <seealso cref="IInstanceDef"/>
    /// <seealso cref="Seasar.Framework.Container.Deployer.InstanceDefFactory"/>
    [Serializable]
    public class IllegalInstanceDefRuntimeException : SRuntimeException
    {
        private string instanceName;

        /// <summary>
        /// <code>IllegalInstanceDefRuntimeException</code>を構築します。
        /// </summary>
        /// <param name="instanceName">指定された不正なコンポーネントインスタンス定義名</param>
        public IllegalInstanceDefRuntimeException(string instanceName)
            : base("ESSR0078", new object[] { instanceName })
        {
            this.instanceName = instanceName;
        }

        /// <summary>
        /// 例外の原因となったコンポーネントインスタンス定義名を返します。
        /// </summary>
        /// <value>コンポーネントインスタンス定義名</value>
        public string InstanceName
        {
            get { return instanceName; }
        }
    }
}
