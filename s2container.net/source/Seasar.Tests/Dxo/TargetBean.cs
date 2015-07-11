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
        private bool _flagToBool;
        private bool _shortToBool;
        private bool _intToBool;
        private bool _longToBool;
        private bool _stringToBool;

        private long _flagToLong;
        private long _shortToLong;
        private long _intToLong;
        private long _longToLong;
        private long  _stringToLong;

        private System.Nullable<long> _nullBoolToNullLong;
        private System.Nullable<long> _nullShortToNullLong;
        private System.Nullable<long> _nullIntToNullLong;
        private System.Nullable<long> _nullLongToNullLong;

        private double _flagToDouble;
        private double _shortToDouble;
        private double _intToDouble;
        private double _longToDouble;
        private double _stringToDouble;

        private string _charToString;
        private string _dateToString;

        private char[] _charToChar;
        private char[] _stringToChar;

        private DateTime _stringToDateTime;
        private DateTime _longToDateTime;

        public bool FlagToBool
        {
            get { return _flagToBool; }
            set { _flagToBool = value; }
        }

        public bool ShortToBool
        {
            get { return _shortToBool; }
            set { _shortToBool = value; }
        }

        public bool IntToBool
        {
            get { return _intToBool; }
            set { _intToBool = value; }
        }

        public bool LongToBool
        {
            get { return _longToBool; }
            set { _longToBool = value; }
        }

        public bool StringToBool
        {
            get { return _stringToBool; }
            set { _stringToBool = value; }
        }

        public long FlagToLong
        {
            get { return _flagToLong; }
            set { _flagToLong = value; }
        }

        public long ShortToLong
        {
            get { return _shortToLong; }
            set { _shortToLong = value; }
        }

        public long IntToLong
        {
            get { return _intToLong; }
            set { _intToLong = value; }
        }

        public long LongToLong
        {
            get { return _longToLong; }
            set { _longToLong = value; }
        }

        public long StringToLong
        {
            get { return _stringToLong; }
            set { _stringToLong = value; }
        }

        public System.Nullable<long> NullBoolToNullLong
        {
            get { return _nullBoolToNullLong; }
            set { _nullBoolToNullLong = value; }
        }

        public System.Nullable<long> NullShortToNullLong
        {
            get { return _nullShortToNullLong; }
            set { _nullShortToNullLong = value; }
        }

        public System.Nullable<long> NullIntToNullLong
        {
            get { return _nullIntToNullLong; }
            set { _nullIntToNullLong = value; }
        }

        public System.Nullable<long> NullLongToNullLong
        {
            get { return _nullLongToNullLong; }
            set { _nullLongToNullLong = value; }
        }

        public double FlagToDouble
        {
            get { return _flagToDouble; }
            set { _flagToDouble = value; }
        }

        public double ShortToDouble
        {
            get { return _shortToDouble; }
            set { _shortToDouble = value; }
        }

        public double IntToDouble
        {
            get { return _intToDouble; }
            set { _intToDouble = value; }
        }

        public double LongToDouble
        {
            get { return _longToDouble; }
            set { _longToDouble = value; }
        }

        public double StringToDouble
        {
            get { return _stringToDouble; }
            set { _stringToDouble = value; }
        }

        public string CharToString
        {
            get { return _charToString; }
            set { _charToString = value; }
        }

        public string DateToString
        {
            get { return _dateToString; }
            set { _dateToString = value; }
        }

        public char[] CharToChar
        {
            get { return _charToChar; }
            set { _charToChar = value; }
        }

        public char[] StringToChar
        {
            get { return _stringToChar; }
            set { _stringToChar = value; }
        }

        public DateTime StringToDateTime
        {
            get { return _stringToDateTime; }
            set { _stringToDateTime = value; }
        }

        public DateTime LongToDateTime
        {
            get { return _longToDateTime; }
            set { _longToDateTime = value; }
        }
    }
}
