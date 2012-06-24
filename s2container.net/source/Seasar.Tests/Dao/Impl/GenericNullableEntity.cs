#region Copyright
/*
 * Copyright 2005-2012 the Seasar Foundation and the Others.
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
using Seasar.Dao.Attrs;

namespace Seasar.Tests.Dao.Impl
{
#if !NET_1_1
    [TimestampProperty("Ddate")]
    [Table("GENERIC_NULLABLE")]
    public class GenericNullableEntity
    {
        private Nullable<Decimal> _id;
        private Nullable<DateTime> _ddate;
        private int _entityNo;

        [ID(IDType.IDENTITY)]
        [ID(IDType.SEQUENCE, "SEQ_GENERIC_NULLABLE")]
        [Column("ID")]
        public Nullable<Decimal> ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public Nullable<DateTime> Ddate
        {
            get { return _ddate; }
            set { _ddate = value; }
        }

        public int EntityNo
        {
            get { return _entityNo; }
            set { _entityNo = value; }
        }
    }
#endif
}
