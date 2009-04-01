using System;

namespace Seasar.Tests.Dxo
{
    public class DateTimeBean
    {
        private DateTime _dateTimeToString;

        public DateTime DateTimeToString
        {
            get { return _dateTimeToString; }
            set { _dateTimeToString = value; }
        }
    }
}