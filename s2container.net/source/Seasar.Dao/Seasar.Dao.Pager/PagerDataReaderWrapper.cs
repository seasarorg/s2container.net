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

using System.Data;
using Seasar.Extension.ADO.Impl;

namespace Seasar.Dao.Pager
{
    public class PagerDataReaderWrapper : DataReaderWrapper
    {
        private int _counter = 0;
        private readonly IDataReader _original;
        private readonly IPagerCondition _condition;

        public PagerDataReaderWrapper(IDataReader original, IPagerCondition condition)
            : base(original)
        {
            _original = original;
            _condition = condition;
            MoveOffset();
        }

        public override bool Read()
        {
            bool next = base.Read();
            if (_condition.Limit == PagerConditionConstants.NONE_LIMIT || _counter < (_condition.Offset + _condition.Limit) && next)
            {
                _counter++;
                return true;
            }
            else
            {
                if (next)
                {
                    _counter++;
                    while (_original.Read())
                    {
                        _counter++;
                    }
                }
                _condition.Count = _counter;
                return false;
            }
        }

        private void MoveOffset()
        {
            int row = 0;
            int offset = _condition.Offset;
            while (row < offset && _original.Read())
            {
                row++;
                _counter++;
            }
        }
    }
}
