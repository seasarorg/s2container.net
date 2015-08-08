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

namespace Seasar.Tests.Dao.Impl
{
    [Table("UNDER_SCORE")]
    public class UnderscoreEntity
    {
        public long UnderScoreNo { get; set; }

        public string Table_Name_ { get; set; }

        public string _Table_Name { get; set; }

        public string _Table_Name_ { get; set; }

        public string TableName { get; set; }
    }
}
