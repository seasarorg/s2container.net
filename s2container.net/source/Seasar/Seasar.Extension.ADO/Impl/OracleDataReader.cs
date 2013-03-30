#region Copyright
/*
 * Copyright 2005-2013 the Seasar Foundation and the Others.
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
using System.Text;

namespace Seasar.Extension.ADO.Impl
{
    public class OracleDataReader : DataReaderWrapper
    {
        private const char WAVE_DASH = (char) 0x301c;

        private const char FULLWIDTH_TILDE = (char) 0xff5e;

        public OracleDataReader(IDataReader original)
            : base(original)
        {
        }

        public override string GetString(int i)
        {
            return Convert(base.GetString(i));
        }

        public override object this[int i]
        {
            get
            {
                object ret = base[i];
                if (ret is string)
                {
                    return Convert(ret as string);
                }
                return ret;
            }
        }

        public override object this[string name]
        {
            get
            {
                object ret = base[name];
                if (ret is string)
                {
                    return Convert(ret as string);
                }
                return ret;
            }
        }

        protected virtual string Convert(string source)
        {
            if (source == null)
            {
                return null;
            }
            StringBuilder result = new StringBuilder(source.Length);
            for (int i = 0; i < source.Length; i++)
            {
                char ch = source[i];
                switch (ch)
                {
                    case WAVE_DASH: // WAVE DASH(U+301C) -> FULLWIDTH TILDE(U+FF5E)
                        ch = FULLWIDTH_TILDE;
                        break;
                    default:
                        break;
                }
                result.Append(ch);
            }
            return result.ToString();
        }
    }
}
