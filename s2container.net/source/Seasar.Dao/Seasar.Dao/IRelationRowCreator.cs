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
using System.Collections.Generic;
using System.Data;
using Seasar.Extension.ADO;

namespace Seasar.Dao
{
    public interface IRelationRowCreator
    {
        /// <summary>
        /// 1行分のオブジェクトを作成する
        /// </summary>
        /// <returns>1行分のEntity型のオブジェクト</returns>
        object CreateRelationRow(IDataReader reader, IRelationPropertyType rpt,
            System.Collections.IList columnNames, System.Collections.Hashtable relKeyValues,
            IDictionary<String, IDictionary<String, IPropertyType>> relationPropertyCache);

        /// <summary>
        /// 関連のプロパティキャッシュを作成する
        /// </summary>
        /// <param name="columnNames">カラム名のリスト</param>
        /// <param name="bmd">メタ情報</param>
        /// <returns>関連のプロパティキャッシュ</returns>
        IDictionary<String, IDictionary<String, IPropertyType>> CreateRelationPropertyCache(System.Collections.IList columnNames, IBeanMetaData bmd);
    }
}
