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

using System.Data;
using Seasar.Extension.ADO;

namespace Seasar.Dao.Pager
{
    public class PagerDataReaderFactoryWrapper : AbstractPagerDataReaderFactoryWrapper, IDataReaderFactory
    {
        public PagerDataReaderFactoryWrapper(
            IDataReaderFactory dataReaderFactory,
            ICommandFactory commandFactory
            )
            : base(dataReaderFactory, commandFactory)
        {
        }

        #region IDataReaderFactory ÉÅÉìÉo

        public IDataReader CreateDataReader(IDataSource dataSource, IDbCommand cmd)
        {
            IDataReader reader = DataReaderFactory.CreateDataReader(dataSource, cmd);
            IPagerCondition condition = PagerContext.GetContext().PeekArgs();
            if (condition != null)
            {
                return new PagerDataReaderWrapper(reader, condition);
            }
            else
            {
                return reader;
            }
        }

        #endregion
    }
}
