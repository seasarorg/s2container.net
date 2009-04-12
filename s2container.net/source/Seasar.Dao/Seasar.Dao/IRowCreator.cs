#region Copyright
/*
 * Copyright 2005-2009 the Seasar Foundation and the Others.
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
using System.Data;

namespace Seasar.Dao
{
    public interface IRowCreator
    {
        /// <summary>
        /// 1行分のオブジェクトを作成する
        /// </summary>
        /// <param name="reader">IDataReader</param>
        /// <param name="columns">Columnのメタデータ</param>
        /// <param name="beanType">オブジェクトの型</param>
        /// <returns>1行分のEntity型のオブジェクト</returns>
        object CreateRow(IDataReader reader, IColumnMetaData[] columns, Type beanType);

        /// <summary>
        /// Columnのメタデータを作成する
        /// </summary>
        /// <param name="columnNames">カラム名のリスト</param>
        /// <param name="beanMetaData">メタ情報</param>
        /// <returns>Columnのメタデータの配列</returns>
        IColumnMetaData[] CreateColumnMetaData(IList columnNames, IBeanMetaData beanMetaData);
    }
}
