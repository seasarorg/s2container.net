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

using System.Collections;
using System.Data;
using Seasar.Extension.ADO;

namespace Seasar.Dao.Impl
{
    public abstract class AbstractObjectListDataReaderHandler : IDataReaderHandler
    {
        protected delegate void ReceiveValues(object val, int index);

        #region IDataReaderHandler メンバ

        public abstract object Handle(IDataReader dataReader);

        #endregion

        protected virtual void Handle(IDataReader dataReader, IList resultList)
        {
            while (dataReader.Read())
            {
                //  検索結果の先頭列の値を戻り値要素の値と見なす
                object val = dataReader.GetValue(0);
                resultList.Add(GetValue(val));
            }
        }

        protected virtual object GetValue(object val)
        {
            return val;
        }
    }
}
