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

using System;

namespace Seasar.Tests.Dxo 
{
    public class TargetBean
    {
        public bool FlagToBool { get; set; }

        public bool ShortToBool { get; set; }

        public bool IntToBool { get; set; }

        public bool LongToBool { get; set; }

        public bool StringToBool { get; set; }

        public long FlagToLong { get; set; }

        public long ShortToLong { get; set; }

        public long IntToLong { get; set; }

        public long LongToLong { get; set; }

        public long StringToLong { get; set; }

        public long? NullBoolToNullLong { get; set; }

        public long? NullShortToNullLong { get; set; }

        public long? NullIntToNullLong { get; set; }

        public long? NullLongToNullLong { get; set; }

        public double FlagToDouble { get; set; }

        public double ShortToDouble { get; set; }

        public double IntToDouble { get; set; }

        public double LongToDouble { get; set; }

        public double StringToDouble { get; set; }

        public string CharToString { get; set; }

        public string DateToString { get; set; }

        public char[] CharToChar { get; set; }

        public char[] StringToChar { get; set; }

        public DateTime StringToDateTime { get; set; }

        public DateTime LongToDateTime { get; set; }
    }
}
