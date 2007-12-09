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
using System.Runtime.Serialization;

namespace Seasar.Dxo.Exception
{
    /// <summary>
    /// DXOネームスペースでスローする例外のルートクラス
    /// </summary>
    [Serializable]
    public class DxoException : System.Exception
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">シリアル化または逆シリアル化に必要なすべてのデータ</param>
        /// <param name="context">指定したシリアル化ストリームの転送元と転送先</param>
        protected DxoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            ;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">例外を説明するメッセージ</param>
        /// <param name="innerException">この例外の発生元となった例外</param>
        public DxoException(string message, System.Exception innerException)
            : base(message, innerException)
        {
            ;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">例外を説明するメッセージ</param>
        public DxoException(string message) : base(message)
        {
            ;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DxoException()
        {
            ;
        }
    }
}