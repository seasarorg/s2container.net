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

using Seasar.Dao.Attrs;

namespace Seasar.Tests.Dao.Impl
{
    public class EmployeeSearchCondition
    {
        private Department _department;
        private string _job;
        private string _dname;
        private string _orderByString;
        public Department Department
        {
            set { _department = value; }
            get { return _department; }
        }

        [Column("dname_0")]
        public string Dname
        {
            set { _dname = value; }
            get { return _dname; }
        }

        public string Job
        {
            set { _job = value; }
            get { return _job; }
        }

        public string OrderByString
        {
            set { _orderByString = value; }
            get { return _orderByString; }
        }
    }
}
