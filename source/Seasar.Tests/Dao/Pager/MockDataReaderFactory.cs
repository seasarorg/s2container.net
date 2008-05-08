#region Copyright
/*
 * Copyright 2005-2008 the Seasar Foundation and the Others.
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

namespace Seasar.Tests.Dao.Pager
{
    public class MockDataReaderFactory : IDataReaderFactory
    {
        private readonly IList _createdDataReaders = new ArrayList();

        #region IDataReaderFactory ÉÅÉìÉo

        public IDataReader CreateDataReader(IDataSource dataSource, IDbCommand cmd)
        {
            MockDataReader reader = new MockDataReader();
            _createdDataReaders.Add(reader);
            return reader;
        }

        #endregion

        public int CreatedDataReaderCount
        {
            get { return _createdDataReaders.Count; }
        }

        public IDataReader GetCreatedDataReader(int index)
        {
            return _createdDataReaders[index] as IDataReader;
        }
    }
}
