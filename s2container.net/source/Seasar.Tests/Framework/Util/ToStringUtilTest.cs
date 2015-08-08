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

using System.Collections;
using MbUnit.Framework;
using Seasar.Framework.Util;

namespace Seasar.Tests.Framework.Util
{
    [TestFixture]
    public class ToStringUtilTest
    {
        [Test]
        public void ToStringIList()
        {
            var arrayChild = new ArrayList();
            arrayChild.Add("valueChild1");
            arrayChild.Add("valueChild2");

            var array = new ArrayList();
            array.Add("value1");
            array.Add("value2");

            array.Add(arrayChild);

            Assert.AreEqual(
                "{value1, value2, {valueChild1, valueChild2}}",
                ToStringUtil.ToString(array)
                );
        }
    }
}
