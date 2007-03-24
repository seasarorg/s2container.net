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

namespace Seasar.Framework.Util
{
    public sealed class DecimalConversionUtil
    {
        private DecimalConversionUtil()
        {
        }

        public static decimal ToDecimal(object o)
        {
            return ToDecimal(o, null);
        }

        public static decimal ToDecimal(object o, string pattern)
        {
            if (o == null || o == DBNull.Value)
            {
                throw new ArgumentNullException("o");
            }
            else if (o is decimal)
            {
                return (decimal) o;
            }
            else if (o is bool)
            {
                return Convert.ToDecimal((bool) o);
            }
            else if (o is byte)
            {
                return Convert.ToDecimal((byte) o);
            }
            else if (o is string)
            {
                return Convert.ToDecimal((string) o);
            }
            else if (o is double)
            {
                return Convert.ToDecimal((double) o);
            }
            else if (o is int)
            {
                return Convert.ToDecimal((int) o);
            }
            else if (o is long)
            {
                return Convert.ToDecimal((long) o);
            }
            else if (o is short)
            {
                return Convert.ToDecimal((short) o);
            }
            else if (o is float)
            {
                return Convert.ToDecimal((float) o);
            }
            else if (o is DateTime)
            {
                return Convert.ToDecimal(((DateTime) o).Ticks);
            }
            else
            {
                return Convert.ToDecimal(o.ToString());
            }
        }
    }
}
