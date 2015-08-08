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

using Seasar.Dao.Attrs;

namespace Seasar.Tests.Dao.Id
{
#if !NET_1_1

    public class HogeSystemNullableDecimal
    {
        [ID(IDType.IDENTITY)]
        public decimal? Id { set; get; }
    }

    public class HogeSystemNullableInt
    {
        [ID(IDType.IDENTITY)]
        public int? Id { set; get; }
    }

    public class HogeSystemNullableShort
    {
        [ID(IDType.IDENTITY)]
        public short? Id { set; get; }
    }

    public class HogeSystemNullableLong
    {
        [ID(IDType.IDENTITY)]
        public long? Id { set; get; }
    }
    public class HogeSystemNullableFloat
    {
        [ID(IDType.IDENTITY)]
        public float? Id { set; get; }
    }
    public class HogeSystemNullableDouble
    {
        [ID(IDType.IDENTITY)]
        public double? Id { set; get; }
    }

#endif
}
