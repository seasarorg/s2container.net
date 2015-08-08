#region Copyright
/*
 * Copyright 2005-2015 the Seasar Foundation and the Others.
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
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Framework.Exceptions;
using Seasar.Framework.Util;

namespace Seasar.Dao.Id
{
    public abstract class AbstractIdentifierGenerator : IIdentifierGenerator
    {
        private static readonly IDataReaderHandler _dataReaderHandler = new ObjectDataReaderHandler();

        protected AbstractIdentifierGenerator(string propertyName, IDbms dbms)
        {
            PropertyName = propertyName;
            Dbms = dbms;
        }

        public string PropertyName { get; }

        public IDbms Dbms { get; }

        protected object ExecuteSql(IDataSource ds, string sql, object[] args)
        {
            ISelectHandler handler = new BasicSelectHandler(ds, sql, _dataReaderHandler);
            return handler.Execute(args);
        }

        protected void SetIdentifier(object bean, object value)
        {
            if (PropertyName == null) throw new EmptyRuntimeException("propertyName");
            var propertyInfo = bean.GetExType().GetProperty(PropertyName);

//            propertyInfo.SetValue(bean,
//                ConversionUtil.ConvertTargetType(value, propertyInfo.PropertyType), null);
            PropertyUtil.SetValue(bean, bean.GetExType(), propertyInfo.Name, propertyInfo.PropertyType, 
                ConversionUtil.ConvertTargetType(value, propertyInfo.PropertyType));
        }

        #region IIdentifierGenerator ÉÅÉìÉo

        public virtual bool IsSelfGenerate
        {
            get { throw new NotImplementedException(); }
        }

        public virtual void SetIdentifier(object bean, IDataSource ds)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
