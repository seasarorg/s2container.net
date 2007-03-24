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
using System.Data;
using System.Text;

namespace Seasar.Framework.Util
{
    public class DataReaderInspector
    {
        private readonly IDataReader _reader;

        public DataReaderInspector(IDataReader reader)
        {
            _reader = reader;
        }

        public string Text
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                do
                {
                    int i = _reader.FieldCount;
                    for (int j = 0; j < i; j++)
                    {
                        sb.Append(_reader.GetName(j) + "|");
                    }
                    sb.Append(Environment.NewLine);

                    while (_reader.Read())
                    {
                        for (int j = 0; j < i; j++)
                        {
                            sb.Append(_reader.GetValue(j).ToString() + "|");
                        }
                        sb.Append(Environment.NewLine);
                    }
                    sb.Append(Environment.NewLine);
                }
                while (_reader.NextResult());

                return sb.ToString();
            }
        }
    }
}
