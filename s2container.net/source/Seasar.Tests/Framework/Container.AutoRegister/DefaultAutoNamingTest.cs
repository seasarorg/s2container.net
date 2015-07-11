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

using MbUnit.Framework;
using Seasar.Framework.Container.AutoRegister;

namespace Seasar.Tests.Framework.Container.AutoRegister
{
    [TestFixture]
    public class DefaultAutoNamingTest
    {
        public void TestDefineName()
        {
            DefaultAutoNaming naming = new DefaultAutoNaming();
            Assert.AreEqual("Foo", naming.DefineName(typeof(Foo)), "1");
            Assert.AreEqual("Ijk", naming.DefineName(typeof(Ijk)), "2");
            Assert.AreEqual("Foo", naming.DefineName(typeof(IFoo)), "3");
            Assert.AreEqual("IBar", naming.DefineName(typeof(IBar)), "4");
        }

        private class Foo
        {
        }

        private class Ijk
        {
        }

        private interface IFoo
        {
        }

        private class IBar
        {
        }
    }
}
