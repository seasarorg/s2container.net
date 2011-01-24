#region Copyright
/*
 * Copyright 2005-2010 the Seasar Foundation and the Others.
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
    public sealed class BooleanConversionUtil
    {
        private BooleanConversionUtil()
        {
        }

        public static bool ToBoolean(object o)
        {
            if (o == null || o == DBNull.Value)
            {
                throw new ArgumentNullException("o");
            }
            else if (o is bool)
            {
                return (bool) o;
            }
            else if (o is byte)
            {
                return Convert.ToBoolean((byte) o);
            }
            else if (o is string)
            {
                string s = (string) o;
                if (string.Compare(bool.TrueString, s, true) == 0)
                {
                    return true;
                }
                else if (string.Compare(bool.FalseString, s, true) == 0)
                {
                    return false;
                }
                else if (s.Equals("0"))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
    }
}
