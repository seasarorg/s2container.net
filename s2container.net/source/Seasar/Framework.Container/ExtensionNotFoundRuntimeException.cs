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
    /// 指定されたパスのファイル名に、 拡張子が付いていなかった場合にスローされます。
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="Seasar.Framework.Container.Factory.S2ContainerFactory">S2コンテナファクトリ</see>は、
    /// S2コンテナを構築しようとした際に、 拡張子に応じて
    /// <see cref="Seasar.Framework.Container.Factory.IS2ContainerBuilder">S2コンテナビルダー</see>を
    /// 切り替えます。このため、 指定された設定ファイル(diconファイルなど)のファイル名に
    /// 拡張子が付いていない場合には、 この例外が発生します。
    /// </para>
    /// </remarks>
    [Serializable]
    public class ExtensionNotFoundRuntimeException : SRuntimeException
    {
        private string path;

        /// <summary>
        /// パスを指定して<code>ExtensionNotFoundRuntimeException</code>を構築します。
        /// </summary>
        /// <param name="path">指定されたパス</param>
        public ExtensionNotFoundRuntimeException(string path)
            : base("ESSR0074", new object[] { path })
        {
            this.path = path;
        }

        /// <summary>
        /// 指定されたパスを返します。
        /// </summary>
        /// <value>指定されたパス</value>
        public string Path
        {
            get { return path; }
        }
    }
}
