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
    /// diconファイルなどの設定情報に対応するS2コンテナが、 
    /// コンテナツリーに登録されていなかった場合にスローされます。
    /// </summary>
    [Serializable]
    public class ContainerNotRegisteredRuntimeException : SRuntimeException
    {
        private string path;

        /// <summary>
        /// 登録されていなかった設定情報のパスを指定して、
        /// <code>ContainerNotRegisteredRuntimeException</code>を構築します。
        /// </summary>
        /// <param name="path">登録されていなかった設定情報のパス</param>
        public ContainerNotRegisteredRuntimeException(string path)
            : base("ESSR0075", new object[] { path })
        {
            this.path = path;
        }

        /// <summary>
        /// コンテナツリーに登録されていなかった設定情報のパスを返します。
        /// </summary>
        /// <value>登録されていなかった設定情報のパス</value>
        public string Path
        {
            get { return path; }
        }
    }
}
