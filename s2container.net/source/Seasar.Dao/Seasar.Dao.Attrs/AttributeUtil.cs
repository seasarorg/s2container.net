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

namespace Seasar.Dao.Attrs
{
    public sealed class AttributeUtil
    {
        private AttributeUtil()
        {
        }

        public static ColumnAttribute GetColumnAttribute(PropertyInfo pi)
        {
            return Attribute.GetCustomAttribute(pi,
                                                typeof(ColumnAttribute)) as ColumnAttribute;
        }

        public static TableAttribute GetTableAttribute(Type type)
        {
            return Attribute.GetCustomAttribute(type,
                                                typeof(TableAttribute)) as TableAttribute;
        }

        public static VersionNoPropertyAttribute GetVersionNoPropertyAttribute(Type type)
        {
            return Attribute.GetCustomAttribute(type,
                                                typeof(VersionNoPropertyAttribute)) as VersionNoPropertyAttribute;
        }

        public static TimestampPropertyAttribute GetTimestampPropertyAttribute(Type type)
        {
            return Attribute.GetCustomAttribute(type,
                                                typeof(TimestampPropertyAttribute)) as TimestampPropertyAttribute;
        }

        public static RelnoAttribute GetRelnoAttribute(PropertyInfo pi)
        {
            return Attribute.GetCustomAttribute(pi,
                                                typeof(RelnoAttribute)) as RelnoAttribute;
        }

        public static IDAttribute[] GetIDAttribute(PropertyInfo pi)
        {
            return Attribute.GetCustomAttributes(pi,
                                                typeof(IDAttribute)) as IDAttribute[];
        }

        public static NoPersistentPropsAttribute GetNoPersistentPropsAttribute(MemberInfo mi)
        {
            return Attribute.GetCustomAttribute(mi,
                                                typeof(NoPersistentPropsAttribute)) as NoPersistentPropsAttribute;
        }

        public static RelkeysAttribute GetRelkeysAttribute(PropertyInfo pi)
        {
            return Attribute.GetCustomAttribute(pi,
                                                typeof(RelkeysAttribute)) as RelkeysAttribute;
        }

        public static BeanAttribute GetBeanAttribute(Type type)
        {
            return Attribute.GetCustomAttribute(type,
                                                typeof(BeanAttribute)) as BeanAttribute;
        }

        public static SqlAttribute[] GetSqlAttributes(MethodInfo mi)
        {
            return Attribute.GetCustomAttributes(mi,
                                                 typeof(SqlAttribute)) as SqlAttribute[];
        }

        public static QueryAttribute GetQueryAttribute(MethodInfo mi)
        {
            return Attribute.GetCustomAttribute(mi,
                                                typeof(QueryAttribute)) as QueryAttribute;
        }

        public static PersistentPropsAttribute GetPersistentPropsAttribute(MethodInfo mi)
        {
            return Attribute.GetCustomAttribute(mi,
                                                typeof(PersistentPropsAttribute)) as PersistentPropsAttribute;
        }

        public static ProcedureAttribute GetProcedureAttribute(MethodInfo mi)
        {
            return Attribute.GetCustomAttribute(mi, typeof(ProcedureAttribute)) as ProcedureAttribute;
        }
    }
}