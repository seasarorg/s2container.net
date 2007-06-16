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

namespace Seasar.Dao.Attrs
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class IDAttribute : Attribute
    {
        private readonly IDType _idType = IDType.ASSIGNED;
        private readonly string _sequenceName;
        private readonly KindOfDbms _dbms = KindOfDbms.None;

        public IDAttribute(string id)
            : this(id, null)
        {
        }

        public IDAttribute(string id, KindOfDbms dbms)
            : this(id, null, dbms)
        {
        }

        public IDAttribute(string id, string sequenceName)
            : this(id, sequenceName, KindOfDbms.None)
        {
        }

        public IDAttribute(string id, string sequenceName, KindOfDbms dbms)
        {
            if ("assigned".Equals(id))
            {
                _idType = IDType.ASSIGNED;
            }
            else if ("identity".Equals(id))
            {
                _idType = IDType.IDENTITY;
            }
            else if ("sequence".Equals(id))
            {
                _idType = IDType.SEQUENCE;
            }
            else
            {
                throw new ArgumentException("id");
            }
            _sequenceName = sequenceName;
            _dbms = dbms;
        }

        public IDAttribute(IDType idType)
            : this(idType, null)
        {
        }

        public IDAttribute(IDType idType, KindOfDbms dbms)
            : this(idType, null, dbms)
        {
        }

        public IDAttribute(IDType idType, string sequenceName)
            : this(idType, sequenceName, KindOfDbms.None)
        {
        }

        public IDAttribute(IDType idType, string sequenceName, KindOfDbms dbms)
        {
            _idType = idType;
            _sequenceName = sequenceName;
            _dbms = dbms;
        }

        public IDType IDType
        {
            get { return _idType; }
        }

        public string SequenceName
        {
            get { return _sequenceName; }
        }

        public KindOfDbms Dbms
        {
            get { return _dbms; }
        }
    }
}
