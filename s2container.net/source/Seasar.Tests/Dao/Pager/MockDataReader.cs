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

namespace Seasar.Tests.Dao.Pager
{
    public class MockDataReader : MockDataReaderBase
    {
        private int _total;
        private int _counter;
        private int _callNextCount;

        public MockDataReader()
        {
        }

        public MockDataReader(int total)
        {
            _total = total;
        }

        public override bool Read()
        {
            _callNextCount++;
            if (_counter <= _total)
            {
                _counter++;
                return true;
            }
            else
            {
                return false;
            }
        }

        public int Counter
        {
            get { return _counter; }
            set { _counter = value; }
        }

        public int CallNextCount
        {
            get { return _callNextCount; }
            set { _callNextCount = value; }
        }
    }
}
