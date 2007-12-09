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

using System.Collections;

namespace Seasar.Dao.Impl
{
    public class RelationRowCache
    {
        private readonly ArrayList _rowMapList;

        public RelationRowCache(int size)
        {
            _rowMapList = new ArrayList();
            for (int i = 0; i < size; ++i)
                _rowMapList.Add(new Hashtable());
        }

        public object GetRelationRow(int relno, RelationKey key)
        {
            return GetRowMap(relno)[key];
        }

        public void AddRelationRow(int relno, RelationKey key, object row)
        {
            GetRowMap(relno)[key] = row;
        }

        protected Hashtable GetRowMap(int relno)
        {
            return (Hashtable) _rowMapList[relno];
        }
    }
}
