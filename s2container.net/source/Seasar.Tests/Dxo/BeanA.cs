/*
 * Created by: 
 * Created: 2007”N7ŒŽ2“ú
 */

using System;

namespace Seasar.Tests.Dxo 
{
    public class BeanA
    {
        private bool _flagToBool;
        private short _shortToBool;
        private int _intToBool;
        private long _longToBool;
        private string _stringToBool;

        private bool _flagToLong;
        private short _shortToLong;
        private int _intToLong;
        private long _longToLong;
        private string _stringToLong;

        private System.Nullable<bool> _nullBoolToNullLong;
        private System.Nullable<short> _nullShortToNullLong;
        private System.Nullable<int> _nullIntToNullLong;
        private System.Nullable<long> _nullLongToNullLong;

        private bool _flagToDouble;
        private short _shortToDouble;
        private int _intToDouble;
        private long _longToDouble;
        private string _stringToDouble;

        private char[] _charToString;
        private DateTime _dateToString;

        private char[] _charToChar;
        private string _stringToChar;

        private string _stringToDateTime;
        private long _longToDateTime;

        public bool FlagToBool
        {
            get { return _flagToBool; }
            set { _flagToBool = value; }
        }

        public short ShortToBool
        {
            get { return _shortToBool; }
            set { _shortToBool = value; }
        }

        public int IntToBool
        {
            get { return _intToBool; }
            set { _intToBool = value; }
        }

        public long LongToBool
        {
            get { return _longToBool; }
            set { _longToBool = value; }
        }

        public string StringToBool
        {
            get { return _stringToBool; }
            set { _stringToBool = value; }
        }

        public bool FlagToLong
        {
            get { return _flagToLong; }
            set { _flagToLong = value; }
        }

        public short ShortToLong
        {  
            get { return _shortToLong; }
            set { _shortToLong = value; }
        }

        public int IntToLong
        {
            get { return _intToLong; }
            set { _intToLong = value; }
        }

        public long LongToLong
        {
            get { return _longToLong; }
            set { _longToLong = value; }
        }

        public string StringToLong
        {
            get { return _stringToLong; }
            set { _stringToLong = value; }
        }

        public System.Nullable<bool> NullBoolToNullLong
        {
            get { return _nullBoolToNullLong; }
            set { _nullBoolToNullLong = value; }
        }

        public System.Nullable<short> NullShortToNullLong
        {
            get { return _nullShortToNullLong; }
            set { _nullShortToNullLong = value; }
        }

        public System.Nullable<int> NullIntToNullLong
        {
            get { return _nullIntToNullLong; }
            set { _nullIntToNullLong = value; }
        }

        public System.Nullable<long> NullLongToNullLong
        {
            get { return _nullLongToNullLong; }
            set { _nullLongToNullLong = value; }
        }

        public bool FlagToDouble
        {
            get { return _flagToDouble; }
            set { _flagToDouble = value; }
        }

        public short ShortToDouble
        {
            get { return _shortToDouble; }
            set { _shortToDouble = value; }
        }

        public int IntToDouble
        {
            get { return _intToDouble; }
            set { _intToDouble = value; }
        }

        public long LongToDouble
        {
            get { return _longToDouble; }
            set { _longToDouble = value; }
        }

        public string StringToDouble
        {
            get { return _stringToDouble; }
            set { _stringToDouble = value; }
        }

        public char[] CharToString
        {
            get { return _charToString; }
            set { _charToString = value; }
        }

        public DateTime DateToString
        {
            get { return _dateToString; }
            set { _dateToString = value; }
        }

        public char[] CharToChar
        {
            get { return _charToChar; }
            set { _charToChar = value; }
        }

        public string StringToChar
        {
            get { return _stringToChar; }
            set { _stringToChar = value; }
        }

        public string StringToDateTime
        {
            get { return _stringToDateTime; }
            set { _stringToDateTime = value; }
        }

        public long LongToDateTime
        {
            get { return _longToDateTime; }
            set { _longToDateTime = value; }
        }
    }
}