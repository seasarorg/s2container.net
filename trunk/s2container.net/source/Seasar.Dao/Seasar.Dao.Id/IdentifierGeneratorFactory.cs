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
using System.Collections;
using System.Reflection;
using Seasar.Dao.Attrs;
using Seasar.Framework.Util;

namespace Seasar.Dao.Id
{
    public class IdentifierGeneratorFactory
    {
        private static readonly Hashtable _generatorTypes = new Hashtable();

        static IdentifierGeneratorFactory()
        {
            AddIdentifierGeneratorType(IDType.ASSIGNED, typeof(AssignedIdentifierGenerator));
            AddIdentifierGeneratorType(IDType.IDENTITY, typeof(IdentityIdentifierGenerator));
            AddIdentifierGeneratorType(IDType.SEQUENCE, typeof(SequenceIdentifierGenerator));
        }

        private IdentifierGeneratorFactory()
        {
        }

        public static void AddIdentifierGeneratorType(IDType idType, Type type)
        {
            _generatorTypes[idType] = type;
        }

        public static IIdentifierGenerator CreateIdentifierGenerator(
            string propertyName, IDbms dbms)
        {
            return CreateIdentifierGenerator(propertyName, dbms, null);
        }

        public static IIdentifierGenerator CreateIdentifierGenerator(
            string propertyName, IDbms dbms, IDAttribute idAttr)
        {
            if (idAttr == null)
                return new AssignedIdentifierGenerator(propertyName, dbms);
            Type type = GetGeneratorType(idAttr.IDType);
            IIdentifierGenerator generator = CreateIdentifierGenerator(type, propertyName, dbms);
            if (idAttr.IDType == IDType.SEQUENCE)
            {
                SetProperty(generator, "SequenceName", idAttr.SequenceName);
            }
            return generator;
        }

        protected static Type GetGeneratorType(IDType idType)
        {
            Type type = (Type) _generatorTypes[idType];
            if (type == null)
            {
                throw new InvalidOperationException("generatorTypes");
            }
            return type;
        }

        protected static IIdentifierGenerator CreateIdentifierGenerator(
            Type type, string propertyName, IDbms dbms)
        {
            ConstructorInfo constructor =
                ClassUtil.GetConstructorInfo(type, new Type[] { typeof(string), typeof(IDbms) });
            return (IIdentifierGenerator)
                ConstructorUtil.NewInstance(constructor, new object[] { propertyName, dbms });
        }

        protected static void SetProperty(IIdentifierGenerator generator, string propertyName, string value)
        {
            PropertyInfo property = generator.GetType().GetProperty(propertyName);
            property.SetValue(generator, value, null);
        }
    }
}
