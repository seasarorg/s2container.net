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

namespace Seasar.Tests.Dxo 
{
    public class EmployeePage
    {
        private string _dname;
        private string _ename;
        private int? _id;
        private string _name;


        public string DName
        {
            get { return _dname; }
            set { _dname = value; }
        }

        public string EName
        {
            get { return _ename; }
            set { _ename = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int? Id
        {
            get { return _id; }
            set { _id = value; }
        }
    }
}
