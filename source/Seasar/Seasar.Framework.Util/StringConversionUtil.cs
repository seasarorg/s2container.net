#region Copyright
/*
 * Copyright 2005 the Seasar Foundation and the Others.
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
using System.Globalization;

namespace Seasar.Framework.Util
{
    public sealed class StringConversionUtil
    {
        private StringConversionUtil()
        {
        }

        public static string ToString(object value)
        {
            return ToString(value, null);
        }

        public static string ToString(object value, string pattern)
        {
            if(value == null)
            {
                return null;
            }
            else if(value is string)
            {
                return (string) value;
            }
            else if(value is IFormattable)
            {
                return ToString((IFormattable) value, pattern);
            }
            else
            {
                return value.ToString();
            }
        }

        public static string ToString(IFormattable value, string pattern)
        {
            if(value != null)
            {
                if(pattern != null)
                {
                    if(value is DateTime)
                    {
                        return value.ToString(pattern, DateTimeFormatInfo.CurrentInfo);
                    }
                    else
                    {
                        return value.ToString(pattern, NumberFormatInfo.CurrentInfo);
                    }
                }
                else
                {
                    return ((object) value).ToString();
                }
            }
            else
            {
                return null;
            }
        }
    }
}
