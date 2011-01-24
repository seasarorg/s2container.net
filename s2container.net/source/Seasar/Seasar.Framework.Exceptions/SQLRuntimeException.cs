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
using System.Runtime.Serialization;

namespace Seasar.Framework.Exceptions
{
    /// <summary>
    /// RDBMSが警告またはエラーを返したときにスローされる例外
    /// </summary>
    [Serializable]
    public class SQLRuntimeException : SRuntimeException
    {
        private readonly string _sql;

        /// <summary>
        /// SQLRuntimeExceptionクラスの新しいインスタンスを初期化し、原因となった例外を設定する
        /// </summary>
        /// <param name="cause">原因となった例外</param>
        public SQLRuntimeException(Exception cause)
            : base("ESSR0071", new object[] { cause }, cause)
        {
        }

        /// <summary>
        /// SQLRuntimeExceptionクラスの新しいインスタンスを初期化し、原因となった例外とSQLを設定する
        /// </summary>
        /// <param name="cause">原因となった例外</param>
        /// <param name="sql">原因となったSQL</param>
        public SQLRuntimeException(Exception cause, string sql)
            : this(cause)
        {
            _sql = sql;
        }

        /// <summary>
        /// シリアル化したデータを使用して、SQLRuntimeExceptionクラスの新しいインスタンスを初期化する
        /// </summary>
        /// <param name="info">シリアル化されたオブジェクト データを保持するオブジェクト</param>
        /// <param name="context">転送元または転送先に関するコンテキスト情報</param>
        public SQLRuntimeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _sql = info.GetString("_sql");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_sql", _sql, typeof(string));
            base.GetObjectData(info, context);
        }

        /// <summary>
        /// 例外の原因となったSQLを設定もしくは取得する
        /// </summary>
        public string Sql
        {
            get { return _sql; }
        }
    }
}
