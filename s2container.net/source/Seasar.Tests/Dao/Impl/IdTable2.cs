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

using Seasar.Dao;
using Seasar.Dao.Attrs;

namespace Seasar.Tests.Dao.Impl
{
    [Table("IDTABLE")]
    public class IdTable2
    {
        private int _myId;
        private string _idName;

        [ID(IDType.IDENTITY, KindOfDbms.DB2)]
        [ID(IDType.SEQUENCE, "SEQ_IDTABLE", KindOfDbms.Firebird)]
        [ID(IDType.SEQUENCE, "SEQ_IDTABLE", KindOfDbms.MDB)]
        [ID(IDType.IDENTITY, KindOfDbms.MSSQLServer)]
        [ID(IDType.IDENTITY, KindOfDbms.MySQL)]
        [ID(IDType.SEQUENCE, "SEQ_IDTABLE", KindOfDbms.Oracle)]
        [ID(IDType.SEQUENCE, "SEQ_IDTABLE", KindOfDbms.PostgreSQL)]
        [Column("ID")]
        public int MyId
        {
            set { _myId = value; }
            get { return _myId; }
        }

        public string IdName
        {
            set { _idName = value; }
            get { return _idName; }
        }
    }
}
