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
using System.Reflection;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Framework.Exceptions;
using Seasar.Framework.Util;

namespace Seasar.Dao.Id
{
    public abstract class AbstractIdentifierGenerator : IIdentifierGenerator
    {
        private static readonly IDataReaderHandler _dataReaderHandler = new ObjectDataReaderHandler();
        private readonly string _propertyName;
        private readonly IDbms _dbms;

        public AbstractIdentifierGenerator(string propertyName, IDbms dbms)
        {
            _propertyName = propertyName;
            _dbms = dbms;
        }

        public string PropertyName
        {
            get { return _propertyName; }
        }

        public IDbms Dbms
        {
            get { return _dbms; }
        }

        protected object ExecuteSql(IDataSource ds, string sql, object[] args)
        {
            ISelectHandler handler = new BasicSelectHandler(ds, sql, _dataReaderHandler);
            return handler.Execute(args);
        }

        protected void SetIdentifier(object bean, object value)
        {
            if (_propertyName == null) throw new EmptyRuntimeException("propertyName");
            PropertyInfo propertyInfo = bean.GetType().GetProperty(_propertyName);

            propertyInfo.SetValue(bean,
                ConversionUtil.ConvertTargetType(value, propertyInfo.PropertyType), null);
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
