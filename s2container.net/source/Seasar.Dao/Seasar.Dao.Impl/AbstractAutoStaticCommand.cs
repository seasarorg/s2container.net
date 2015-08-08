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

using System.Collections;
using System.Text;
using Seasar.Extension.ADO;

namespace Seasar.Dao.Impl
{
    public abstract class AbstractAutoStaticCommand : AbstractStaticCommand
    {
        protected AbstractAutoStaticCommand(IDataSource dataSource,
            ICommandFactory commandFactory, IBeanMetaData beanMetaData, string[] propertyNames)
            : base(dataSource, commandFactory, beanMetaData)
        {
            SetupPropertyTypes(propertyNames);
            SetupSql();
        }

        public override object Execute(object[] args)
        {
            var handler = CreateAutoHandler();
            handler.Sql = Sql;
            var rows = handler.Execute(args);
            if (rows != 1) throw new NotSingleRowUpdatedRuntimeException(args[0], rows);
            return rows;
        }

        protected IPropertyType[] PropertyTypes { get; set; }

        protected abstract AbstractAutoHandler CreateAutoHandler();

        protected abstract void SetupPropertyTypes(string[] propertyNames);

        protected void SetupInsertPropertyTypes(string[] propertyNames)
        {
            var types = new ArrayList();
            for (var i = 0; i < propertyNames.Length; ++i)
            {
                var pt = BeanMetaData.GetPropertyType(propertyNames[i]);
                if (pt.IsPrimaryKey && !BeanMetaData.IdentifierGenerator.IsSelfGenerate)
                    continue;
                types.Add(pt);
            }
            PropertyTypes = (IPropertyType[]) types.ToArray(typeof(IPropertyType));
        }

        protected void SetupUpdatePropertyTypes(string[] propertyNames)
        {
            var types = new ArrayList();
            for (var i = 0; i < propertyNames.Length; ++i)
            {
                var pt = BeanMetaData.GetPropertyType(propertyNames[i]);
                if (pt.IsPrimaryKey) continue;
                types.Add(pt);
            }
            PropertyTypes = (IPropertyType[]) types.ToArray(typeof(IPropertyType));
        }

        protected virtual void SetupDeletePropertyTypes(string[] propertyNames)
        {
        }

        protected abstract void SetupSql();

        protected void SetupInsertSql()
        {
            var bmd = BeanMetaData;
            var buf = new StringBuilder(100);
            buf.Append("INSERT INTO ");
            buf.Append(bmd.TableName);
            buf.Append(" (");
            for (var i = 0; i < PropertyTypes.Length; ++i)
            {
                var pt = PropertyTypes[i];
                buf.Append(pt.ColumnName);
                buf.Append(", ");
            }
            buf.Length = buf.Length - 2;
            buf.Append(") VALUES (");
            for (var i = 0; i < PropertyTypes.Length; ++i)
            {
                buf.Append("?, ");
            }
            buf.Length = buf.Length - 2;
            buf.Append(")");
            Sql = buf.ToString();
        }

        protected void SetupUpdateSql()
        {
            CheckPrimaryKey();
            var buf = new StringBuilder(100);
            buf.Append("UPDATE ");
            buf.Append(BeanMetaData.TableName);
            buf.Append(" SET ");
            for (var i = 0; i < PropertyTypes.Length; ++i)
            {
                var pt = PropertyTypes[i];
                buf.Append(pt.ColumnName);
                buf.Append(" = ?, ");
            }
            buf.Length = buf.Length - 2;
            SetupUpdateWhere(buf);
            Sql = buf.ToString();
        }

        protected void SetupDeleteSql()
        {
            CheckPrimaryKey();
            var buf = new StringBuilder(100);
            buf.Append("DELETE FROM ");
            buf.Append(BeanMetaData.TableName);
            SetupUpdateWhere(buf);
            Sql = buf.ToString();
        }

        protected void CheckPrimaryKey()
        {
            var bmd = BeanMetaData;
            if (bmd.PrimaryKeySize == 0)
                throw new PrimaryKeyNotFoundRuntimeException(bmd.BeanType);
        }

        protected void SetupUpdateWhere(StringBuilder buf)
        {
            var bmd = BeanMetaData;
            buf.Append(" WHERE ");
            for (var i = 0; i < bmd.PrimaryKeySize; ++i)
            {
                buf.Append(bmd.GetPrimaryKey(i));
                buf.Append(" = ? AND ");
            }
            buf.Length = buf.Length - 5;
            if (bmd.HasVersionNoPropertyType)
            {
                var pt = bmd.VersionNoPropertyType;
                buf.Append(" AND ");
                buf.Append(pt.ColumnName);
                buf.Append(" = ?");
            }
            if (bmd.HasTimestampPropertyType)
            {
                var pt = bmd.TimestampPropertyType;
                buf.Append(" AND ");
                buf.Append(pt.ColumnName);
                buf.Append(" = ?");
            }
        }
    }
}
