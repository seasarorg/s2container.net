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

using Seasar.Extension.ADO;

namespace Seasar.Dao.Impl
{
    public class InsertAutoStaticCommand : AbstractAutoStaticCommand
    {
        public InsertAutoStaticCommand(IDataSource dataSource, ICommandFactory commandFactory,
            IBeanMetaData beanMetaData, string[] propertyNames)
            : base(dataSource, commandFactory, beanMetaData, propertyNames)
        {
        }

        protected override AbstractAutoHandler CreateAutoHandler()
        {
            return new InsertAutoHandler(DataSource, CommandFactory,
                BeanMetaData, PropertyTypes);
        }

        protected override void SetupSql()
        {
            SetupInsertSql();
        }

        protected override void SetupPropertyTypes(string[] propertyNames)
        {
            SetupInsertPropertyTypes(propertyNames);
        }
    }
}
