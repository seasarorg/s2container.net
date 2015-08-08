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
    public class BeanA
    {
        public bool FlagToBool { get; set; }

        public short ShortToBool { get; set; }

        public int IntToBool { get; set; }

        public long LongToBool { get; set; }

        public string StringToBool { get; set; }

        public bool FlagToLong { get; set; }

        public short ShortToLong { get; set; }

        public int IntToLong { get; set; }

        public long LongToLong { get; set; }

        public string StringToLong { get; set; }

        public bool? NullBoolToNullLong { get; set; }

        public short? NullShortToNullLong { get; set; }

        public int? NullIntToNullLong { get; set; }

        public long? NullLongToNullLong { get; set; }

        public bool FlagToDouble { get; set; }

        public short ShortToDouble { get; set; }

        public int IntToDouble { get; set; }

        public long LongToDouble { get; set; }

        public string StringToDouble { get; set; }

        public char[] CharToString { get; set; }

        public DateTime DateToString { get; set; }

        public char[] CharToChar { get; set; }

        public string StringToChar { get; set; }

        public string StringToDateTime { get; set; }

        public long LongToDateTime { get; set; }
    }
}
